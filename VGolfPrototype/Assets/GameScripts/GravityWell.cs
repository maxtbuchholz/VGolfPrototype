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
    float gravityPull = 5;
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
    }
    public void SetAffectEdObjectListForProjection(GameObject obj)
    {
        affectedObjects = new GameObject[] { obj };
        SetUpAffectedObjects();
    }
    private bool LastTimeWasGrav = false;
    void FixedUpdate()
    {
        UpdateGravityWell(true);
    }
    private void UpdateGravityWell(bool NaturalUpdate)
    {
        //Debug.DrawLine(transform.position, new Vector2(transform.position.x + radius, transform.position.y), Color.green);
        //get object distance
        Vector2 WellPos = transform.position;
        for (int i = 0; i < affectedObjects.Length; i++)
        {
            AddGravToObject(affectedObjects[i], affectedObjectsColliders[i], affectedObjectsGravHandlers[i], 1);
        }
    }
    public void AddGravToObject(GameObject obj, Collider2D col, BallGravityhandler gra, float percentPfUllGrav)
    {
            if (true)
            {
                Vector2 ClosestDist = Physics2D.ClosestPoint(obj.transform.position, CenterCollider);
                Vector2 vDist = new Vector2(ClosestDist.x - obj.transform.position.x, ClosestDist.y - obj.transform.position.y);
                float dist = vDist.magnitude;
                //float dist = Mathf.Sqrt(dx + dy);
                //DebugText.text = Mathf.Round(dist).ToString();
                if (dist <= radius)
                {
                    gra.UpdateGravityInfluence(transform.GetInstanceID(), true);
                    LastTimeWasGrav = true;
                    // apply force based on object stats
                    //Debug.DrawLine(transform.position, affectedObjects[i].transform.position, Color.yellow);
                    Vector2 offset = vDist;// (transform.position - affectedObjects[i].transform.position);
                                           //offset = offset.normalized;
                    float mag = 5.0f;// * percentPfUllGrav;// offset.magnitude;
                    var rig = obj.GetComponent<Rigidbody2D>();
                    Vector2 force = offset / mag / mag * (gravityPull);
                    force *= percentPfUllGrav;
                    rig.velocity = rig.velocity + force;
                }
                else
                {
                    gra.UpdateGravityInfluence(transform.GetInstanceID(), false);
                    LastTimeWasGrav = false;
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
    //void OnCollisionEnter2D(Collision2D col)
    //{
    //    //Debug.Log("");
    //    for(int i = 0; i < affectedObjects.Length; i++)
    //    {
    //        if (affectedObjects[i] = col.gameObject)
    //        {
    //            affectedIsTouching[i] = true;
    //        }
    //    }
    //}
    //void OnCollisionExit2D(Collision2D col)
    //{
    //    //Debug.Log("");
    //    for (int i = 0; i < affectedObjects.Length; i++)
    //    {
    //        if (affectedObjects[i] = col.gameObject)
    //        {
    //            affectedIsTouching[i] = false;
    //        }
    //    }
    //}
}
