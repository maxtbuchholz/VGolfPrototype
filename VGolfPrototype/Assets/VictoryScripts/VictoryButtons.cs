using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryButtons : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene(DataGameToVictory.instance.GetgGmeSceneName()); // loads current scene
    }
    public void GiveFeedback()
    {
        Application.OpenURL("http://unity3d.com/");
    }
}
