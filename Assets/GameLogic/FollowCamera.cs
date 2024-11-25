using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target; // Reference to the object to follow
    public float smoothSpeed = 5f; // Speed of the smooth follow

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            // Calculate the new position with Lerp
            Vector3 newPosition = Vector3.Lerp(transform.position, target.position, smoothSpeed * Time.deltaTime);

            // Update the position of this object
            transform.position = newPosition;
        }
    }
}
