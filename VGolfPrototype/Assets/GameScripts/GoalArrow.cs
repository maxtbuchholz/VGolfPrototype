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
    [SerializeField] Transform ArrowParent;
    private SpriteRenderer[] ArrowSprites;

    private float MinDistanceFromGoal = 4f;
    bool origPosSet = false;
    private void Start()
    {
        ArrowSprites = ArrowParent.GetComponentsInChildren<SpriteRenderer>();
        for(int i = 0; i < ArrowSprites.Length; i++)
        {
            ArrowSprites[i].gameObject.transform.localPosition = new Vector3(ArrowSprites[i].gameObject.transform.localPosition.x, (float)i * -ArrowOffset, ArrowSprites[i].gameObject.transform.localPosition.z);
        }
    }
    void Update()
    {
        UpdateGoalArrow();
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
        MinScreenPos.x += 1.5f;
        MinScreenPos.y += 1.5f;
        MaxScreenPos.x -= 1.5f;
        MaxScreenPos.y -= 1.5f;
        //Debug.DrawLine(MaxScreenPos, MinScreenPos, Color.yellow);
        if(rect.Contains(angleBetween0))
        {
            if (origPosSet)
                transform.position = (angleBetween0 + (transform.position * 3)) / 4;
            else
                transform.position = angleBetween0;
            //float angle = Mathf.Rad2Deg * Mathf.Atan((Goal.position.y - transform.position.y) / (Goal.position.x - transform.position.x));
            //float angle = Vector2.SignedAngle(transform.position, Goal.position);
            float angle = Mathf.Rad2Deg * (Mathf.Atan2(Goal.position.y - transform.position.y, Goal.position.x - transform.position.x));
            angle = -(90 - angle);
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
            if (origPosSet)
                transform.position = (angleBetween0 + (transform.position * 3)) / 4;
            else
                transform.position = angleBetween0;
        }
        //Debug.DrawLine(Goal.position, transform.position, Color.red);
        //float angle2 = Mathf.Rad2Deg * Mathf.Atan((Goal.position.y - transform.position.y) / (Goal.position.x - transform.position.x));
        //float angle2 = Vector2.SignedAngle(transform.position, Goal.position);
        float angle2 = Mathf.Rad2Deg * (Mathf.Atan2(Goal.position.y - transform.position.y, Goal.position.x - transform.position.x));
        angle2 = -(90 - angle2);
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
    float sTime = 0;
    float MaxTime = 2f;
    float MaxAlpha = 0.2f;
    float ArrowOffset = 0.1f;
    float ArrowMoveMaxOffset = 0.1f;
    float TimeArrowOffsetPerOneSec = 0.1f;

    float minAppearDist = 5.0f;
    float maxAppearDist = 8.0f;
    private void UpdateGoalArrow()
    {
        sTime += Time.deltaTime;
        sTime %= MaxTime;
        //sTime /= MaxTime;
        for(int i = 0; i < ArrowSprites.Length; i++)
        {
            float bGDist = (Ball.transform.position - Goal.transform.position).magnitude;
            if(bGDist > maxAppearDist)
            {
                Color originalArrowColor = ArrowSprites[i].GetComponent<SpriteRenderer>().color;
                originalArrowColor.a = 0.5f;
                ArrowSprites[i].color = originalArrowColor;
            }
            else if(bGDist > minAppearDist)
            {
                Color originalArrowColor = ArrowSprites[i].GetComponent<SpriteRenderer>().color;
                float per = (bGDist - minAppearDist) / (maxAppearDist - minAppearDist);
                originalArrowColor.a = 0.5f * per;
                ArrowSprites[i].color = originalArrowColor;
            }
            else
            {
                Color originalArrowColor = ArrowSprites[i].GetComponent<SpriteRenderer>().color;
                originalArrowColor.a = 0f;
                ArrowSprites[i].color = originalArrowColor;
            }
            float val = ArrowMoveMaxOffset * Mathf.Sin((1f / MaxTime) * sTime * 6.28318530718f);
            ArrowSprites[i].gameObject.transform.localPosition = new Vector3(0, val , ArrowSprites[i].gameObject.transform.localPosition.z);
            ////float val = Mathf.Sin((1f / MaxTime) * (sTime + ((float)i * TimeArrowOffsetPerOneSec * MaxTime)) * 6.28318530718f);
            //Color originalArrowColor = ArrowSprites[i].GetComponent<SpriteRenderer>().color;
            //originalArrowColor.a = (val * (MaxAlpha)) + (0.5f * MaxAlpha);
            ////DebugText.text = (originalArrowColor.a).ToString();
            //ArrowSprites[i].color = originalArrowColor;
        }
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
