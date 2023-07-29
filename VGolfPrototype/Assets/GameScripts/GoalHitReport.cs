using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalHitReport : MonoBehaviour
{
    [SerializeField] Goal goal;
    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject ob = collision.gameObject;
        if(ob.TryGetComponent<BallSpeedReporter>(out BallSpeedReporter BSR))
        { 
            if(BSR.isReal && (goal != null))
                goal.CheckForGoal();
        }
    }

}
