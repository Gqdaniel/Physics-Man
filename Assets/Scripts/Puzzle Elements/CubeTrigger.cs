using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTrigger : MonoBehaviour
{
    //public SignalReceiver signalReceiver;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PhysicsObject"))
        {
            //signalReceiver.Signal(other);
            Debug.Log("Collision");
        }
    }
}
