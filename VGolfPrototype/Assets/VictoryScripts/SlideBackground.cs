using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideBackground : MonoBehaviour
{
    [SerializeField] Rigidbody2D SlideDownRb2D;
    [SerializeField] Rigidbody2D SlideUpRb2D;
    [SerializeField] VictoryScreenStart VStart;
    bool hitBottom = false;
    void FixedUpdate()
    {
        if (!hitBottom)
        {
            SlideDownRb2D.velocity =  new Vector2(0, -20);
            SlideUpRb2D.velocity =  new Vector2(0, 20);
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        hitBottom = true;
        SlideDownRb2D.velocity = new Vector2(0, 0);
        SlideUpRb2D.velocity = new Vector2(0, 0);
        //VStart.SetToCenter();
    }
}
