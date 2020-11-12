using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactable
{
    //by default, ALL methods in interfaces must be public abstract, so we don't write it out
    void Highlight();
    void Unhighlight();
}
