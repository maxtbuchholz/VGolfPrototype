using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class TouchHandler : MonoBehaviour
{
    private GameObject Ball;
    private Rigidbody2D BallRB;
    [SerializeField] Camera camera;
    [SerializeField] ScoreController scoreController;
    [SerializeField] Projection projection;
    [SerializeField] GameObject PullBackJoystick;
    [SerializeField] BallSpeedReporter BallSpeed;
    [SerializeField] private TextMeshProUGUI DebugText;
    [SerializeField] JoyStickShape joyStickShape;
    private List<int> activeTouches;
    private List<int> prevFrameActiveTouches;
    private List<int> touchesWeThinkAreActive;
    private Dictionary<int, string> touchJob;
    private Dictionary<int, Vector2> originalTouchPos;
    private float maxPullBack = 8.0f;
    private float pushForce = 3f;
    private Vector2 Force;
    private bool PullDistanceLongEnough = false;
    bool CanStartAim = true;
    private float minPullDist = 0.8f;
    private void Start()
    {
        DebugText.text = "Init";
        maxForce = pushForce * pushForce;
        Ball = GameObject.Find("Ball");
        touchesWeThinkAreActive = new List<int>();
        touchJob = new Dictionary<int, string>();
        originalTouchPos = new Dictionary<int, Vector2>();
        Application.targetFrameRate = 60;
        PullBackJoystick.SetActive(false);
        BallRB = Ball.GetComponent<Rigidbody2D>();
        prevFrameActiveTouches = new List<int>();
    }
    string debugOutput;
    int pullingIndex = -1;
    private void FixedUpdate()
    {
        //projection.Show();
        //try
        //{
        //    projection.SimulatrTrajectory(Ball.transform.position, new Vector2(-10, 0), BallRB.velocity, 0.0f, Ball.transform.rotation);
        //}
        //catch (System.Exception e)
        //{
        //    Debug.Log(e.ToString());
        //}
        activeTouches = new List<int>();
        if ((pullingIndex != -1) && (UnityEngine.Input.touchCount == 0)) ResetTouch();
        for (int i = 0; i < UnityEngine.Input.touchCount; i++)
        {
            int fingerIndex = UnityEngine.Input.touches[i].fingerId;
            activeTouches.Add(fingerIndex);
            string fingerJob = "";
            touchJob.TryGetValue(fingerIndex, out fingerJob);
            if (!prevFrameActiveTouches.Contains(fingerIndex))
            {
                if (!touchesWeThinkAreActive.Contains(fingerIndex)) touchesWeThinkAreActive.Add(fingerIndex);
                if (CanStartAim && BallSpeed.AbleToBeHit && (pullingIndex == -1))   //aim
                {
                    CanStartAim = false;
                    pullingIndex = fingerIndex;
                    touchJob[fingerIndex] = "Ball";
                    Vector3 JoyStickPos = camera.ScreenToWorldPoint(UnityEngine.Input.touches[i].position);
                    JoyStickPos.z = transform.position.z - 0.5f;
                    PullBackJoystick.transform.position = JoyStickPos;
                    PullBackJoystick.SetActive(true);
                }
                else if (BallSpeed.AbleToBeHit)                                     //launch
                {
                    for (int j = 0; j < touchesWeThinkAreActive.Count; j++)
                    {
                        touchJob.TryGetValue(touchesWeThinkAreActive[j], out string job);
                        if (job == "Ball")
                        {
                            CanStartAim = true;
                            touchJob[touchesWeThinkAreActive[j]] = "Used";
                            LaunchBall();
                            pullingIndex = -1;
                            touchJob[fingerIndex] = "Used";
                        }
                    }
                }
            }
            else if ((pullingIndex != -1) && (fingerJob == "Ball"))   //switch aim touch
            {
                Vector2 tempPos = camera.ScreenToWorldPoint(UnityEngine.Input.touches[i].position);
                Vector2 origPos = PullBackJoystick.transform.position;

                float angle = AngleBetween(tempPos, origPos);
                if (angle < 0)
                    angle = 360 + angle;
                float radia = ConvertToRadians(angle);
                float hypot = (tempPos.y - origPos.y) / (float)System.Math.Sin(radia);
                if (hypot > maxPullBack)
                {
                    tempPos.x = origPos.x + (maxPullBack * (float)System.Math.Cos(radia));
                    tempPos.y = origPos.y + (maxPullBack * (float)System.Math.Sin(radia));
                }
                Vector2 trajSpeed = new Vector2((tempPos.x - origPos.x) / maxPullBack, (tempPos.y - origPos.y) / maxPullBack);
                float distance = Vector2.Distance(origPos, tempPos);
                Vector2 direction = (origPos - tempPos).normalized;
                Force = direction * distance * pushForce;
                Vector3 JoyCirPos = new Vector3(tempPos.x, tempPos.y, PullBackJoystick.transform.position.z);
                PullBackJoystick.transform.GetChild(0).transform.GetChild(0).transform.position = JoyCirPos;

                if (hypot > minPullDist)
                {
                    joyStickShape.UpdateColor(1);
                    PullDistanceLongEnough = true;
                    projection.Show();
                    Vector2 currentMvmtForce = Vector2.zero;
                    float currentRot = 0.0f;
                    if (Ball.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
                    {
                        currentMvmtForce = rb.velocity;
                        currentRot = rb.rotation;
                        currentMvmtForce = (currentMvmtForce * rb.mass) / 4f;
                    }
                    projection.SimulatrTrajectory(Ball.transform.position, Force, currentMvmtForce, currentRot, Ball.transform.rotation);
                }
                else
                {
                    joyStickShape.UpdateColor(hypot / minPullDist);
                    PullDistanceLongEnough = false;
                    projection.Hide();
                }
            }
        }
        prevFrameActiveTouches = activeTouches;
        for (int i = 0; i < touchesWeThinkAreActive.Count; i++)
        {
            //DebugText.text = ListToString(touchesWeThinkAreActive) + "\n" + ListToString(activeTouches);
            if (!activeTouches.Contains(touchesWeThinkAreActive[i]))
            {
                touchJob.TryGetValue(touchesWeThinkAreActive[i], out string job);
                if ((job == "Ball"))
                {
                    LaunchBall();
                    pullingIndex = -1;
                    CanStartAim = true;
                }
                touchesWeThinkAreActive.RemoveAt(i);
                touchJob[i] = "";
            }
        }
    }
    private void ResetTouch()   //function to reset the touches because im big dum dum
    {
        pullingIndex = -1;
        projection.Hide();
        CanStartAim = true;
    }

    private void LaunchBall()
    {
        projection.Hide();
        if (PullDistanceLongEnough)
        {
            BallSpeed.BallShot();
            //Ball.GetComponent<Rigidbody2D>().AddForce(Force, ForceMode2D.Impulse);
            Rigidbody2D rb = Ball.GetComponent<Rigidbody2D>();
            rb.velocity = rb.velocity + Force;
            scoreController.ScoreAdOne();
        }
        PullBackJoystick.SetActive(false);
    }
    private float AngleBetween(Vector3 a, Vector3 b)
    {
        float xDiff = a.x - b.x;
        float yDiff = a.y - b.y;
        return (float)System.Math.Atan2(yDiff, xDiff) * 180.0f / (float)System.Math.PI;
    }
    public static float ConvertToRadians(float degrees)
    {
        return (float)(degrees * (System.Math.PI / 180.0));
    }
    private float maxForce;
    public Vector2 GetForceToAdd(Vector2 currForce, Vector2 tryForce)
    {
        ///Velocity to force
        currForce.x = ((currForce.x * BallRB.mass) / 4);
        currForce.y = ((currForce.y * BallRB.mass) / 4);

        Vector2 wantedForce = currForce + tryForce;
        ///test if larger than max
        float hyp = wantedForce.magnitude;
        if (hyp < maxForce)
            return tryForce;

        ///calculate line equation
        float slope = (wantedForce.y - currForce.y) / (wantedForce.x - currForce.x);
        float intercept = currForce.y - (slope * currForce.x);

        ///figure out combined equation with only x as a variable
        float a = slope * slope;
        float b = 2 * (slope * intercept);
        float c = intercept * intercept;

        a += 1;
        c -= (maxForce * maxForce);

        ///get 2 x values
        float xA = (-b + Mathf.Sqrt((b * b) - (4 * a * c))) / (2 * a);
        float xB = (-b - Mathf.Sqrt((b * b) - (4 * a * c))) / (2 * a);

        ///get correct x value
        float x;
        if (((xA > currForce.x) && (xA < wantedForce.x)) || ((xA < currForce.x) && (xA > wantedForce.x)))
            x = xA;
        else
            x = xB;
        float y = (slope * x) + intercept;
        Vector2 addForce = new Vector2(x, y) - currForce;
        return addForce;

    }
    private string ListToString(List<int> list)
    {
        string r = "";
        foreach(int s in list)
        {
            r += s + " ";
        }
        return r;
    }

}
