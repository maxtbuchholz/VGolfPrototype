using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeGameBackground : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] RectTransform BackgroundCanvas;
    private GameObject CentralBackground;
    void Start()
    {
        CentralBackground = gameObject;
        ResizeBackground();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void ResizeBackground()
    {
        if (!CentralBackground.TryGetComponent<RectTransform>(out RectTransform RectB)) return;
        float height = BackgroundCanvas.rect.height;
        float width = BackgroundCanvas.rect.width;
        height /= RectB.rect.height;
        width /= RectB.rect.width;
        Vector2 centerSet = new Vector2(width * CentralBackground.transform.localScale.x, height * CentralBackground.transform.localScale.y);
        CentralBackground.transform.localScale = centerSet;
    }
}
