using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float speed = 2f;

    private Transform thisTransform;

    private void Start()
    {
        //transform == GetComponent<Transform>()
        thisTransform = this.transform;
    }

    private void Update()
    {
        //thisTransform.position = new Vector3(thisTransform.position.x, thisTransform.position.y, thisTransform.position.z + speed * Time.deltaTime);
        //thisTransform.position = thisTransform.position + new Vector3(0f, 0f, speed * Time.deltaTime);
        //thisTransform.position += new Vector3(0f, 0f, speed * Time.deltaTime);
        thisTransform.position += Vector3.forward * speed * Time.deltaTime;
    }
}
