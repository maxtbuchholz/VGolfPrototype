using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCounter : MonoBehaviour
{
    public int Collisions = 0;
    void OnCollisionEnter2D(Collision2D col)
    {
        Collisions++;
    }
}
