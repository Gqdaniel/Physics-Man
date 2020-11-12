using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativeGravityChange : PhysicsEffect
{
    private const float FORCE_INCREMENT = 0.2f;

    [SerializeField]
    [Range(0f, 2f)]
    public float force = 1;
    private float maxForce = 2;
    private float minForce = 0f;

    public override void ApplyEffect(RaycastHit hit)
    {
        Rigidbody otherRb = hit.rigidbody;
        otherRb.useGravity = false;
        Vector3 newGrav = pShapeHit.normal * -Physics.gravity.magnitude * force * otherRb.mass;

        ConstantForce gravity = otherRb.GetComponent<ConstantForce>();
        if (gravity == null)
        {
            gravity = otherRb.gameObject.AddComponent<ConstantForce>();
        }
        gravity.force = newGrav;
    }

    public override void ApplyEffect(RaycastHit hitInfo, GameObject player)
    {
        throw new System.NotImplementedException();
    }

    public override void RemoveEffect(GameObject obj)
    {
        ConstantForce gravity = obj.GetComponent<ConstantForce>();
        if(gravity != null)
        {
            obj.GetComponent<Rigidbody>().useGravity = true;
            Destroy(gravity);
        }
    }

    // Increase or decrease the force and keep it within bounds
    public override void ChangeMagnitude(bool increment)
    {
        // Info for collision script
        CollisionGravityChange collisionGravityChange = GetComponent<CollisionGravityChange>();

        magnitude = force;
        if (increment)
        {
            if (force < maxForce)
            {
                magnitude += FORCE_INCREMENT;
            }
            else if (force > maxForce)
            {
                force = maxForce;
            }
        }
        else
        {
            if (force > minForce)
            {
                magnitude -= FORCE_INCREMENT;
            }
            else if (force < minForce)
            {
                force = minForce;
            }
        }
        force = magnitude;

        // duplicate effect in collison script
        collisionGravityChange.force = force;
        NofityEffectedObjsOfUpdatedMagnitude();
    }

    private void Update()
    {
        if (force > maxForce)
        {
            force = maxForce;
        }
        if (force < minForce)
        {
            force = minForce;
        }
    }
}
