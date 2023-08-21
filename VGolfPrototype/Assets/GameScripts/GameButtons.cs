using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameButtons : MonoBehaviour
{
    [SerializeField] List<Collider2D> buttons;
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
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
