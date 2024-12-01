using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardSurfaceCollision : MonoBehaviour
{
    public ExplodeOnContact explodeOnContact;
    private Rigidbody2D parentRigidbody; // Reference to parent's Rigidbody2D
    public float velocityThreshold = 5f;
    // Start is called before the first frame update
    void Start()
    {
        explodeOnContact = GetComponentInParent<ExplodeOnContact>();

        if (explodeOnContact == null)
        {
            Debug.LogWarning("ExplodeOnContact component not found in parent!");
        }

        parentRigidbody = GetComponentInParent<Rigidbody2D>();

        if (parentRigidbody == null)
        {
            Debug.LogWarning("Rigidbody2D not found in parent!");
        }
    }

    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "HardSurface")
        {
            // Check if the collision velocity exceeds the threshold
            if (collision.relativeVelocity.magnitude >= velocityThreshold )
            {
                // Trigger explosion
                if (explodeOnContact != null)
                {
                    explodeOnContact.HardSurfaceExplode();
                }
            }
        }
    }
}
