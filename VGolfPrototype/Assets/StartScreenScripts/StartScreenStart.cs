using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreenStart : MonoBehaviour
{
    [SerializeField] GameObject CentralBackground;
    [SerializeField] RectTransform Page;
    [SerializeField] RectTransform BackgroundCanvas;
    void Start()
    {
        ResizeBackground();
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
        Vector2 centerSet = new Vector2(width * CentralBackground.transform.localScale.x, height * CentralBackground.transform.localScale.y);
        CentralBackground.transform.localScale = centerSet;
    }
    private void Update()
    {
        
    }
}
