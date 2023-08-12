using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickShape : MonoBehaviour
{
    [SerializeField] Transform TouchStick;

    void Update()
    {
        float angle = Mathf.Rad2Deg * (Mathf.Atan2(TouchStick.position.y - transform.position.y, TouchStick.position.x - transform.position.x));
        //angle -= 45;
        //if (angle > 180)
        //    angle -= 180;
        angle %= 180;
        TouchStick.transform.eulerAngles = new Vector3(0, 0, angle - 90);
    }
}
