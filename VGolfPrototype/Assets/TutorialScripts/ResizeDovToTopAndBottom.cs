using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeDovToTopAndBottom : MonoBehaviour
{
    [SerializeField] RectTransform Top;
    [SerializeField] RectTransform Bottom;
    void Update()
    {
        float topH = Top.rect.height;
        float bottomH = Bottom.rect.height;
        float topC = Top.transform.localPosition.y - topH;
        float bottomC = Bottom.transform.localPosition.y + bottomH;
        RectTransform centerRect = transform.GetComponent<RectTransform>();
        centerRect.transform.localPosition = new Vector3(0, (topC + bottomC) / 2, centerRect.transform.position.z);
        Vector2 rec = centerRect.sizeDelta;
        rec.y = topC + Mathf.Abs(bottomC);
        centerRect.sizeDelta = rec;
    }
}
