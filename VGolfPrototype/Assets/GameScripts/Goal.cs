using System.Collections;
using System.Collections.Generic;
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
    public void CheckForGoal()
    {
        Rect r = GoalRect.rect;
        r.position = transform.position;
        if (r.Contains(Ball.position) && BallSpeed.AbleToBeHit)
        {
            if (!VictoryLoaded)
            {
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
