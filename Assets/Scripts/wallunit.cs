using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallunit : MonoBehaviour
{
    [SerializeField] private Vector3 overlapBoxSize;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wallunit"))
        {
            // Get the orientation of the collided wall unit
            Quaternion orientation = collision.gameObject.transform.rotation;

            // Set the orientation of this wall unit to match the collided one
            transform.rotation = orientation;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hole") 
            || other.gameObject.CompareTag("Ball")
            || other.gameObject.CompareTag("UndoRedo")
            )
        {
            // Check if there is any overlap with other objects
            Collider[] colliders = Physics.OverlapBox(transform.position, overlapBoxSize / 2f);

            // If there is any overlap, move the wall unit up until there is no overlap
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject != gameObject)
                {
                    transform.position += Vector3.up * (collider.bounds.size.y + overlapBoxSize.y) / 2f;
                }
            }
        }
    }
}
