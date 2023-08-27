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
    [SerializeField] ScoreController scoreController;
    [SerializeField] ParticleSystem ballExplosionParticle;
    private bool VictoryLoaded = false;
    public void Start()
    {
        if (gameObject.scene.name == "Simulation")
            Destroy(this);
    }
    private float prevY;
    private float BallInTime = -1;
    private bool didballPoof = false;
    private bool switchedCameraToGoal = false;
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
                    BallInTime = 0;
                    VictoryLoaded = true;
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
            if(BallInTime >= 0)
            {
                BallInTime += Time.deltaTime;
            }
            if ((BallInTime > 1.0f) && !switchedCameraToGoal)  //explosion
            {
                switchedCameraToGoal = true;
                GameCamera.GetComponent<FollowBall>().ball = transform;
            }
            if ((BallInTime > 1.0f) && !didballPoof)  //explosion
            {
                didballPoof = true; SpriteRenderer[] srs = Ball.GetComponentsInChildren<SpriteRenderer>();
                for (int i = 0; i < srs.Length; i++)
                {
                    srs[i].enabled = false;
                }
                ParticleSystem par = GameObject.Instantiate(ballExplosionParticle);
                par.transform.position = Ball.transform.position;
                par.Play();
            }
            if (BallInTime > 2.0f)  //send to victory page
            {
                BallInTime = -1;
                string name = transform.GetInstanceID().ToString();
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
                DataGameToVictory.instance.SetScore(scoreController.GetScore());
                DataGameToVictory.instance.SetPar(scoreController.GetPar());
                if (GameCamera.TryGetComponent<Camera>(out Camera cam))
                {
                    DataGameToVictory.instance.SetGameCameraOrthSize(cam.orthographicSize);
                }
                DataGameToVictory.instance.SetGameCamera(GameCamera);
                SceneManager.LoadScene("Victory", LoadSceneMode.Additive);
                //GameCamera.SetActive(false);
                //GameCamera.SetActive(false);
                //SceneManager.UnloadSceneAsync(y);
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
