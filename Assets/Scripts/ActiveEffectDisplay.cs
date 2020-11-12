using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveEffectDisplay : MonoBehaviour
{
    public static ActiveEffectDisplay instance;

    public Text forceFieldText;
    public Text orbitalFieldText;
    public Text gravityText;

    public PhysicsEffect effect;
    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            //Destroy(this);
            return;
        }
    }

    public void StoreEffectText(PhysicsEffect effect)
    {
        this.effect = effect;
    }

    public void DisplayEffectText(List<PhysicsEffect> effectsList)
    {
        ClearEffectDisplay();
        foreach(PhysicsEffect physicsEffect in effectsList)
        {
            if (physicsEffect is ForceField)
            {
                forceFieldText.text = "1 " + physicsEffect.effectName;
            }
            if (physicsEffect is OrbitalField)
            {
                orbitalFieldText.text = "2 " + physicsEffect.effectName;
            }
            else if (physicsEffect is RelativeGravityChange)
            {
                //Debug.Log(physicsEffect);
                gravityText.text = "3 " + physicsEffect.effectName;
            }
        }
    }

    public void ClearEffectDisplay()
    {
        forceFieldText.text = "1 ";
        orbitalFieldText.text = "2 ";
        gravityText.text = "3 ";
    }
}
