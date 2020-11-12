using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    public Sprite inventoryIcon;
    public string interactionText;
    public Placeable placeable;
    private GameObject obj;

    private void Start()
    {
        obj = this.gameObject;
    }
}
