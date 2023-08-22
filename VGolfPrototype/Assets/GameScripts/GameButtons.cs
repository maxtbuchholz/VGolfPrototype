using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameButtons : MonoBehaviour
{
    [SerializeField] List<Collider2D> buttons;
    [SerializeField] GameObject cam;
    [SerializeField] Camera camcam;
    [SerializeField] TouchHandler touchHandler;
    public bool notTouching(int index)
    {
        Touch testTouch = UnityEngine.Input.touches[index];
        Vector2 point = camcam.ScreenToWorldPoint(testTouch.position);
        for(int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i].OverlapPoint(point))
            {
                return false;
            }
        }
        return true;
    }
    public void ToSettingsPage()
    {
        if (SceneManager.sceneCount == 2)
        {
            Time.timeScale = 0;
            touchHandler.enabled = false;
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].gameObject.GetComponent<Button>().interactable = false;
            }
            prevY = this.cam.transform.position.y;
            DataGameToVictory.instance.SetButtonList(buttons);
            DataGameToVictory.instance.SetTouchHandler(touchHandler);
            DataGameToVictory.instance.SetGameCameraYOffset(prevY);
            DataGameToVictory.instance.SetGameSceneName(SceneManager.GetActiveScene().name);
            if (this.cam.TryGetComponent<Camera>(out Camera cam))
            {
                DataGameToVictory.instance.SetGameCameraOrthSize(cam.orthographicSize);
            }

            DataGameToVictory.instance.SetGameCamera(this.cam);
            SceneManager.LoadScene("Settings", LoadSceneMode.Additive); // loads current scene
        }
    }
    private float prevY;
    public void Update()
    {
        if (prevY != camcam.transform.position.y)
        {
            prevY = camcam.transform.position.y;
            DataGameToVictory.instance.SetGameCameraYOffset(prevY);
        }
    }
}
