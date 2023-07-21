using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class TouchHandler : MonoBehaviour
{
    private GameObject Ball;
    [SerializeField] Camera camera;
    //[SerializeField] Trajectory trajectory;
    [SerializeField] ScoreController scoreController;
    [SerializeField] Projection projection;
    [SerializeField] GameObject PullBackJoystick;
    [SerializeField] private TextMeshProUGUI DebugText;
    private List<int> activeTouches;
    private List<int> touchesWeThinkAreActive;
    private Dictionary<int, string> touchJob;
    private Dictionary<int, Vector2> originalTouchPos;
    private float maxPullBack = 4.0f;
    private float pushForce = 4f;
    private Vector2 Force;
    private bool PullDistanceLongEnough = false;
    private void Start()
    {
        Ball = GameObject.Find("Ball");
        touchesWeThinkAreActive = new List<int>();
        touchJob = new Dictionary<int, string>();
        originalTouchPos = new Dictionary<int, Vector2>();
        Application.targetFrameRate = 60;
        PullBackJoystick.SetActive(false);
    }
    void Update()
    {
        //projection.SimulatrTrajectory(Ball.transform.position, new(2.0f, 2.0f));
        activeTouches = new List<int>();
        for (int i = 0; i < Input.touchCount; i++)
        {
            activeTouches.Add(Input.touches[i].fingerId);
            if(!touchesWeThinkAreActive.Contains(Input.touches[i].fingerId)) touchesWeThinkAreActive.Add(Input.touches[i].fingerId);
            Vector3 touchPosition = camera.ScreenToWorldPoint(Input.touches[i].position);
            if (Input.touches[i].phase == TouchPhase.Began)
            {
                //Vector2 touchPosWorld2D = new Vector2(touchPosition.x, touchPosition.y);
                //RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, camera.transform.forward);
                //if ((hitInformation.collider != null)
                //{
                //    GameObject touchedObject = hitInformation.transform.gameObject;
                //    if((touchedObject == Ball) || true) //touching ball
                //    {
                //        originalTouchPos[Input.touches[i].fingerId] = Input.touches[i].position;
                //        touchJob[Input.touches[i].fingerId] = "Ball";
                //        //trajectory.Show();
                //    }
                //}
                originalTouchPos[Input.touches[i].fingerId] = camera.ScreenToWorldPoint(Input.touches[i].position);
                touchJob[Input.touches[i].fingerId] = "Ball";
                PullBackJoystick.transform.position = originalTouchPos[Input.touches[i].fingerId];
                PullBackJoystick.SetActive(true);
                DebugText.text =  PullBackJoystick.transform.position.ToString();
            }
            else
            {
                string job = "";
                touchJob.TryGetValue(touchesWeThinkAreActive[i], out job);
                if (job == "Ball")
                {
                    Vector2 tempPos = camera.ScreenToWorldPoint(Input.touches[i].position);
                    Vector2 origPos = originalTouchPos[Input.touches[i].fingerId]; // Ball.transform.position;// camera.ScreenToWorldPoint(originalTouchPos[Input.touches[i].fingerId]);
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
                    if (hypot > 0.8)
                    {
                        PullDistanceLongEnough = true;
                        projection.Show();
                        //trajectory.Show();
                        projection.SimulatrTrajectory(Ball.transform.position, Force);
                    }
                    else
                    {
                        PullDistanceLongEnough = false;
                        projection.Hide();
                        //trajectory.Hide();
                    }
                    //Debug.Log(hypot);
                    //tempPos.z = -6;
                    Vector2 trajSpeed = new Vector2((tempPos.x - origPos.x) / maxPullBack, (tempPos.y - origPos.y) / maxPullBack);
                    //Debug.Log(speed.x.ToString() + "  " + speed.y.ToString());
                    Vector2 LaunchGrapEndingPoint = tempPos;
                    Vector2 LaunchGrapStartingPoint = origPos;
                    float distance = Vector2.Distance(LaunchGrapStartingPoint, LaunchGrapEndingPoint);
                    Vector2 direction = (LaunchGrapStartingPoint - LaunchGrapEndingPoint).normalized;
                    Force = direction * distance * pushForce;
                    //trajectory.UpdateDots(Ball.transform.position, Force);
                    Debug.DrawLine(Ball.transform.position, LaunchGrapEndingPoint, Color.blue);
                    Debug.DrawLine(LaunchGrapStartingPoint, LaunchGrapEndingPoint, Color.red);
                    PullBackJoystick.transform.GetChild(0).transform.GetChild(0).transform.position = tempPos;
                    ////tempOb.transform.GetChild(0).transform.GetChild(0).transform.position = tempPos;
                }
            }
        }
        for(int i = 0; i < touchesWeThinkAreActive.Count; i++)
        {
            if (!activeTouches.Contains(touchesWeThinkAreActive[i]))
            {
                string job = "";
                touchJob.TryGetValue(touchesWeThinkAreActive[i], out job);
                if (job == "Ball")
                {
                    projection.Hide();
                    //trajectory.Hide();
                    if (PullDistanceLongEnough)
                    {
                        Ball.GetComponent<Rigidbody2D>().AddForce(Force, ForceMode2D.Impulse);
                        scoreController.ScoreAdOne();
                    }
                    PullBackJoystick.SetActive(false);
                }
                touchesWeThinkAreActive.RemoveAt(i);
            }
        }
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
}
