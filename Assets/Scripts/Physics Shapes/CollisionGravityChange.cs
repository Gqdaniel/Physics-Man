using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionGravityChange : MonoBehaviour
{
    private float range = 100f;
    public Transform otherTransform;
    public LayerMask mask;

    [SerializeField]
    [Range(0.1f, 2f)]
    public float force;
    ConstantForce gravity;

    // Raycast from collision to Dodec transform.0 . Set gravity according to hit normal
    public void OnCollisionEnter(Collision collision)
    {
        otherTransform = collision.transform;
        Vector3 direction = (this.transform.position - otherTransform.position).normalized;
        gravity = otherTransform.GetComponent<ConstantForce>();

        RaycastHit hit;
        if (Physics.Raycast(otherTransform.position, direction, out hit, range, mask, QueryTriggerInteraction.Ignore))
        {
            Vector3 normal = hit.normal;
            Vector3 newGrav = normal * -9.81f * force * collision.rigidbody.mass;

            if (gravity == null)
            {
                gravity = collision.gameObject.AddComponent<ConstantForce>();

                // Turn off gravity, use new gravity
                collision.rigidbody.useGravity = false;
                gravity.force = newGrav;
            }
            else
            {
                newGrav = normal * -9.81f * force * collision.rigidbody.mass;
                gravity.force = newGrav;
            }
        }
    }
}
