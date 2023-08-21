using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryScreenStart : MonoBehaviour
{
    //[SerializeField] RectTransform SlideDown;
    //[SerializeField] RectTransform SlideUp;
    //[SerializeField] GameObject Background;
    //[SerializeField] GameObject Background2;
    [SerializeField] GameObject CentralBackground;
    [SerializeField] RectTransform Page;
    [SerializeField] RectTransform Pagemask;
    [SerializeField] Rigidbody2D PageRdb2;
    [SerializeField] RectTransform BackgroundCanvas;
    [SerializeField] ThumbsUpAnimation ThumbsUpA;
    [SerializeField] List<Transform> Ball;
    [SerializeField] WaveTextEffect ThanksFor;
    [SerializeField] WaveTextEffect Playing;
    void Start()
    {
        DataGameToVictory.instance.GetGameCamera().GetComponent<Camera>().enabled = false;
        //ResizeWallColliders();
        //SizeToScreen();
        ResizeBackground();
        MoveToPlaces();
    }
    private void ResizeBackground()
    {
        if (!CentralBackground.TryGetComponent<RectTransform>(out RectTransform RectB)) return;
        float height = BackgroundCanvas.rect.height;
        float width = BackgroundCanvas.rect.width;
        Page.sizeDelta = BackgroundCanvas.rect.size;
        height /= RectB.rect.height;
        width /= RectB.rect.width;
        height /= 100;
        width /= 100;
        //Vector2 set = new Vector2(width * Background.transform.localScale.x, (height * Background.transform.localScale.y) / 2.0f);
        Vector2 centerSet = new Vector2(width * CentralBackground.transform.localScale.x, height * CentralBackground.transform.localScale.y);
        //Background.transform.localScale = set;
        //Background2.transform.localScale = set;
        CentralBackground.transform.localScale = centerSet;
        //Pagemask.transform.localScale = centerSet;
        //BottomCollider.transform.localPosition = new Vector2(0,-0.5f);
        ThumbsUpA.StartAnimation();
    }
    private void MoveToPlaces()
    {
        float height = BackgroundCanvas.rect.height;
        //Vector3 ballPos = Ball.transform.localPosition;
        Page.localPosition = new Vector2(0, -height);
        //Ball.transform.localPosition = ballPos;
    }
    bool inPlace = false;
    private void Update()
    {
        if(Page.localPosition.y < 0)
        {
            PageRdb2.velocity = new Vector2(0, 30);
        }
        else if(!inPlace)
        {
            inPlace = true;
            PageRdb2.velocity = new Vector2(0, 0);
            Page.localPosition = new Vector2(0, 0);
            PageRdb2.bodyType = RigidbodyType2D.Static;
            ThanksFor.Go();
            Playing.Go();
            for (int i = 0; i < Ball.Count; i++)
            {
                Ball[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }
        }

    }
    //private void ResizeWallColliders()
    //{
    //    BoxCollider2D Box = GetComponent<BoxCollider2D>();
    //    RectTransform rect = GetComponent<RectTransform>();
    //    Box.size = new Vector2(rect.rect.width, rect.rect.height);
    //}
    //public void SetToCenter()
    //{
    //    SlideDown.transform.localPosition = new Vector2(0, SlideDown.rect.height / 2f);
    //    SlideUp.transform.localPosition = new Vector2(0, -SlideDown.rect.height /2f);
    //    ThumbsUpA.StartAnimation();
    //}
    //private void SizeToScreen()
    //{

    //    //float cameraHeight = 2f * camera.orthographicSize;
    //    //float cameraWidth = cameraHeight * camera.aspect;
    //    //float canvasWidth = gameCanvas.GetComponent<RectTransform>().rect.width;
    //    //float cameraCanvasDifference = canvasWidth / cameraWidth;
    //    //camera.orthographicSize *= cameraCanvasDifference;
    //}
}
