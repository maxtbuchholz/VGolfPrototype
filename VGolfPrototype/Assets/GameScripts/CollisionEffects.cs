using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class CollisionEffects: MonoBehaviour
{
    [SerializeField] ParticleSystem collisionParticle;
    private Rigidbody2D rg2d;
    // Start is called before the first frame update
    void Start()
    {
        rg2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float speed = rg2d.velocity.magnitude;
        if (speed > 0.5)
            lastSpeed = speed;
    }
    float lastSpeed = 0.5f;
    void OnCollisionEnter2D(Collision2D col)
    {
        foreach (ContactPoint2D contact in col.contacts)
        {
            if(lastSpeed > 10)
                ShowAndRunParticle(contact.point);
        }
    }
    private void ShowAndRunParticle(Vector2 point)
    {
        ParticleSystem par = GameObject.Instantiate(collisionParticle);
        Burst bur = par.emission.GetBurst(0);
        bur.count = new MinMaxCurve { constant = ( 2 * lastSpeed) - 5 };
        var main = par.main;
        main.startSpeed = new ParticleSystem.MinMaxCurve { constantMin = 0.05f, constantMax = (lastSpeed / 8) }; //(lastSpeed / 8) + 0.5f
        //par.main = main;//.startSpeed = new ParticleSystem.MinMaxCurve { constantMin = 1, constantMax = (lastSpeed / 3) + 1 };
        float angle = Mathf.Rad2Deg * (Mathf.Atan2(transform.position.y - point.y, transform.position.x - point.x));
        //Vector2 vectorAngle = new Vector2(transform.position.x - point.x, transform.position.y - point.y);
        par.transform.eulerAngles = new Vector3(0,0, angle - 117.5f);
        par.transform.position = new Vector3(point.x, point.y, transform.position.z);
        par.Play();
    }
}
