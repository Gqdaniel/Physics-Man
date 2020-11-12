using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PhysicsEffect : MonoBehaviour, Interactable
{
    public string effectName = "DefaultEffectName";
    public Material regMaterial;
    public Material highlightedMaterial;

    //is the effect only applied once on click?
    public bool isOneShotEffect = false;

    //the magnitude of the effect
    protected float magnitude = 0;

    //pShapeHit is the hit info from when we first shot the pShape
    protected RaycastHit pShapeHit;

    //keeps track of all the objs currently effected by this effect
    protected List<EffectsManager> effectedObjs = new List<EffectsManager>();

    private Renderer rend;

    protected virtual void Start()
    {
        rend = GetComponent<MeshRenderer>();
        if(rend == null)
        {
            rend = GetComponentInParent<MeshRenderer>();
        }
    }

    //pShapeHit is the hit info from the linked pShape
    public void PrimeEffect(RaycastHit pShapeHit)
    {
        this.pShapeHit = pShapeHit;
    }

    public void AddEffectedObj(EffectsManager obj)
    {
        effectedObjs.Add(obj);
    }

    public void RemoveEffectedObj(EffectsManager obj)
    {
        effectedObjs.Remove(obj);
        RemoveEffect(obj.gameObject);
    }

    public void RemoveEffectFromAllObjs()
    {
        while(effectedObjs.Count > 0)
        {
            effectedObjs[0].RemoveEffect(this);
        }
    }

    public void NofityEffectedObjsOfUpdatedMagnitude()
    {
        for(int i = 0; i < effectedObjs.Count; i++)
        {
            effectedObjs[i].UpdateMagnitude(this);
        }
    }

    public void Highlight()
    {
        rend.material = highlightedMaterial;
    }

    public void Unhighlight()
    {
        rend.material = regMaterial;
    }

    //objectHit is the hit info from the obj we are trying to apply the effect to
    public abstract void ApplyEffect(RaycastHit objectHit);
    public abstract void ApplyEffect(RaycastHit hitInfo, GameObject player);
    public abstract void RemoveEffect(GameObject obj);
    public abstract void ChangeMagnitude(bool increment);
}
