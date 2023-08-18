using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    [SerializeField] BallSpeedReporter BallSpeed;
    [SerializeField] Transform Ball;
    [SerializeField] TextMeshProUGUI DebugText;
    [SerializeField] GameObject GameCamera;
    [SerializeField] TouchHandler TouchH;
    [SerializeField] Collider2D goalCollider;
    private bool VictoryLoaded = false;
    public void Start()
    {
        if (gameObject.scene.name == "Simulation")
            Destroy(this);
    }
    private float prevY;
    public void Update()
    {
        //if (goalCollider.OverlapPoint(Ball.position))
        //{
        //    overlapping = true;
        //}
        if (true)
        {
            //Rect r = GoalRect.rect;
            //r.position = GoalRect.rect.position -  GoalRect.anchoredPosition;
            //Vector2 BallPos = Ball.transform.position;

            if (goalCollider.OverlapPoint(Ball.position))
            {
                if (!VictoryLoaded)
                {
                    string name = transform.GetInstanceID().ToString();
                    VictoryLoaded = true;
                    name += name;
                    Debug.Log(name + Time.frameCount);
                    if (Ball.gameObject.TryGetComponent<BallSpeedReporter>(out BallSpeedReporter BSR))
                    {
                        BSR.SetInGoal();
                    }
                    DebugText.text += "Goal!";
                    TouchH.enabled = false;
                    int y = SceneManager.GetActiveScene().buildIndex;
                    prevY = GameCamera.transform.position.y;
                    DataGameToVictory.instance.SetGameCameraYOffset(prevY);
                    DataGameToVictory.instance.SetGameSceneName(SceneManager.GetActiveScene().name);
                    if (GameCamera.TryGetComponent<Camera>(out Camera cam))
                    {
                        DataGameToVictory.instance.SetGameCameraOrthSize(cam.orthographicSize);
                    }
                    GameCamera.SetActive(false);
                    SceneManager.LoadScene("Victory", LoadSceneMode.Additive);
                    //GameCamera.SetActive(false);
                    //SceneManager.UnloadSceneAsync(y);
                }
                else
                {
                    if(prevY != GameCamera.transform.position.y)
                    {
                        prevY = GameCamera.transform.position.y;
                        DataGameToVictory.instance.SetGameCameraYOffset(prevY);
                    }
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
