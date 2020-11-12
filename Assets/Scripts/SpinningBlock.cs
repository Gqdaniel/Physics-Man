using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningBlock : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Directional vector of how the block should rotate.  All values will be normalized")]
    private Vector3 rotationVector = new Vector3(5.0f, 0.0f, 0.0f);
    [SerializeField]
    private Vector3 magnitudeVector = Vector3.one;

    private Transform thisTransform;

    private void Start()
    {
        thisTransform = this.transform;
    }

    private void FixedUpdate()
    {
        Vector3 v = rotationVector.normalized;
        v.Scale(magnitudeVector);
        thisTransform.Rotate(v, Space.Self);
    }
}
