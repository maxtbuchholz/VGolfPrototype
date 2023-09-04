using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class GravityWell : MonoBehaviour
{
    [SerializeField] Collider2D AffectedGravArea;
    [SerializeField] GameObject[] affectedObjects;
    BallGravityhandler[] affectedObjectsGravHandlers;
    Collider2D[] affectedObjectsColliders;
    [SerializeField] TextMeshProUGUI DebugText;
    [SerializeField] bool goal = false;
    [SerializeField] Collider2D optionalGoalCollider;
    private bool[] affectedIsTouching;
    private Collider2D CenterCollider;
    float radius = 5;
    float gravRangeRadius;
    [SerializeField] float gravityPull = 10;
    private void Start()
    {
        List<GameObject> tempobj = new List<GameObject>(GameObject.FindGameObjectsWithTag("Ball"));
        for (int i = tempobj.Count - 1; i >= 0; i--)
        {
            if (tempobj[i].scene != gameObject.scene)
                tempobj.RemoveAt(i);
        }
        affectedObjects = tempobj.ToArray();
        SetUpAffectedObjects();
        gravRangeRadius = CenterCollider.bounds.extents.magnitude;
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
        //Vector2 WellPos = transform.position;
        for (int i = 0; i < affectedObjects.Length; i++)
        {
            try
            {
                AddGravToObject(affectedObjects[i], affectedObjectsColliders[i], affectedObjectsGravHandlers[i], 1);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }
    float unescapeRadius = 1.4f;
    public void AddGravToObject(GameObject obj, Collider2D col, BallGravityhandler gra, float percentPfUllGrav)
    {
        if (true)
        {
            Vector2 ClosestDist = Physics2D.ClosestPoint(obj.transform.position, CenterCollider);
            Vector2 DistanceInside = Physics2D.ClosestPoint(obj.transform.position, AffectedGravArea);
            Vector2 vDist = new Vector2(ClosestDist.x - obj.transform.position.x, ClosestDist.y - obj.transform.position.y);
            float dist = vDist.magnitude;
            //float dist = Mathf.Sqrt(dx + dy);
            //DebugText.text = Mathf.Round(dist).ToString();
            if (AffectedGravArea.OverlapPoint(obj.transform.position))
            {
                gra.UpdateGravityInfluence(transform.GetInstanceID(), true);
                LastTimeWasGrav = true;
                // apply force based on object stats
                //Debug.DrawLine(transform.position, affectedObjects[i].transform.position, Color.yellow);
                Vector2 offset = vDist;// (transform.position - affectedObjects[i].transform.position);
                                       //offset = offset.normalized;
                float mag = (dist + 1) / (dist + DistanceInside.magnitude + 1);
                var rig = obj.GetComponent<Rigidbody2D>();
                Vector2 force = offset * mag * (gravityPull);
                force *= percentPfUllGrav;
                if(dist > 0)
                    force *= (1 / dist);
                if (goal && (optionalGoalCollider != null))
                {
                    UpdateAsGoal(force, rig, obj);
                }
                else
                {
                    rig.velocity = rig.velocity + force;
                    //Debug.DrawLine(obj.transform.position, Vector3.zero, UnityEngine.Color.blue);
                }
            }
            else
            {
                gra.UpdateGravityInfluence(transform.GetInstanceID(), false);
                LastTimeWasGrav = false;
                //Debug.DrawLine(obj.transform.position + new Vector3(0.1f,0.1f,0.1f), Vector3.zero, UnityEngine.Color.yellow);
            }
        }
    }
    float TimeInBlackHole = 0f;
    int objInGravID = -1;
    private void UpdateAsGoal(Vector2 force, Rigidbody2D rig, GameObject obj)
    {
        if (objInGravID != obj.GetInstanceID())
        {
            objInGravID = obj.GetInstanceID();
            TimeInBlackHole = 0f;
        }
        rig.velocity = rig.velocity + force;
        Bounds unBounds = optionalGoalCollider.bounds;
        unBounds.center = new Vector3(unBounds.center.x, unBounds.center.y, obj.transform.position.z);
        if (unBounds.Contains(obj.transform.position))
        {
            TimeInBlackHole += Time.deltaTime;
            Vector2 ColPos = CenterCollider.transform.position;
            Vector2 CurDist = obj.transform.position - CenterCollider.transform.position;
            Vector2 NextPos = CurDist + rig.velocity.normalized;
            float CurMag = CurDist.magnitude;
            float NexMag = NextPos.magnitude;

            float timeToCenter = 2f;
            float multiplyer;
            if (TimeInBlackHole > timeToCenter)
                multiplyer = 0f;
            else
                multiplyer = 0.40f * (1 - (TimeInBlackHole / timeToCenter));
            float rad = optionalGoalCollider.transform.lossyScale.x * multiplyer;

            if (NexMag != rad) //!= CurMag
            {
                Vector2 FinPos = NextPos.normalized * rad;
                Vector2 GoTo = (FinPos - CurDist);
                float magv = rig.velocity.magnitude;
                //float speed = 200f * (TimeInBlackHole / timeToCenter);
                //if (magv < 40f)
                //    magv = 40f;
                rig.velocity = magv * GoTo.normalized;
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
