using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorForce : PhysicsEffect
{
    private const float FORCE_INCREMENT = 5;

    public GameObject player;
    public Transform arrowPivotPoint;
    public Transform directionVector;
    public float force = 30f;
    private float maxForce = 100;
    private float minForce = 10;

    //public int maxNumScorefulUses = 5;

    //private int totalTimesUsed = 0;

    // Applies a vector force to a PhysicsObject
    public override void ApplyEffect(RaycastHit hit)
    {
        hit.rigidbody.AddForce(getDirectionVector(), ForceMode.Impulse);
    }

    public override void ApplyEffect(RaycastHit hitInfo, GameObject player)
    {
        Debug.Log("Applying to player" + hitInfo.collider); // Not printing
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.AddForce(getDirectionVector(), ForceMode.VelocityChange);
    }

    public override void RemoveEffect(GameObject obj)
    {
        //vector force does not have anything to remove
    }

    // Increase or decrease the force and keep it within bounds
    public override void ChangeMagnitude(bool increment)
    {
        magnitude = force;
        if (increment)
        {
            if (force < maxForce)
            {
                magnitude += FORCE_INCREMENT;
            }
            else if (force >= maxForce)
            {
                force = maxForce;
            }
        }
        else
        {
            if(force > minForce)
            {
                magnitude -= FORCE_INCREMENT;
            }
            else if (force <= minForce)
            {
                force = minForce;
            }
        }
        force = magnitude;
    }

    public Vector3 getDirectionVector()
    {
        return directionVector.up.normalized * force;
    }

    public Vector3 getCurrentVector()
    {
        if (directionVector == null)
        {
            return Vector3.zero;
        }

        Vector3 result = getDirectionVector();
        return result;
    }

    protected override void Start()
    {
        base.Start();
        ////force = Random.Range(MovementBlockData.instance.minForce, MovementBlockData.instance.maxForce);
        //if (!MovementBlockData.instance.isValidForce(force))
        //{
        //    Debug.LogError("Force " + force + " on MovementBlock " + name + " is outside min max force ranges " + MovementBlockData.instance.forceRangeToString() + "!");
        //    return;
        //}

        float yScale = PShapeData.instance.getForceScaleRatio(force);
        arrowPivotPoint.localScale = new Vector3(arrowPivotPoint.localScale.x, yScale, arrowPivotPoint.localScale.z);

        Material mat = directionVector.GetComponent<Renderer>().material;
        mat.color = PShapeData.instance.getForceColor(force);
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
