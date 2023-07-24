using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallSpeedReporter : MonoBehaviour
{
    public bool AbleToBeHit = true;
    private Rigidbody2D rb2d;
    private Collider2D collider;
    [SerializeField] private TextMeshProUGUI DebugText;
    bool grounded = false;
    private SpriteRenderer ballSprite;
    private Color hittable = new(0, 1, 1, 1);
    private Color notHittable = new(1f, 0.5f, 0.5f, 1);
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        ballSprite = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        //DebugText.text = ((rb2d.velocity.magnitude * rb2d.mass) / 4).ToString();
        DebugText.text = AbleToBeHit.ToString();
        if ((rb2d.velocity.magnitude < 0.2) && grounded)
        {
            //rb2d.velocity = Vector2.zero;
            AbleToBeHit = true;
            ballSprite.material.color = hittable;
        }
        else
        {
            AbleToBeHit = false;
            ballSprite.material.color = notHittable;
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
