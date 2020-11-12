using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : PhysicsEffect
{
    private const float FORCE_MULTIPLIER = 0.5f;
    private const float FORCE_INCREMENT = 5f;

    public enum ForceType
    {
        ATTRACTION = -1,
        REPULSION = 1,
        NONE = 0
    }

    [SerializeField]
    private Transform originTransform;
    [SerializeField]
    private ForceType forceType = ForceType.ATTRACTION;
    [SerializeField]
    private float range = 5f;

    // Force variables
    [SerializeField]
    [Range(-30f, 30f)]
    private float force = 0f;
    private float minForce = -30f;
    private float maxForce = 30f;

    private SphereCollider sphereCollider;

    public GameObject forceField;

    protected override void Start()
    {
        base.Start();
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = range;
    }

    // Set forcetype
    private void Update()
    {
        if (force < 0)
        {
            forceType = ForceType.ATTRACTION;
        }
        else if (force > 0)
        {
            forceType = ForceType.REPULSION;
        }
        else if (force == 0)
        {
            forceType = ForceType.NONE;
        }

        if (force > maxForce)
        {
            force = maxForce;
        }
        if (force < minForce)
        {
            force = minForce;
        }
    }

    // TODO Applying a forceField gives all objects in its field a ConstantForce (Should not do this)
    public override void ApplyEffect(RaycastHit hit)
    {
        Transform existingForceFieldObj = hit.transform.Find("ForceField");
        if (existingForceFieldObj == null)
        {
            forceField = pShapeHit.transform.Find("ForceField").gameObject;
            Transform cloneForceFieldObj = hit.transform.Find("ForceField(Clone)");
            if (cloneForceFieldObj == null)
            {
                Instantiate(forceField, hit.transform);
            }
        }
        else
        {
            ForceField existingForceField = existingForceFieldObj.GetComponent<ForceField>();
            existingForceField.force = force;
            //existingForceField.SetRange(force);
        }
    }

    public override void RemoveEffect(GameObject obj)
    {
        Transform existingForceFieldObj = obj.transform.Find("ForceField(Clone)");
        if (existingForceFieldObj != null)
        {
            Destroy(existingForceFieldObj.gameObject);
        }
    }

    public override void ApplyEffect(RaycastHit hitInfo, GameObject player)
    {
        throw new System.NotImplementedException();
    }

    // Increase or decrease the force and keep it within bounds
    // Changing the magnitude of the Shape should also change it for its Linked Object
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
            if (force > minForce)
            {
                magnitude -= FORCE_INCREMENT;
            }
            else if (force <= minForce)
            {
                force = minForce;
            }
        }
        force = magnitude;
        range = 2 + (Mathf.Abs(force) / 10);
        SetRange(range);
        NofityEffectedObjsOfUpdatedMagnitude();
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("StaticObject"))
        {
            return;
        }
        if (col.CompareTag("PhysicsShape"))
        {
            return;
        }
        if (col.CompareTag("ForceField"))
        {
            return;
        }
        if (col.CompareTag("OrbitalField"))
        {
            return;
        }
        else if (col.CompareTag("PhysicsObject"))
        {
            originTransform = GetComponentInParent<Transform>();
            Rigidbody otherRB = col.GetComponent<Rigidbody>();
            Vector3 direction = (otherRB.position - originTransform.position).normalized;
            Vector3 resultantForce = direction * force * FORCE_MULTIPLIER;
            otherRB.AddForce(resultantForce, ForceMode.Impulse);
        }
    }

    public void SetRange(float range)
    {
        this.range = range;
        sphereCollider.radius = range;
    }
}
