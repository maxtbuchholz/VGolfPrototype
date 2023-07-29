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
                int y = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene("Victory", LoadSceneMode.Single);
                SceneManager.UnloadSceneAsync(y);
            }
        }
    }
}
