using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    public Renderer[] renderers;
    public Material validPlacementMaterial;
    public Material invalidPlacementMaterial;

    public bool isValidPlacement { get; private set; } = true;
    private int numCollisions = 0;

    private void Start()
    {
        setValidMove();
    }

    private void OnTriggerEnter(Collider other)
    {
        numCollisions++;
        if (isValidPlacement)
        {
            isValidPlacement = false;
            setInvalidMove();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        numCollisions--;
        if(numCollisions == 0)
        {
            isValidPlacement = true;
            setValidMove();
        }
    }

    private void setValidMove()
    {
        for(int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = validPlacementMaterial;
        }
    }

    private void setInvalidMove()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = invalidPlacementMaterial;
        }
    }
}
