using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPad : MonoBehaviour
{
    [SerializeField] Rigidbody2D rbd2;
    [SerializeField] float UpwardForce = 30;
    [SerializeField] float ResetDownForce = 5;
    [SerializeField] float MaxYDist = 2;
    private bool Launch = false;
    private bool frstLaunchComplete = false;
    private bool secondDownStarted = false;
    void FixedUpdate()
    {
        if (Launch)
        {
            LaunchPlatformGo();
        }
    }
    void LaunchPlatformGo()
    {
        Vector2 distFromStart = transform.localPosition;
        if (true)
        {
            if (!frstLaunchComplete)
            {
                frstLaunchComplete = true;
                rbd2.velocity = transform.up * UpwardForce;
            }
            else if (transform.localPosition.y > MaxYDist)
            {
                secondDownStarted = true;
                rbd2.velocity = transform.up * -ResetDownForce;
            }
            else if ((transform.localPosition.y < 0) && secondDownStarted)
            {
                rbd2.velocity = new Vector2(0, 0);
                frstLaunchComplete = false;
                secondDownStarted = false;
                Launch = false;
            }
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        Launch = true;
    }
    public void GhostUpdate()
    {
        if (Launch)
        {
            LaunchPlatformGo();
        }
    }
    //public void GhostTrigger()
    //{
    //    Launch = true;
    //}
    public void Reset()
    {
        rbd2.velocity = new Vector2(0, 0);
        transform.localPosition = new Vector2(0, 0);
        frstLaunchComplete = false;
        Launch = false;
    }
}
