using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallSpeedReporter : MonoBehaviour
{
    [HideInInspector] public bool AbleToBeHit = true;
    [HideInInspector] public bool isReal = true;
    private Rigidbody2D rb2d;
    private Collider2D collider;
    [SerializeField] private TextMeshProUGUI DebugText;
    [SerializeField] GameObject originalParent;
    bool grounded = false;
    private SpriteRenderer ballSprite;
    private Color hittable = new(1, 1, 1, 1);
    private Color notHittable = new(1, 1, 1, 0.5f);
    private Vector2 originalSca;
    private float distToGround;
    int FramesThoughtGrounded;
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        ballSprite = GetComponent<SpriteRenderer>();
        originalSca = transform.localScale;
        distToGround = collider.bounds.extents.y;
    }
    private void Update()
    {
        //DebugText.text = gameObject.transform.parent.ToString();
        //DebugText.text = ((rb2d.velocity.magnitude * rb2d.mass) / 4).ToString() + " " + grounded.ToString();
        //DebugText.text = AbleToBeHit.ToString();
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
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!grounded)
        {
            if (GetGrounded() && (rb2d.velocity.magnitude < 0.2))
                FramesThoughtGrounded++;
            else
                FramesThoughtGrounded = 0;
            if (FramesThoughtGrounded > 9)
            {
                grounded = true;
            }
        }

        //grounded = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {

        grounded = false;
        if (isReal)
        {
            if (originalParent != null)
            {
                //gameObject.transform.parent = originalParent.transform;
                gameObject.transform.SetParent(originalParent.transform, true);
                //gameObject.transform.localScale = originalSca;
            }
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if(GetGrounded())
        {
            //FramesThoughtGrounded = 1;
            //grounded = true;
        }
        if (isReal && (col.gameObject.transform.parent != null))
        {
            if (col.gameObject.CompareTag("Movable"))
            {
                gameObject.transform.SetParent(col.gameObject.transform.parent.transform, true);
            }
            //gameObject.transform.localScale = new Vector3(
            //gameObject.transform.localScale.x / col.gameObject.transform.localScale.x,
            //gameObject.transform.localScale.y / col.gameObject.transform.localScale.y,
            //gameObject.transform.localScale.z / col.gameObject.transform.localScale.z);
            //gameObject.transform.localScale = originalSca / col.gameObject.transform.localScale;
        }
    }
    private bool GetGrounded()
    {
        RaycastHit2D[] hits =  Physics2D.RaycastAll(transform.position, -Vector2.up, distToGround + 0.01f);
        if (hits.Length > 1)
            return true;
        return false;
    }
} 
