using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetScoreVictoryScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private TextMeshProUGUI ParText;
    void Start()
    {
        ScoreText.text =  "Score: " + DataGameToVictory.instance.GetScore().ToString();
        ParText.text = "Par: " + DataGameToVictory.instance.Getpar().ToString();
    }
}
