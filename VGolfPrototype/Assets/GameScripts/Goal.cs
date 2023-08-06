using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    [SerializeField] RectTransform GoalRect;
    [SerializeField] BallSpeedReporter BallSpeed;
    [SerializeField] Transform Ball;
    [SerializeField] TextMeshProUGUI DebugText;
    [SerializeField] GameObject GameCamera;
    [SerializeField] TouchHandler TouchH;
    private bool VictoryLoaded = false;
    private bool ShouldCheck = false;
    public void CheckForGoal()
    {
        ShouldCheck = true;
    }
    public void Update()
    {
        if (ShouldCheck && !VictoryLoaded)
        {
            //Rect r = GoalRect.rect;
            //r.position = GoalRect.rect.position -  GoalRect.anchoredPosition;
            Vector2 BallPos = Ball.transform.position;

            if (ContainsPoint(BallPos, GoalRect) && BallSpeed.AbleToBeHit)
            {
                if (!VictoryLoaded)
                {
                    if(Ball.gameObject.TryGetComponent<BallSpeedReporter>(out BallSpeedReporter BSR))
                    {
                        BSR.SetInGoal();
                    }
                    VictoryLoaded = true;
                    DebugText.text += "Goal!";
                    TouchH.enabled = false;
                    int y = SceneManager.GetActiveScene().buildIndex;
                    DataGameToVictory.instance.SetGameCameraYOffset(transform.position.y);
                    if (GameCamera.TryGetComponent<Camera>(out Camera cam))
                    {
                        DataGameToVictory.instance.SetGameCameraOrthSize(cam.orthographicSize);
                    }
                    SceneManager.LoadScene("Victory", LoadSceneMode.Additive);
                    //GameCamera.SetActive(false);
                    //SceneManager.UnloadSceneAsync(y);
                }
            }
        }
    }
    private bool ContainsPoint(Vector2 point, RectTransform rt)
    {
        Rect rect = rt.rect;
        float leftSide = rt.position.x - rect.width / 2;
        float rightSide = rt.position.x + rect.width / 2;
        float topSide = rt.position.y + rect.height / 2;
        float bottomSide = rt.position.y - rect.height / 2;

        if (point.x >= leftSide &&
            point.x <= rightSide &&
            point.y >= bottomSide &&
            point.y <= topSide)
            return true;
        return false;
    }
}
