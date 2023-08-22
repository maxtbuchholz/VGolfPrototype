using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ScoreText;
    private int score = 0;
    [SerializeField] int par = 10;
    void Start()
    {
        ScoreText.text = score.ToString() + " / " + par.ToString();
    }
    public int GetScore()
    {
        return score;   
    }
    public int GetPar()
    {
        return par;
    }
    public void ScoreAdOne()
    {
        score++;
        ScoreText.text = score.ToString() + " / " + par.ToString();
    }
}
