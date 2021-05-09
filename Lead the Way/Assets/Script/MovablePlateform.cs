using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePlateform : MonoBehaviour
{
    [SerializeField] float velocity;
    [SerializeField] Transform[] points;
    [SerializeField] GameObject wheel;

    int index = 0;

    private void FixedUpdate()
    {
        if(Vector3.Distance(transform.position, points[index].position) < 0.1f)
        {
            transform.position += Vector3.zero;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, points[index].position, velocity * Time.deltaTime);
            wheel.transform.Rotate(0f, 0f, -2f, Space.Self);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            index = 1;
            collision.collider.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            index = 0;
            collision.collider.transform.SetParent(null);
        }
    }
}
