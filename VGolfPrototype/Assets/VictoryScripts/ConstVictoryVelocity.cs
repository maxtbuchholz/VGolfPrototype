using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstVictoryVelocity : MonoBehaviour
{
    private Rigidbody2D rgbd;
    private float speed = 35f;
    private void Start()
    {
        rgbd = gameObject.GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        if (rgbd == null)
        {
            if (!gameObject.TryGetComponent<Rigidbody2D>(out rgbd))
                return;
        }
        else
        {
            Vector2 dir = rgbd.velocity.normalized;
            if (rgbd.velocity.magnitude < speed)
                rgbd.velocity = dir * speed;
        }
    }
}
