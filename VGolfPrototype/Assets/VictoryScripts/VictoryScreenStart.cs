using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryScreenStart : MonoBehaviour
{
    [SerializeField] RectTransform SlideDown;
    [SerializeField] RectTransform SlideUp;
    [SerializeField] GameObject Background;
    [SerializeField] GameObject Background2;
    void Start()
    {
        //ResizeWallColliders();
        SizeToScreen();
        ResizeBackground();
        MoveToPlaces();
    }
    private void ResizeBackground()
    {
        if (!Background.TryGetComponent<RectTransform>(out RectTransform RectB)) return;
        float height = SlideDown.rect.height;
        float width = SlideDown.rect.width;
        height /= RectB.rect.height;
        width /= RectB.rect.width;
        height /= 100;
        width /= 100;
        Vector2 set = new Vector2(width * Background.transform.localScale.x, (height * Background.transform.localScale.y) / 2.0f);
        Background.transform.localScale = set;
        Background2.transform.localScale = set;
    }
    private void MoveToPlaces()
    {
        float height = SlideDown.rect.height;
        SlideDown.transform.localPosition = new Vector2(0,height / 1f);
        SlideUp.transform.localPosition = new Vector2(0, -height / 1f);
        Background.transform.localPosition = new Vector2(0, -height / 4f);
        Background2.transform.localPosition = new Vector2(0, height / 4f);
    }
    private void ResizeWallColliders()
    {
        BoxCollider2D Box = GetComponent<BoxCollider2D>();
        RectTransform rect = GetComponent<RectTransform>();
        Box.size = new Vector2(rect.rect.width, rect.rect.height);
    }
    public void SetToCenter()
    {
        SlideDown.transform.localPosition = new Vector2(0, SlideDown.rect.height / 2f);
        SlideUp.transform.localPosition = new Vector2(0, -SlideDown.rect.height /2f);
    }
    private void SizeToScreen()
    {

        //float cameraHeight = 2f * camera.orthographicSize;
        //float cameraWidth = cameraHeight * camera.aspect;
        //float canvasWidth = gameCanvas.GetComponent<RectTransform>().rect.width;
        //float cameraCanvasDifference = canvasWidth / cameraWidth;
        //camera.orthographicSize *= cameraCanvasDifference;
    }
}
