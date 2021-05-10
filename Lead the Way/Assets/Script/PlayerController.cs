using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float movSpeed;
    [SerializeField] Rigidbody2D rb;
    Vector3 velocity = Vector3.zero;
    int direction = 1;
    bool isMoving;
    bool isGrounded;
    bool canDie = true;

    [Header("Jump")]
    [SerializeField] float jumpForce;
    [SerializeField] float xWallForce;
    [SerializeField] float yWallForce;
    [SerializeField] float wallJumpTime;
    [SerializeField] float wallSlidingSpeed;
    bool wallJumpingFace;
    bool wallJumping;
    bool isFacingWall;


    [Header("Check")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform wallCheckRight;
    [SerializeField] Transform wallCheckLeft;
    [SerializeField] LayerMask wallLayer;
    bool isTouchingWall;
    bool wallSliding;

    [Header("Animation")]
    [SerializeField] Animator anim;
    [SerializeField] GameObject landingEffect;
    bool spawnLandingEffect;

    [Header("Sound")]
    [SerializeField] AudioClip landingAudio;
    [SerializeField] AudioClip deathAudio;



    Dictionary<string, Action> keywordActions = new Dictionary<string, Action>();
    KeywordRecognizer keywordRecognizer;

    // Start is called before the first frame update
    void Start()
    {
        keywordActions.Add("Start", StartMoving);
        keywordActions.Add("Stop", StopMoving);
        keywordActions.Add("Droite", MoveRight);
        keywordActions.Add("Gauche", MoveLeft);
        keywordActions.Add("Hop", Jump);

        keywordRecognizer = new KeywordRecognizer(keywordActions.Keys.ToArray(), ConfidenceLevel.Low);
        keywordRecognizer.OnPhraseRecognized += OnKeywordRecognizer;
        keywordRecognizer.Start();
    }

    void OnKeywordRecognizer(PhraseRecognizedEventArgs args)
    {
        keywordActions[args.text].Invoke();
    }

    private void FixedUpdate()
    {
        // Check touching ground and wall
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.15f, groundLayer);
        isTouchingWall = (Physics2D.OverlapCircle(wallCheckRight.position, 0.15f, wallLayer) || Physics2D.OverlapCircle(wallCheckLeft.position, 0.15f, wallLayer));

        // Update direction for animation
        anim.SetBool("IsInAir", !isGrounded);
        anim.SetFloat("Direction", direction);
        anim.SetBool("WallSlide", wallSliding);


        if (isTouchingWall && !isGrounded)
        {
            wallSliding = true;
        }
        else
        {
            wallSliding = false;
        }

        if (isMoving)
        {
            MovePlayer();
        }
        else
        {
            anim.SetBool("IsMoving", false);
        }

        if (wallJumpingFace)
        {
            rb.velocity = new Vector2(xWallForce * -direction, yWallForce);
        }
        else if (wallJumping)
        {
            rb.velocity = new Vector2(6 * direction, jumpForce);
        }
    }

    void MovePlayer()
    {
        if (wallSliding)
        {
            rb.velocity = Vector3.zero;

            if (Physics2D.OverlapCircle(wallCheckRight.position, 0.15f, wallLayer) && direction == 1)
            {
                isFacingWall = true;
            }
            else if (Physics2D.OverlapCircle(wallCheckLeft.position, 0.15f, wallLayer) && direction == -1)
            {
                isFacingWall = true;
            }
            else
            {
                isFacingWall = false;
            }

            anim.SetBool("FacingWall", isFacingWall);
        }
        else
        {
            float horizontalMovement = direction * movSpeed * Time.deltaTime;
            Vector3 movement = new Vector2(horizontalMovement, rb.velocity.y);

            rb.velocity = Vector3.SmoothDamp(rb.velocity, movement, ref velocity, .05f);
        }

        anim.SetBool("IsMoving", true);

        if (isGrounded)
        {
            if (spawnLandingEffect)
            {
                Instantiate(landingEffect, groundCheck.position, Quaternion.identity);
                spawnLandingEffect = false;
                UserInterfaceAudio.PlayClip(landingAudio);
            }
        }
        else
        {
            spawnLandingEffect = true;
        }
    }

    void StartMoving()
    {
        isMoving = true;
    }

    void StopMoving()
    {
        isMoving = false;
        rb.velocity = Vector3.zero;
    }

    void MoveRight()
    {
        direction = 1;
    }

    void MoveLeft()
    {
        direction = -1;
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        else if (wallSliding)
        {
            if (isFacingWall)
                wallJumpingFace = true;
            else
                wallJumping = true;

            Invoke("SetWallJumpingFalse", wallJumpTime);
        }
    }

    void SetWallJumpingFalse()
    {
        wallJumping = false;
        wallJumpingFace = false;
    }

    void PlayLandingSound()
    {
        UserInterfaceAudio.PlayClip(landingAudio);
    }

    public void Death()
    {
        if(canDie)
        {
            isMoving = false;
            canDie = false;

            keywordRecognizer.Dispose();
            Destroy(rb);
            anim.SetBool("Death", true);
            UserInterfaceAudio.PlayClip(deathAudio);
        }
    }

    void RestartLevel()
    {
        LevelManager.Instance.RestartLevel();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Death"))
        {
            Death();
        }
    }
}
