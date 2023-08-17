using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitVictoryCamera : MonoBehaviour
{
    void Start()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, DataGameToVictory.instance.GetGameCameraYOffset(), -1);
        if(gameObject.TryGetComponent<Camera>(out Camera cam))
        {
            cam.orthographicSize = DataGameToVictory.instance.GetGameCameraOrthSize();
        }
    }
    private void LateUpdate()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, DataGameToVictory.instance.GetGameCameraYOffset(), -1);
    }
}
