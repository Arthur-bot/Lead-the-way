using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] GameObject virtualCamera;
    [SerializeField] Collider2D roomBoundary;

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            virtualCamera.SetActive(true);
        }

    }

    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            virtualCamera.SetActive(false);
        }
    }

}
