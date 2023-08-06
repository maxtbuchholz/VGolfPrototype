using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GoalArrow : MonoBehaviour
{
    [SerializeField] Transform Goal;
    [SerializeField] RectTransform Screen;
    [SerializeField] Transform Ball;
    [SerializeField] TextMeshProUGUI DebugText;
    [SerializeField] Camera camera;

    private float MinDistanceFromGoal = 3f;
    void Update()
    {
        Vector3 vertPos = Ball.position;
        Vector3 angleBetween0 = vertPos - Goal.position;
        angleBetween0.z = 0;
        Vector3 originalAngle = angleBetween0.normalized;
        angleBetween0 = originalAngle * MinDistanceFromGoal;

        angleBetween0.x += Goal.position.x;
        angleBetween0.y += Goal.position.y;
        angleBetween0.z = transform.position.z;

        Rect rect = GetWorldRect(Screen);
        Vector2 MinScreenPos = rect.min;
        Vector2 MaxScreenPos = rect.max;
        Debug.DrawLine(MaxScreenPos, MinScreenPos, Color.yellow);
        if(rect.Contains(angleBetween0))
        {
            transform.position = (angleBetween0 + (transform.position * 3)) / 4;
            //float angle = Mathf.Rad2Deg * Mathf.Atan((Goal.position.y - transform.position.y) / (Goal.position.x - transform.position.x));
            //float angle = Vector2.SignedAngle(transform.position, Goal.position);
            float angle = Mathf.Rad2Deg * (Mathf.Atan2(Goal.position.y - transform.position.y, Goal.position.x - transform.position.x));
            angle = -(90 - angle);
            DebugText.text = angle.ToString();
            //if (vertPos.y <= Goal.position.y)
            //{
            //    if (angle < 0)
            //        angle += 90;
            //    else
            //        angle -= 90;
            //}
            //else
            //{
            //    if (angle < 0)
            //        angle -= 90;
            //    else
            //        angle += 90;
            //}
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
        else
        {
            if(angleBetween0.y > MaxScreenPos.y)
            {
                angleBetween0 = new Vector3(Goal.position.x, MaxScreenPos.y, 0);
            }
            else
            {
                angleBetween0 = new Vector3(Goal.position.x, MinScreenPos.y, 0);
            }
            angleBetween0.z = transform.position.z;
            transform.position = (angleBetween0 + (transform.position * 3)) / 4;
        }
        Debug.DrawLine(Goal.position, transform.position, Color.red);
        //float angle2 = Mathf.Rad2Deg * Mathf.Atan((Goal.position.y - transform.position.y) / (Goal.position.x - transform.position.x));
        //float angle2 = Vector2.SignedAngle(transform.position, Goal.position);
        float angle2 = Mathf.Rad2Deg * (Mathf.Atan2(Goal.position.y - transform.position.y, Goal.position.x - transform.position.x));
        angle2 = -(90 - angle2);
        DebugText.text = angle2.ToString();
        //if (vertPos.y <= Goal.position.y)
        //{
        //    if (angle2 < 0)
        //        angle2 += 90;
        //    else
        //        angle2 -= 90;
        //}
        //else
        //{
        //    if (angle2 < 0)
        //        angle2 -= 90;
        //    else
        //        angle2 += 90;
        //}
        transform.eulerAngles = new Vector3(0, 0, angle2);
    }
    private static Rect GetWorldRect(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        // Get the bottom left corner.
        Vector3 position = corners[0];

        Vector2 size = new Vector2(
            rectTransform.lossyScale.x * rectTransform.rect.size.x,
            rectTransform.lossyScale.y * rectTransform.rect.size.y);

        return new Rect(position, size);
    }
}
