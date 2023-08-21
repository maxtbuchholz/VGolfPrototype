using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeGameUiCanvas : MonoBehaviour
{
    [SerializeField] RectTransform WidthRect;
    [SerializeField] Camera gameCamera;
    void Start()
    {
        RectTransform rectT = gameObject.GetComponent<RectTransform>();
        rectT.sizeDelta = new Vector2(WidthRect.rect.width, WidthRect.rect.width / gameCamera.aspect);
    }
}
