using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowBall : MonoBehaviour
{
    [SerializeField] Camera camera;
    [SerializeField] Canvas gameCanvas;
    [SerializeField] Transform ball;
    private RectTransform bounds;
    float boundRight;
    float boundLeft;
    float boundTop;
    float boundBottom;
    float verticalCameraExtent;
    float horizontalCameraExtent;
    void Start()
    {
        bounds = gameCanvas.GetComponent<RectTransform>();
        float cameraHeight = 2f * camera.orthographicSize;
        float cameraWidth = cameraHeight * camera.aspect;
        float canvasWidth = gameCanvas.GetComponent<RectTransform>().rect.width;
        float cameraCanvasDifference = canvasWidth / cameraWidth;
        camera.orthographicSize *= cameraCanvasDifference;

        boundRight = bounds.offsetMax[0];
        boundLeft = bounds.offsetMin[0];
        boundTop = bounds.offsetMax[1];
        boundBottom = bounds.offsetMin[1];

        verticalCameraExtent = camera.GetComponent<Camera>().orthographicSize;
        horizontalCameraExtent = verticalCameraExtent * Screen.width / Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 ballPos = new Vector3(ball.position.x, ball.position.y + (0.5f * verticalCameraExtent), transform.position.z);
        if((ballPos.x + horizontalCameraExtent) > boundRight)
        {
            ballPos.x = boundRight - horizontalCameraExtent;
        }
        else if ((ballPos.x - horizontalCameraExtent) < boundLeft)
        {
            ballPos.x = boundLeft + horizontalCameraExtent;
        }

        if ((ballPos.y + verticalCameraExtent) >  boundTop)
        {
            ballPos.y = boundTop - verticalCameraExtent;
        }
        else if ((ballPos.y - verticalCameraExtent) < boundBottom)
        {
            ballPos.y = boundBottom + verticalCameraExtent;
        }
        transform.position = new Vector3(ballPos[0], ballPos[1], transform.position.z);
    }
}
