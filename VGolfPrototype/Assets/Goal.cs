using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] RectTransform GoalRect;
    [SerializeField] BallSpeedReporter BallSpeed;
    [SerializeField] Transform Ball;
    [SerializeField] TextMeshProUGUI DebugText;

    public void CheckForGoal()
    {
        Rect r = GoalRect.rect;
        r.position = transform.position;
        if (r.Contains(Ball.position) && BallSpeed.AbleToBeHit)
        {
            DebugText.text += "Goal!";
        }
    }
}
