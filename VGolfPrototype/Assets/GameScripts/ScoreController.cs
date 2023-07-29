using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ScoreText;
    private int score = 0;
    void Start()
    {
        ScoreText.text = score.ToString();
    }
    public void ScoreAdOne()
    {
        score++;
        ScoreText.text = score.ToString();
    }
}
