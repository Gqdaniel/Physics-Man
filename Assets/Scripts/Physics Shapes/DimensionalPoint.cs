using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionalPoint : PhysicsEffect
{
    public override void ApplyEffect(RaycastHit objectHit)
    {
        Transform thisTransform = this.transform;
        Transform otherTransform = objectHit.transform;
        Vector3 thisPosition = thisTransform.position;
        Vector3 otherPosition = otherTransform.position;
        Vector3 thisScale = thisTransform.localScale;
        Vector3 otherScale = otherTransform.localScale;
        float icoVol = thisScale.x * thisScale.y * thisScale.z;
        float otherVol = otherScale.x * otherScale.y * otherScale.z;


        otherTransform.position = thisPosition;
        thisTransform.position = otherPosition;


        //thisScale.x = Mathf.Pow(icoVol, 1f / 3f);
        //thisScale.y = Mathf.Pow(icoVol, 1f / 3f);
        //thisScale.z = Mathf.Pow(icoVol, 1f / 3f);
        otherTransform.localScale = thisScale;


        //otherScale.x = Mathf.Pow(otherVol, 1f / 3f);
        //otherScale.y = Mathf.Pow(otherVol, 1f / 3f);
        //otherScale.z = Mathf.Pow(otherVol, 1f / 3f);
        thisTransform.localScale = otherScale;  
    }

    public override void ApplyEffect(RaycastHit hitInfo, GameObject player)
    {
        throw new System.NotImplementedException();
    }

    public override void RemoveEffect(GameObject obj)
    {
        // Spacial point does not have anything to remove

    }
    public override void ChangeMagnitude(bool increment)
    {
        throw new System.NotImplementedException();
    }

}
