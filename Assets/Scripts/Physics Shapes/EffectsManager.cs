using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectsManager : MonoBehaviour, Interactable
{
    public Material regMaterial;
    public Material highlightedMaterial;

    private Renderer rend;

    private class TrackedEffect
    {
        public PhysicsEffect effect { get; private set; }
        public RaycastHit hitInfo { get; private set; }


        public TrackedEffect(PhysicsEffect effect, RaycastHit hitInfo)
        {
            this.effect = effect;
            this.hitInfo = hitInfo;
        }
    }

    private Dictionary<PhysicsEffect, TrackedEffect> currentEffects;

    private void Start()
    {
        currentEffects = new Dictionary<PhysicsEffect, TrackedEffect>();
        rend = GetComponent<Renderer>();
        regMaterial = rend.material;
    }

    public void AddEffect(RaycastHit hitInfo, PhysicsEffect effect)
    {
        //TODO prevents exact same effect from being added, but later we should make it so
        //if an updated version of an effect type is added, the original is removed etc.

        //Debug.Log("Effect: " + effect);
        //Does not work for Fields
        if(!currentEffects.ContainsKey(effect))
        {
            if(!effect.isOneShotEffect)
            {
                currentEffects.Add(effect, new TrackedEffect(effect, hitInfo));
                effect.AddEffectedObj(this);
            }
            effect.ApplyEffect(hitInfo);
            
        }
    }

    // Add effect to player
    public void AddEffect(RaycastHit hitInfo, PhysicsEffect effect, GameObject player)
    {
        //Debug.Log("Effect: " + effect);
        //Does not work for Fields
        if (!currentEffects.ContainsKey(effect))
        {
            if (!effect.isOneShotEffect)
            {
                currentEffects.Add(effect, new TrackedEffect(effect, hitInfo));
                effect.AddEffectedObj(this);
            }
            effect.ApplyEffect(hitInfo, player);

        }
    }

        public void UpdateMagnitude(PhysicsEffect effect)
    {
        if(currentEffects.ContainsKey(effect))
        {
            TrackedEffect effectInfo = currentEffects[effect];
            effect.ApplyEffect(effectInfo.hitInfo); // does not work for field shapes
        }
    }

    public void RemoveEffect(PhysicsEffect effect)
    {
        if(currentEffects.ContainsKey(effect))
        {
            effect.RemoveEffectedObj(this);
            currentEffects.Remove(effect);
        }
    }

    public void RemoveAllEffects()
    {
        foreach(KeyValuePair<PhysicsEffect, TrackedEffect> pair in currentEffects)
        {
            pair.Key.RemoveEffectedObj(this);
        }
        currentEffects.Clear();
    }

    public List<PhysicsEffect> GetActiveEffects()
    {
        return currentEffects.Keys.ToList();
    }

    public void Highlight()
    {
        rend.material = highlightedMaterial;
    }

    public void Unhighlight()
    {
        rend.material = regMaterial;
    }
}
