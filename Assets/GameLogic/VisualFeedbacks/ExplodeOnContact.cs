using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Explodable))]
public class ExplodeOnContact : MonoBehaviour
{
    private Explodable _explodable;
    public float explosionForceThreshold = 2f; // Minimum collision force required to trigger explosion
    public PointManager pointManager;
    public GameManager gameManager;
    public int objectType = 0;
    //0 = small
    //1 = medium
    //2 = large

    void Start()
    {
        GameObject myPointManager = GameObject.FindGameObjectWithTag("PointManager");
        pointManager = myPointManager.GetComponent<PointManager>();
        GameObject myuGameManager = GameObject.FindGameObjectWithTag("GameManager");
        gameManager = myuGameManager.GetComponent<GameManager>();
        _explodable = GetComponent<Explodable>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Calculate the collision force
        float collisionForce = collision.relativeVelocity.magnitude;

        // Check if the force exceeds the threshold
        if (collisionForce >= explosionForceThreshold)
        {
            //Add Time
            if(objectType == 0)
            {
                gameManager.AddTimeSmallObject();
            }
            else if (objectType == 1)
            {
                gameManager.AddTimeMediumObject();
            }
            else if (objectType == 2)
            {
                gameManager.AddTimeBigObject();
            }

            bool isCriticalHit = false;
            int points = Mathf.RoundToInt(collisionForce * 10); // Adjust multiplier as needed
            pointManager.AddPoints(points);
            pointManager.ComboMeter();
            DamagePopup.Create(gameObject.transform.position, points, isCriticalHit);
            // Trigger explosion
            _explodable.explode(collision.transform.position, collision.relativeVelocity.magnitude*0.1f);

            // Apply explosion force to nearby objects
            ExplosionForce ef = GameObject.FindObjectOfType<ExplosionForce>();
            if (ef != null)
            {
                ef.doExplosion(transform.position);
            }
        }
    }

    public void HardSurfaceExplode()
    {
        float collisionForce = 20;
        //Add Time
        if (objectType == 0)
        {
            collisionForce = 20;
            gameManager.AddTimeSmallObject();
        }
        else if (objectType == 1)
        {
             collisionForce = 40;
            gameManager.AddTimeMediumObject();
        }
        else if (objectType == 2)
        {
             collisionForce = 80;
            gameManager.AddTimeBigObject();
        }

        bool isCriticalHit = false;
        int points = Mathf.RoundToInt(collisionForce * 10); // Adjust multiplier as needed
        pointManager.AddPoints(points);
        pointManager.ComboMeter();
        DamagePopup.Create(gameObject.transform.position, points, isCriticalHit);
        // Trigger explosion
        _explodable.explode(transform.position,1f);

        // Apply explosion force to nearby objects
        ExplosionForce ef = GameObject.FindObjectOfType<ExplosionForce>();
        if (ef != null)
        {
            ef.doExplosion(transform.position);
        }
    }

}
