using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureManager : MonoBehaviour
{
    public static CaptureManager instance;

    public Transform player;
    public Transform cameraPivot;
    public Transform currentCapturePivot;
    public string capturedLayer = "3D-UI";

    private GameObject currentCapture;
    private PhysicsEffect currentEffect;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
    }

    private void Update()
    {
        cameraPivot.rotation = Quaternion.Euler(cameraPivot.rotation.eulerAngles.x, player.rotation.eulerAngles.y,
            cameraPivot.rotation.eulerAngles.z);
    }

    //Apply link for other objects
    public void applyLink(RaycastHit hit)
    {
        applyLink(hit, hit.collider.gameObject);
    }

    //Apply Link for the player
    public void applyLink(RaycastHit hit, GameObject obj)
    {
        //if (currentEffect != null)
        //{
            currentEffect = hit.transform.gameObject.GetComponent<PhysicsEffect>();
            obj.GetComponent<EffectsManager>().AddEffect(hit, currentEffect, obj);
        //}
    }

    public void setNewLink(RaycastHit pShapeHit, GameObject link)
    {
        clearLink();
        
        // Set currentEffect to linked effect
        if (link.GetComponent<PhysicsEffect>())
        {
            currentEffect = link.GetComponent<PhysicsEffect>();
            currentEffect.PrimeEffect(pShapeHit);
        }
        else
        {
            if (link.GetComponentInChildren<ForceField>())
            {
                currentEffect = link.GetComponentInChildren<ForceField>();
                currentEffect.PrimeEffect(pShapeHit);
            }
            else if (link.GetComponentInChildren<OrbitalField>())
            {
                currentEffect = link.GetComponentInChildren<OrbitalField>();
                currentEffect.PrimeEffect(pShapeHit);
            }
        }
        

        GameObject newLink = Instantiate(link, currentCapturePivot);
        Transform newLinkTransform = newLink.transform;
        newLinkTransform.localScale = new Vector3(1, 1, 1); // Clamp size to always keep in view
        newLinkTransform.localPosition = Vector3.zero;
        newLinkTransform.gameObject.layer = LayerMask.NameToLayer(capturedLayer);
        traverseChildren(newLinkTransform);

        currentCapture = newLink;
    }

    public void clearLink()
    {
        if (currentCapture != null)
        {
            Destroy(currentCapture);
        }
    }

    public void ChangeMagnitudeOfCurrentEffect(bool increment)
    {
        if(currentEffect != null)
        {
            currentEffect.ChangeMagnitude(increment);
        }
    }

    private void traverseChildren(Transform t)
    {
        foreach(Transform child in t)
        {
            child.gameObject.layer = LayerMask.NameToLayer(capturedLayer);
            traverseChildren(child);
        }
    }
}
