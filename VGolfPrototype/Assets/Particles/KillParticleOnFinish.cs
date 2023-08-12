using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillParticleOnFinish : MonoBehaviour
{
    ParticleSystem thisPS;
    void Start()
    {
        thisPS = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!thisPS.isPlaying)
            Destroy(gameObject);
    }
}
