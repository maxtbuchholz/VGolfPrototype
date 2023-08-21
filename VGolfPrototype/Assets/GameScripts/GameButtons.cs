using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameButtons : MonoBehaviour
{
    [SerializeField] List<Collider2D> buttons;
    [SerializeField] Camera cam;
    public bool notTouching(int index)
    {
        Touch testTouch = UnityEngine.Input.touches[index];
        Vector2 point = cam.ScreenToWorldPoint(testTouch.position);
        for(int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i].OverlapPoint(point))
            {
                Debug.Log("");
            }
        }
        return true;
    }
}
