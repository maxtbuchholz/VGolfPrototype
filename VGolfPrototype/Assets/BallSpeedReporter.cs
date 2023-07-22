using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpeedReporter : MonoBehaviour
{
    private bool AbleToBeHit = true;
    private Rigidbody2D rb2d;
    private Collider2D collider;
    bool grounded = false;
    private SpriteRenderer ballSprite;
    private Color hittable = new(1, 1, 1);
    private Color notHittable = new(0.5f, 0.5f, 0.5f);
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        ballSprite = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if ((rb2d.velocity.magnitude < 0.2) && grounded)
        {
            //rb2d.velocity = Vector2.zero;
            AbleToBeHit = true;
            ballSprite.material.SetColor("White", hittable);
        }
        else
        {
            AbleToBeHit = false;
            ballSprite.material.SetColor("Gray", Color.red);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        grounded = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        grounded = false;
    }
}
