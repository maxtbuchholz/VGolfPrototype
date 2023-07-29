using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    [SerializeField] float verticleMvmtDiff = 2;
    [SerializeField] float horizontalMvmtDiff = 0;
    [SerializeField] float timeForFull = 3;
    [SerializeField] PlatformTypes platformtype;

    private Rigidbody2D r2bd;

    private float time = 0;
    private Vector2 originalPos;
    private Vector2 farPos;
    private void Start()
    {
        originalPos = transform.localPosition;
        farPos = originalPos + new Vector2(horizontalMvmtDiff, verticleMvmtDiff);
        r2bd = gameObject.GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        time += Time.deltaTime;
        time %= timeForFull;
        if (platformtype == PlatformTypes.sin)
        {
            float s = (time / timeForFull) * (2 * Mathf.PI);
            s = Mathf.Sin(s);
            transform.position = new Vector2(originalPos.x + (horizontalMvmtDiff * s), originalPos.y + (verticleMvmtDiff * s));
        }
        else if (platformtype == PlatformTypes.linear)
        {
            float t = (time / timeForFull) * 2;
            if(t < 1)
            {
                transform.localPosition = new Vector2(originalPos.x + (horizontalMvmtDiff * t), originalPos.y + (verticleMvmtDiff * t));
                //r2bd.velocity = new Vector2(0, 1);
            }
            else
            {
                //r2bd.velocity = new Vector2(0, -1);
                t -= 1;
                t = 1 - t;
                transform.localPosition = new Vector2(originalPos.x + (horizontalMvmtDiff * t), originalPos.y + (verticleMvmtDiff * t));
            }
        }
    }
}
enum PlatformTypes
{
    sin,
    linear
}
