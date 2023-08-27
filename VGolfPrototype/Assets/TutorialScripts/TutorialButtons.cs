using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialButtons : MonoBehaviour
{
    [SerializeField] int tutorialPageNumber;
    public void Back()
    {
        if(tutorialPageNumber == 1)
            SceneManager.LoadScene("StartPage");
        else
            SceneManager.LoadScene((tutorialPageNumber - 1).ToString()+"TutoriallScreen", LoadSceneMode.Single);
    }
    public void Next()
    {
        if (tutorialPageNumber == 4)
            SceneManager.LoadScene("BallGame");
        else
            SceneManager.LoadScene((tutorialPageNumber + 1).ToString() + "TutoriallScreen", LoadSceneMode.Single);
    }
}
