using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalHitReport : MonoBehaviour
{
    [SerializeField] Goal goal;
    private void OnCollisionStay2D(Collision2D collision)
    {
        goal.CheckForGoal();
    }

}
