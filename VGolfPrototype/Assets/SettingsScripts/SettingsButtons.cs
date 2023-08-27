using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsButtons : MonoBehaviour
{
    public void BackToGam()
    {
        int countLoaded = SceneManager.sceneCount;
        Scene[] loadedScenes = new Scene[countLoaded];

        for (int i = 0; i < countLoaded; i++)
        {
            if (SceneManager.GetSceneAt(i).name == "Settings")
                SceneManager.UnloadSceneAsync(i);
        }
        DataGameToVictory.instance.GetGameCamera().GetComponent<Camera>().enabled = true;
        DataGameToVictory.instance.GetTouchHandler().enabled = true;
        List<Collider2D> buttons = DataGameToVictory.instance.GetButtonList();
        for(int i = 0; i < buttons.Count; i++)
        {
            buttons[i].gameObject.GetComponent<Button>().interactable = true;
        }
        Time.timeScale = 1;
    }
    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(DataGameToVictory.instance.GetgGmeSceneName()); // loads current scene
    }
}
