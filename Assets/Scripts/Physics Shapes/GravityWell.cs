using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityWell : MonoBehaviour
{
    public bool invert = false;
    public float radius = 5f;
    public float force = 50f;
    [Range(0f, 1f)]
    public float fallOff = 0.9f;
    public LayerMask mask;

    [Header("==DEBUG==")]
    public bool debugRadius = false;
    public Color debugRadiusColor;
    public Color debugRadiusFallOffColor;

    private Transform thisTransform;
    private Rigidbody thisRb;

    private void Start()
    {
        thisTransform = this.transform;
        thisRb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Collider[] hits = Physics.OverlapSphere(thisTransform.position, radius, mask, QueryTriggerInteraction.Ignore);
        for (int i = 0; i < hits.Length; i++)
        {
            Rigidbody rb = hits[i].GetComponent<Rigidbody>();
            if (rb != null && rb != thisRb)
            {
                float distance = Vector3.Distance(thisTransform.position, rb.transform.position);
                float fallOffDist = radius * fallOff;
                float linearFalloutMulti = 1f - ((distance - fallOffDist) / (radius - fallOffDist));
                float forceMulti = (distance / radius <= fallOff) ? 1f : linearFalloutMulti;

                if (forceMulti < 0f)
                {
                    forceMulti = 0f;
                }

                float forceToApply = -force * forceMulti;
                if (invert)
                {
                    forceToApply *= -1;
                }
                rb.AddExplosionForce(forceToApply, thisTransform.position, radius, 0f, ForceMode.Impulse);
            }
        }
    }

    private void OnDrawGizmos() //Selected for when you click on the object
    {
        if (debugRadius)
        {
            Gizmos.color = debugRadiusColor;
            Gizmos.DrawWireSphere(this.transform.position, radius);

            Gizmos.color = debugRadiusFallOffColor;
            Gizmos.DrawWireSphere(this.transform.position, radius * fallOff);
        }
    }
}
