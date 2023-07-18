using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TouchHandler : MonoBehaviour
{
    [SerializeField] GameObject Ball;
    [SerializeField] Camera camera;

    private List<int> activeTouches;
    private List<int> touchesWeThinkAreActive;
    private Dictionary<int, string> touchJob;
    private void Start()
    {
        touchesWeThinkAreActive = new List<int>();
        touchJob = new Dictionary<int, string>();
        Application.targetFrameRate = 60;
    }
    void Update()
    {

        activeTouches = new List<int>();
        for (int i = 0; i < Input.touchCount; i++)
        {
            activeTouches.Add(Input.touches[i].fingerId);
            if(!touchesWeThinkAreActive.Contains(Input.touches[i].fingerId)) touchesWeThinkAreActive.Add(Input.touches[i].fingerId);

            if (Input.touches[i].phase == TouchPhase.Began)
            {
                Vector3 touchPosition = camera.ScreenToWorldPoint(Input.touches[i].position);
                Vector2 touchPosWorld2D = new Vector2(touchPosition.x, touchPosition.y);
                RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, camera.transform.forward);
                if (hitInformation.collider != null)
                {
                    GameObject touchedObject = hitInformation.transform.gameObject;
                    if(touchedObject == Ball) //touching ball
                    {
                        touchJob[Input.touches[i].fingerId] = "Ball";
                    }
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
                    Destroy(Ball);
                }
                touchesWeThinkAreActive.RemoveAt(i);
            }
        }
    }
}
