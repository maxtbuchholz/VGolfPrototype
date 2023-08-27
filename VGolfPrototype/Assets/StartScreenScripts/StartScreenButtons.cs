using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreenButtons : MonoBehaviour
{
    public void ToLevel()
    {
        SceneManager.LoadScene("BallGame");
    }
    public void ToTutorial()
    {
        SceneManager.LoadScene("1TutoriallScreen");
    }
}
