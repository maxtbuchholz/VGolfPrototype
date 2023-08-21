using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataGameToVictory : MonoBehaviour
{
    private int Score = -1;
    private float GameCameraYOffset = -1;
    private float GameCameraOrthSize = -1;
    public static DataGameToVictory instance;
    private string gameSceneName = null;
    private GameObject gameCamera;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    public void SetScore(int Score)
    {
        this.Score = Score;
    }
    public int GetScore()
    {
        return Score;
    }
    public void SetGameCamera(GameObject gameCamera)
    {
        this.gameCamera = gameCamera;
    }
    public GameObject GetGameCamera()
    {
        return gameCamera;
    }
    public void SetGameSceneName(string gameSceneName)
    {
        this.gameSceneName = gameSceneName;
    }
    public string GetgGmeSceneName()
    {
        return gameSceneName;
    }
    public void SetGameCameraYOffset(float GameCameraYOffset)
    {
        this.GameCameraYOffset = GameCameraYOffset;
    }
    public float GetGameCameraYOffset()
    {
        return GameCameraYOffset;
    }
    public void SetGameCameraOrthSize(float GameCameraOrthSize)
    {
        this.GameCameraOrthSize = GameCameraOrthSize;
    }
    public float GetGameCameraOrthSize()
    {
        return GameCameraOrthSize;
    }
}