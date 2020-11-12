using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalField : PhysicsEffect
{
    private const float FORCE_INCREMENT = 2f;

    public GameObject orbitalField;
    public float satelliteSpeed = 10f;
    public float maxSpeed = 10f;
    public bool debugPaths = false;
    public float force = 0;
    private float maxForce = 10;
    private float minForce = 1;
    [SerializeField]
    private float range = 5f;

    private SphereCollider sphereCollider;

    private Transform thisTransform;
    private Dictionary<Collider, OrbitalData> localUps;

    private class OrbitalData
    {
        public Vector3 up;
        public Rigidbody rb;
        public Transform thisTransform;
        public GameObject orbitalField;

        public OrbitalData(Vector3 up, Rigidbody rb, Transform thisTransform)
        {
            this.up = up;
            this.rb = rb;
            this.thisTransform = thisTransform;
        }
    }

    protected override void Start()
    {
        base.Start();
        thisTransform = this.transform;
        sphereCollider = GetComponent<SphereCollider>();
        localUps = new Dictionary<Collider, OrbitalData>();
    }

    public override void ApplyEffect(RaycastHit objectHit)
    {
        Transform existingOrbitalFieldObj = objectHit.transform.Find("OrbitalField");
        if (existingOrbitalFieldObj == null)
        {
            orbitalField = pShapeHit.transform.Find("OrbitalField").gameObject;
            Transform cloneOrbitFieldObj = objectHit.transform.Find("OrbitalField(Clone)");
            if (cloneOrbitFieldObj == null)
            {
                Instantiate(orbitalField, objectHit.transform);
            }
        }
        else
        {
            OrbitalField existingOrbitalField = existingOrbitalFieldObj.GetComponent<OrbitalField>();
            existingOrbitalField.force = force;
        }
    }

    public override void ApplyEffect(RaycastHit hitInfo, GameObject player)
    {
        throw new System.NotImplementedException();
    }

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
        range = 5 + (Mathf.Abs(force) / 10);
        SetRange(range);
        NofityEffectedObjsOfUpdatedMagnitude();
    }

    public override void RemoveEffect(GameObject obj)
    {
        Transform existingOrbitalFieldObj = obj.transform.Find("OrbitalField(Clone)");
        if (existingOrbitalFieldObj != null)
        {
            Destroy(existingOrbitalFieldObj.gameObject);
        }
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
        else
        {
            Vector3 up;
            Rigidbody targetRb;
            Transform target;
            Vector3 lookAt;
            //if we don't know this collider
            //add it into the dict with its calculated up val
            if(!localUps.ContainsKey(col))
            {
                targetRb = col.GetComponent<Rigidbody>();
                target = col.transform;
                lookAt = thisTransform.position - target.position;
                up = Quaternion.AngleAxis(90f, Vector3.forward) * lookAt;
                localUps.Add(col, new OrbitalData(up, targetRb, target));
            }
            else //otherwise just grab the precalculated values
            {
                OrbitalData current = localUps[col];
                targetRb = current.rb;
                target = current.thisTransform;
                lookAt = thisTransform.position - target.position;
                up = current.up;
            }

            Vector3 dir = Vector3.Cross(lookAt, up).normalized;
            targetRb.AddForce(dir * satelliteSpeed, ForceMode.VelocityChange);
            if(targetRb.velocity.magnitude > maxSpeed)
            {
                targetRb.velocity = dir * maxSpeed;
                //Debug.Log("Reached max force!");
            }
            else
            {
                //Debug.Log("Adding force to body!");
            }
            targetRb.useGravity = false;

#if UNITY_EDITOR
            if(debugPaths)
            {
                Debug.DrawRay(target.position, up.normalized * 30f, Color.red, 0.5f);
                Debug.DrawRay(target.position, lookAt.normalized * 30f, Color.yellow, 0.5f);
                Debug.DrawRay(target.position, dir.normalized * 30f, Color.green, 0.5f);
            }
#endif
        }
    }

    private void OnTriggerExit(Collider col)
    {
        OrbitalData current = localUps[col];
        current.rb.useGravity = true;
        localUps.Remove(col);
    }

    public void SetRange(float range)
    {
        this.range = range;
        sphereCollider.radius = range;
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
