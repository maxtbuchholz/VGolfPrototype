using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeGameUiCanvas : MonoBehaviour
{
    [SerializeField] RectTransform WidthRect;
    [SerializeField] Camera gameCamera;
    [SerializeField] RectTransform ViewCanvas;
    [SerializeField] List<Transform> UiElements;
    void Start()
    {
        RectTransform rectT = gameObject.GetComponent<RectTransform>();
        float multiplyer = WidthRect.rect.width / ViewCanvas.rect.width;
        foreach(Transform tform in UiElements)
        {
            tform.localScale = new Vector3(tform.localScale.x * multiplyer, tform.localScale.y * multiplyer, tform.localScale.z * multiplyer);
        }
        rectT.sizeDelta = new Vector2(WidthRect.rect.width, WidthRect.rect.width / gameCamera.aspect);
    }
}
