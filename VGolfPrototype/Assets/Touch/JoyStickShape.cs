using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JoyStickShape : MonoBehaviour
{
    [SerializeField] Transform TouchStick;
    [SerializeField] Transform SlideBar;
    [SerializeField] Transform TouchStickMask;
    [SerializeField] TextMeshProUGUI DebugText;
    [SerializeField] Transform BottomCircleMask;
    [SerializeField] Transform physBottomCircle;

    private float TouchmaskMoveDownAmount;
    private void Start()
    {
        TouchmaskMoveDownAmount = 2.487f;// TouchStickMask.localScale.y + TouchStick.localScale.y;
    }
    void Update()
    {
        float angle = Mathf.Rad2Deg * (Mathf.Atan2(TouchStick.position.y - transform.position.y, TouchStick.position.x - transform.position.x));
        TouchStick.transform.eulerAngles = new Vector3(0, 0, angle + 90);
        SlideBar.transform.eulerAngles = new Vector3(0, 0, angle - 270);
        Vector2 MaskPos = transform.position - TouchStick.position;
        float mag = MaskPos.magnitude;
        mag += 14f;// TouchmaskMoveDownAmount;
        MaskPos = MaskPos.normalized * mag;
        //Debug.DrawLine(transform.position, TouchStick.position, Color.yellow);
        TouchStickMask.position = (new Vector3(transform.position.x - MaskPos.x, transform.position.y - MaskPos.y, transform.position.z));// new Vector3(TouchStick.position.x, TouchStick.position.y, TouchStick.position.z);
        DebugText.text = MaskPos.ToString();
        TouchStickMask.rotation = TouchStick.rotation;
        BottomCircleMask.position = physBottomCircle.position;
    }
}
