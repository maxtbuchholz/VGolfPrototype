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
    [SerializeField] GameObject BackCircle;
    bool grounded = false;
    private SpriteRenderer ballSprite;
    private Transform ballTrail;
    //private Transform ballParticle;
    private Transform BallDashedLineParent;
    private RectTransform[] BallDashedLines;
    private Color hittable = new(1, 1, 1, 1);
    private Color notHittable = new(0.8f, 0.8f, 0.8f, 1);
    private Vector2 originalSca;
    private float distToGround;
    int FramesThoughtGrounded;
    private bool LastTimeWasEnabled = false;
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        ballSprite = GetComponent<SpriteRenderer>();
        ballTrail = transform.Find("Trail");
        BallDashedLineParent = transform.Find("DashedLineParent");
        BallDashedLines = BallDashedLineParent.GetComponentsInChildren<RectTransform>();
        originalRotSinDist = BallDashedLines[1].anchoredPosition.y;
        BallDashedLineParent.gameObject.SetActive(false);
        BackCircle.gameObject.SetActive(true);
        //ballParticle = transform.Find("Particle");
        //ballParticle.gameObject.SetActive(false);
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
            UpdateDashedCircle();
            AbleToBeHit = true;
            //ballSprite.material.color = hittable;
            if (!LastTimeWasEnabled)
            {
                BackCircle.gameObject.SetActive(false);
                ballTrail.gameObject.SetActive(false);
                BallDashedLineParent.gameObject.SetActive(true);
                //ballParticle.gameObject.SetActive(true);
            }
            LastTimeWasEnabled = true;
        }
        else
        {
            rotSinTime = 0;
            AbleToBeHit = false;
            //ballSprite.material.color = notHittable;
            if (LastTimeWasEnabled)
            {
                BackCircle.gameObject.SetActive(true);
                ballTrail.gameObject.SetActive(true);
                BallDashedLineParent.gameObject.SetActive(false);
                //ballParticle.gameObject.SetActive(false);
            }
            LastTimeWasEnabled = false;
        }
    }
    private float prevRotation = 0;
    private float rotSpeed = 40;
    private float rotSinTime = 0;
    private float originalRotSinDist;
    private float rotSinDistance = 3f;
    private float rotSinSpeed = 4;
    private void UpdateDashedCircle()
    {
        float delTim = Time.deltaTime;
        prevRotation = (prevRotation + (rotSpeed * delTim)) % 360;
        BallDashedLineParent.transform.eulerAngles = new Vector3(0, 0, prevRotation);
        rotSinTime += (delTim * rotSinSpeed);
        rotSinTime %= 6.283185f; //mod by 2pi
        float s = rotSinDistance * -Mathf.Cos(rotSinTime);
        s += originalRotSinDist;
        BallDashedLines[1].anchoredPosition = new Vector2(0, s);
        BallDashedLines[2].anchoredPosition = new Vector2(s, 0);
        BallDashedLines[3].anchoredPosition = new Vector2(0, -s);
        BallDashedLines[4].anchoredPosition = new Vector2(-s, 0);
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
