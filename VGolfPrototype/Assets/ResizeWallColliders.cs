using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeWallColliders : MonoBehaviour
{
    [SerializeField] BoxCollider2D TopCollider, BottomCollider, LeftCollider, RightCollider;
    void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        var rect = rectTransform.rect;
        RightCollider.size = new Vector2(1, rect.height);
        LeftCollider.size = new Vector2(1, rect.height);
        TopCollider.size = new Vector2(rect.width, 1);
        BottomCollider.size = new Vector2(rect.width, 1);
    }
}
