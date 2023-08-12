using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickShape : MonoBehaviour
{
    [SerializeField] Transform TouchStick;
    [SerializeField] Transform SlideBar;
    [SerializeField] Transform TouchStickMask;

    void Update()
    {
        float angle = Mathf.Rad2Deg * (Mathf.Atan2(TouchStick.position.y - transform.position.y, TouchStick.position.x - transform.position.x));
        TouchStick.transform.eulerAngles = new Vector3(0, 0, angle - 90);
        SlideBar.transform.eulerAngles = new Vector3(0, 0, angle - 270);
        TouchStickMask.position = TouchStick.position;
        TouchStickMask.rotation = TouchStick.rotation;
    }
}
