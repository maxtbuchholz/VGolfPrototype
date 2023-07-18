using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SizeCameraToScreen : MonoBehaviour
{
    [SerializeField] Camera camera;
    [SerializeField] Canvas gameCanvas;
    // Start is called before the first frame update
    void Start()
    {
        float cameraHeight = 2f * camera.orthographicSize;
        float cameraWidth = cameraHeight * camera.aspect;
        float canvasWidth = gameCanvas.GetComponent<RectTransform>().rect.width;
        float cameraCanvasDifference = canvasWidth / cameraWidth;
        camera.orthographicSize *= cameraCanvasDifference;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
