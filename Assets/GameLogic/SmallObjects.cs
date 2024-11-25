using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallObjects : MonoBehaviour
{
    public PointManager pointManager;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        GameObject myPointManager = GameObject.FindGameObjectWithTag("PointManager");
        pointManager = myPointManager.GetComponent<PointManager>();

        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object collided with something tagged as "Ground"
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Calculate the speed of the object on collision
            float speed = rb.velocity.magnitude;
            bool isCriticalHit = false;

            // Award points based on the speed
            int points = Mathf.RoundToInt(speed * 10); // Adjust multiplier as needed
            pointManager.myPoints += points;
            DamagePopup.Create(gameObject.transform.position, points, isCriticalHit);
            // Optional: Log for debugging
            Debug.Log($"Collision speed: {speed}, Points awarded: {points}");
        }
    }
}
