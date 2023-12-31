using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BallSpeedReporter : MonoBehaviour
{
    [HideInInspector] public bool AbleToBeHit = true;
    [HideInInspector] public bool isReal = true;
    private Rigidbody2D rb2d;
    private Collider2D collider;
    [SerializeField] private TextMeshProUGUI DebugText;
    [SerializeField] GameObject originalParent;
    //[SerializeField] GameObject BackCircle;
    //[SerializeField] GameObject InnerSplitCircleParent;
    [SerializeField] GameObject WaveCircleParent;
    Transform[] WaveCircles;
    bool grounded = false;
    private SpriteRenderer ballSprite;
    private Transform ballTrail;
    //private Transform ballParticle;
    private Transform BallDashedLineParent;
    private RectTransform[] BallDashedLines;
    private List<Transform> BallInnerCurveLines;
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
        //BallDashedLineParent = transform.Find("DashedLineParent");
        //BallDashedLines = BallDashedLineParent.GetComponentsInChildren<RectTransform>();
        //BallInnerCurveLines = InnerSplitCircleParent.GetComponentsInChildren<Transform>().ToListPooled();
        //originalRotSinDist = BallDashedLines[1].anchoredPosition.y;
        //originalInnerRotSinDist = BallInnerCurveLines[2].localPosition.y;
        //BallDashedLineParent.gameObject.SetActive(false);
        //BackCircle.gameObject.SetActive(true);
        WaveCircles = WaveCircleParent.GetComponentsInChildren<Transform>();
        origWaveCircleScale = WaveCircles[1].transform.localScale.x;
        originalWaveCircleColor = WaveCircles[1].GetComponent<SpriteRenderer>().color;
        //ballParticle = transform.Find("Particle");
        //ballParticle.gameObject.SetActive(false);
        originalSca = transform.localScale;
        distToGround = collider.bounds.extents.y;
    }
    float rotSinTime = 0;
    private void Update()
    {
        timeSinceShot += Time.deltaTime;
        //DebugText.text = transform.parent.ToString();
        UpdateGrounded();
        //DebugText.text = gameObject.transform.parent.ToString();
        //DebugText.text = ((rb2d.velocity.magnitude * rb2d.mass) / 4).ToString() + " " + grounded.ToString();
        //DebugText.text = AbleToBeHit.ToString();
        if (grounded)
        {
            //rb2d.velocity = Vector2.zero;
            //UpdateDashedCircle();
            UpdateWaveCircles();
            AbleToBeHit = true;
            //ballSprite.material.color = hittable;
            if (!LastTimeWasEnabled)
            {
                    //BackCircle.gameObject.SetActive(false);
                    //ballTrail.gameObject.SetActive(false);
                    WaveCircleParent.SetActive(true);
                    //BallDashedLineParent.gameObject.SetActive(true);
                    //InnerSplitCircleParent.gameObject.SetActive(true);
                    //ballParticle.gameObject.SetActive(true);\
            }
            LastTimeWasEnabled = true;
        }
        else
        {
            rotSinTime = 0;
            WaveCircleTime = 0;
            AbleToBeHit = false;
            //ballSprite.material.color = notHittable;
            if (LastTimeWasEnabled)
            {
                //BackCircle.gameObject.SetActive(true);
                //ballTrail.gameObject.SetActive(true);
                WaveCircleParent.SetActive(false);
                for (int i = 0; i < WaveCircleRenderers.Length; i++)
                {
                    WaveCircles[i + 1].transform.localScale = new Vector2(origWaveCircleScale, origWaveCircleScale);
                }
                //BallDashedLineParent.gameObject.SetActive(false);
                //InnerSplitCircleParent.gameObject.SetActive(false);
                //ballParticle.gameObject.SetActive(false);
            }
            LastTimeWasEnabled = false;
        }
    }
    public void BallShot()
    {
        timeSinceShot = 0;
    }
    private float timeSinceShot;
    private bool InGoal = false;
    public void SetInGoal()
    {
        InGoal = true;
        //BackCircle.gameObject.SetActive(true);
        ballTrail.gameObject.SetActive(false);
        WaveCircleParent.SetActive(false);
        //BallDashedLineParent.gameObject.SetActive(false);
        //InnerSplitCircleParent.gameObject.SetActive(false);
    }
    private float WaveCircleTime = 0;
    private const float MaxWaveCircleTime = 1;
    private float origWaveCircleScale;
    private Color originalWaveCircleColor;
    private SpriteRenderer[] WaveCircleRenderers = null;
    //private float[] WaveStarts = { 0, 0.15f, 0.3f, 0.45f, 0.6f, 0.75f };
    private float[] WaveStarts = { 0, 0.08f, 0.16f, 0.24f, 0.32f, 0.40f };
    private float WaveCircleMaxcale = 2.5f;
    const float multiCircleOffset = 0.2f;
    private void UpdateWaveCircles()
    {
        if (WaveCircleRenderers == null)
        {
            WaveCircleRenderers = new SpriteRenderer[WaveCircles.Length - 1];
            for (int i = 1; i < WaveCircles.Length; i++)
            {
                WaveCircleRenderers[i - 1] = WaveCircles[i].GetComponent<SpriteRenderer>();
            }
        }
        WaveCircleTime += Time.deltaTime;
        for (int i = 0; i < WaveCircleRenderers.Length; i++)
        {
            float localWaveCircleTime = WaveCircleTime % MaxWaveCircleTime;
            float pT = localWaveCircleTime / MaxWaveCircleTime;
            pT -= WaveStarts[i];
            float coW = (1 - Mathf.Cos(2 * Mathf.PI * pT)) / 2;
            float scale;
            if ((pT < 0.5) && (pT >= 0))
            {
                scale = (origWaveCircleScale * coW * WaveCircleMaxcale) + origWaveCircleScale + (multiCircleOffset * (WaveCircleRenderers.Length - 1));
                WaveCircles[i + 1].transform.localScale = new Vector2(scale, scale);
                WaveCircleRenderers[i].color = originalWaveCircleColor;
            }
            else if (pT <= 1)
            {
                Color origAlp = originalWaveCircleColor;
                origAlp.a = coW * originalWaveCircleColor.a;
                WaveCircleRenderers[i].color = origAlp;
                //WaveCircles[1].transform.localScale = new Vector2(origWaveCircleScale, origWaveCircleScale);
            }
        }
    }
    private void UpdateGrounded()
    {
        if(timeSinceShot > 1000000000)
        {
            grounded = true;
        }
        else if (!grounded)
        {
            if (GetGrounded())
                FramesThoughtGrounded++;
            else
                FramesThoughtGrounded = 0;
            if ((FramesThoughtGrounded > 9) && (colllGroundAmount > 0) && (rb2d.velocity.magnitude < 0.2))
            {
                grounded = true;
            }
        }
    }
    private int colllGroundAmount = 0;
    private void OnCollisionExit2D(Collision2D collision)
    {
        colllGroundAmount--;
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
        colllGroundAmount++;
            if (col.gameObject.transform.parent != null)
            {
                if (col.gameObject.CompareTag("Movable") || col.gameObject.CompareTag("Well"))
                {
                if(gameObject.transform.parent.transform.localScale.x == gameObject.transform.parent.transform.localScale.y)
                    gameObject.transform.SetParent(col.gameObject.transform.parent.transform, true);
                }
            }
    }

    float prevGroundedDist = -1;
    private bool GetGrounded()
    {
        if (transform.parent == null) return false;

        float dist = (transform.position - transform.parent.position).magnitude;
        if (Mathf.Abs(dist - prevGroundedDist) < 0.2)
        {
            return true;
        }
        else
        {
            prevGroundedDist = dist;
            return false;
        }

        //RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, -Vector2.up, distToGround + 0.01f);
        //if (hits.Length > 1)
        //    return true;
        //return false;
    }
}