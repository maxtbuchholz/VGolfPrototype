using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class GravityWell : MonoBehaviour
{
    [SerializeField] GameObject[] affectedObjects;
    BallGravityhandler[] affectedObjectsGravHandlers;
    Collider2D[] affectedObjectsColliders;
    [SerializeField] TextMeshProUGUI DebugText;
    private bool[] affectedIsTouching;
    private Collider2D CenterCollider;
    float radius = 5;
    float gravityPull = 10;
    private void Start()
    {
        List<GameObject> tempobj = new List<GameObject>(GameObject.FindGameObjectsWithTag("Ball"));
        for(int i = tempobj.Count - 1; i >= 0; i--)
        {
            if (tempobj[i].scene != gameObject.scene)
                tempobj.RemoveAt(i);
        }
        affectedObjects = tempobj.ToArray();
        SetUpAffectedObjects();
        if (gameObject.scene.name != "Simulation")
            Debug.Log("");
    }
    public void SetAffectEdObjectListForProjection(GameObject obj)
    {
        if (gameObject.scene.name != "Simulation")
            Debug.Log("");
        affectedObjects = new GameObject[] { obj };
        SetUpAffectedObjects();
    }
    private bool LastTimeWasGrav = false;
    void Update()
    {
        UpdateGravityWell();
    }
    public void UpdateGravityWell()
    {
        //Debug.DrawLine(transform.position, new Vector2(transform.position.x + radius, transform.position.y), Color.green);
        //get object distance
        Vector2 WellPos = transform.position;
        for (int i = 0; i < affectedObjects.Length; i++)
        {
            if (affectedObjectsColliders[i] != null)
            {
                try
                {
                    float closeMag = (CenterCollider.ClosestPoint(affectedObjectsColliders[i].transform.position) - WellPos).magnitude;
                }
                catch (System.Exception e)
                {
                    if (affectedObjects[i].name == "Ball")
                        Debug.Log(e.Message);
                }
                if (true)
                {
                    float dx = transform.position.x - affectedObjects[i].transform.position.x;
                    float dy = transform.position.y - affectedObjects[i].transform.position.y;
                    float dist = Mathf.Sqrt((dx * dx) + (dy * dy));
                    //DebugText.text = dist.ToString();
                    if (dist <= radius)
                    {
                        if (!LastTimeWasGrav)
                        {
                            affectedObjectsGravHandlers[i].UpdateGravityInfluence(transform.GetInstanceID(), true);
                            LastTimeWasGrav = true;
                        }
                        // apply force based on object stats
                        //Debug.DrawLine(transform.position, affectedObjects[i].transform.position, Color.yellow);
                        Vector2 offset = (transform.position - affectedObjects[i].transform.position);
                        var mag = offset.magnitude;
                        var rig = affectedObjects[i].GetComponent<Rigidbody2D>();
                        Vector2 force = offset / mag / mag * (gravityPull * Time.deltaTime);
                        //rig.velocity = rig.velocity + force;
                        if (!affectedIsTouching[i])
                        {
                            rig.AddForce(force, ForceMode2D.Impulse);
                        }
                        else
                        {
                            rig.velocity = Vector2.zero;
                        }
                    }
                    else
                    {
                        if (affectedObjects[i].name == "Ball")
                            Debug.Log("Ball");
                        affectedObjectsGravHandlers[i].UpdateGravityInfluence(transform.GetInstanceID(), false);
                        LastTimeWasGrav = false;
                    }
                }
            }
            else
            {
                if(gameObject.scene.name != "Simulation")
                    Debug.Log("");
            }
        }
    }
    private void SetUpAffectedObjects()
    {
        affectedIsTouching = new bool[affectedObjects.Length];
        CenterCollider = transform.GetComponent<Collider2D>();
        affectedObjectsGravHandlers = new BallGravityhandler[affectedObjects.Length];
        affectedObjectsColliders = new Collider2D[affectedObjects.Length];
        for (int i = 0; i < affectedObjects.Length; i++)
        {
            affectedIsTouching[i] = false;
            affectedObjectsGravHandlers[i] = affectedObjects[i].GetComponent<BallGravityhandler>();
            affectedObjectsColliders[i] = affectedObjects[i].GetComponent<Collider2D>();
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("");
        for(int i = 0; i < affectedObjects.Length; i++)
        {
            if (affectedObjects[i] = col.gameObject)
            {
                affectedIsTouching[i] = true;
            }
        }
    }
    void OnCollisionExit2D(Collision2D col)
    {
        Debug.Log("");
        for (int i = 0; i < affectedObjects.Length; i++)
        {
            if (affectedObjects[i] = col.gameObject)
            {
                affectedIsTouching[i] = false;
            }
        }
    }
}
