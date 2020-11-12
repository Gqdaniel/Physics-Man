using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public Image inventorySlot;

    public Pickupable currentlyHeldObj { get; private set; }

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

    public void setHeldObject(Pickupable pickedUpObj)
    {
        currentlyHeldObj = pickedUpObj;
        currentlyHeldObj.gameObject.SetActive(false);

        inventorySlot.sprite = currentlyHeldObj.inventoryIcon;
    }

    public Placeable placeObject()
    {
        Placeable placeable = currentlyHeldObj.placeable;
        currentlyHeldObj = null;
        return placeable;
    }

    public bool isCurrentlyHoldingObj()
    {
        return currentlyHeldObj != null;
    }
}
