using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject player;
    public float range = 100f;
    public Transform origin;
    public LayerMask mask;

    private Collider previousHitCollider;

    private void Update()
    {
        if(PauseMenu.isPaused)
        {
            return;
        }

        RaycastHit hit;
        Collider hitCollider = null;
        if (Physics.Raycast(origin.position, origin.forward, out hit, range, mask, QueryTriggerInteraction.Ignore))
        {
            hitCollider = hit.collider;
        }

        if(hitCollider != null)
        {
            // Determines whether setting a Link or applying one
            if (Input.GetButtonDown("Fire1"))
            {
                shoot(false, hit);
            }
            else if (Input.GetButtonDown("Fire2"))
            {
                shoot(true, hit);
            }

            if (Input.GetButtonDown("ClearLink"))
            {
                Collider col = hit.collider;
                //clear all the effects of one specific obj
                if (col.CompareTag("PhysicsObject"))
                {
                    col.GetComponent<EffectsManager>().RemoveAllEffects();
                }
                else if (col.CompareTag("PhysicsShape"))
                {
                    GetEffect(col.transform).RemoveEffectFromAllObjs();
                }
            }
        }

        // Determines whether increasing or desreasing a magnitude
        if (Input.mouseScrollDelta.y < 0)
        {
            ChangeMagnitude(true, hitCollider);
        }
        else if (Input.mouseScrollDelta.y > 0)
        {
            ChangeMagnitude(false, hitCollider);
        }

        UpdateEffectsText(hitCollider);
        UpdateHighlights(hitCollider);
        //save this frame's hit collider so we can reference later if need be
        previousHitCollider = hitCollider;
    }

    private void shoot(bool link, RaycastHit hit)
    {
        Collider col = hit.collider;
        PhysicsEffect physicsEffect = GetEffect(col.transform);

        // Links a Physics Shape, and stores its properties
        if (link)
        {
            if (col.CompareTag("PhysicsShape"))
            {
                ActiveEffectDisplay.instance.StoreEffectText(physicsEffect);
                CaptureManager.instance.setNewLink(hit, col.gameObject); // Does not hit field shapes
            }
        }
        // Shoots the stored properties into a PhysicsObject (Any regular object)
        else
        {
            //apply to player
            if (col.CompareTag("PhysicsShape"))
            {
                CaptureManager.instance.applyLink(hit, player);
            }
            //apply to object
            else if(col.CompareTag("PhysicsObject"))
            {
                CaptureManager.instance.applyLink(hit);
            }
        }
    }

    // Change so if you are looking at a Physics Shape it changes that Shape and its derivatives
    // and if you are looking at anything else, it defaults to your Linked Shape
    private void ChangeMagnitude(bool increment, Collider hitCollider)
    {
        if (hitCollider != null)
        {
            if(hitCollider.CompareTag("PhysicsShape"))
            {
                GetEffect(hitCollider.transform).ChangeMagnitude(increment);
            }
            else
            {
                CaptureManager.instance.ChangeMagnitudeOfCurrentEffect(increment);
            }
        }
        else
        {
            CaptureManager.instance.ChangeMagnitudeOfCurrentEffect(increment);
        }
    }

    private void UpdateEffectsText(Collider hitCollider)
    {
        if (hitCollider != null)
        {
            if (hitCollider.CompareTag("PhysicsObject"))
            {
                List<PhysicsEffect> activeEffects = hitCollider.GetComponent<EffectsManager>().GetActiveEffects();
                ActiveEffectDisplay.instance.DisplayEffectText(activeEffects);
            }
            else
            {
                ActiveEffectDisplay.instance.ClearEffectDisplay();
            }
        }
        else
        {
            ActiveEffectDisplay.instance.ClearEffectDisplay();
        }
    }

    private void UpdateHighlights(Collider hitCollider)
    {
        //only update highlights if we are looking somewhere new (either at a new object, or at nothing)
        if(hitCollider != previousHitCollider)
        {
            //if we weren't previously looking at nothing, and whatever we looked at before wasn't a 
            //static object (meaning it could be highlighted), then unhighlight it
            if(previousHitCollider != null && !previousHitCollider.CompareTag("StaticObject"))
            {
                GetInteractable(previousHitCollider).Unhighlight();
            }

            //if we are now currently looking at something that isn't a static object
            //(meaning it can be highlighted), then highlight it
            if(hitCollider != null && !hitCollider.CompareTag("StaticObject"))
            {
                GetInteractable(hitCollider).Highlight();
            }
        }
    }

    private Interactable GetInteractable(Collider col)
    {
        //try to get interactable from top level obj
        Interactable interactable = col.GetComponent<Interactable>();
        //if we can't, must be a forcefield of some type, so get it from their child
        if (interactable == null)
        {
            interactable = col.GetComponentInChildren<Interactable>();
        }
        return interactable;
    }

    private PhysicsEffect GetEffect(Transform obj)
    {
        Transform forceFieldObj = obj.Find("ForceField");
        if (forceFieldObj != null)
        {
            return forceFieldObj.GetComponent<PhysicsEffect>();
        }
        return obj.GetComponent<PhysicsEffect>();
    }
}
