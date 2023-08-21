using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTransform : MonoBehaviour
{
    float time = 0f;
    [SerializeField] float timeForFull = 4.0f;
    void Update()
    {
        time += Time.deltaTime;
        time %= timeForFull;
        float ang = time / timeForFull;
        ang *= 360;
        transform.eulerAngles = new Vector3(0,0,ang);
    }
}
