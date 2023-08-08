using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGravityhandler : MonoBehaviour
{
    List<int> IDS;
    Rigidbody2D rgd2;
    private void Start()
    {
        IDS = new List<int>();
        rgd2 = transform.GetComponent<Rigidbody2D>();
    }
    private void FixAndStart()
    {
        IDS = new List<int>();
        rgd2 = transform.GetComponent<Rigidbody2D>();
    }
    public void UpdateGravityInfluence(int ID, bool Active)
    {
        if (IDS == null) FixAndStart();
        if (Active)
        {
            if (IDS.Contains(ID))
            {

            }
            else
            {
                IDS.Add(ID);
                if(IDS.Count == 1)
                    SetGlobalGravity(false);
            }
        }
        else
        {
            if (IDS.Contains(ID))
            {
                IDS.Remove(ID);
            }
            if (IDS.Count == 0)
                SetGlobalGravity(true);
        }
    }
    private void SetGlobalGravity(bool isActive)
    {
        if (isActive)
            rgd2.gravityScale = 1;
        else
            rgd2.gravityScale = 0;
    }
}
