using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryScreenStart : MonoBehaviour
{
    [SerializeField] RectTransform RectT;
    void Start()
    {
        //ResizeWallColliders();
        MoveToTop();
    }
    private void MoveToTop()
    {
        float height = RectT.rect.height;
        RectT.transform.localPosition = new Vector2(0,height);
    }
    private void ResizeWallColliders()
    {
        BoxCollider2D Box = GetComponent<BoxCollider2D>();
        RectTransform rect = GetComponent<RectTransform>();
        Box.size = new Vector2(rect.rect.width, rect.rect.height);
    }
}
