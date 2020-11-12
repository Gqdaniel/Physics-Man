using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVector : MonoBehaviour
{
    public float forceIncrement = 0.1f;
    
    private Rigidbody rb;
    private CharacterController charController;
    private Vector3 currentForce = Vector3.zero;
    //private float currentScoreRatio = 0f;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        charController = GetComponent<CharacterController>();
    }

    //// Update is called once per frame
    //private void Update()
    //{
    //    //Vector3 pVector = Vector3.zero;
    //    if (Input.GetButtonDown("Fire1"))
    //    {
    //        PlayerLaunch();
    //    }
    //}

    private void FixedUpdate()
    {
        if(currentForce.magnitude >= forceIncrement)
        {
            Vector3 increment = currentForce.normalized * forceIncrement;
            charController.Move(increment);
            currentForce -= increment;
            //Score.instance.increaseScore(currentScoreRatio);

            if(currentForce.magnitude < forceIncrement)
            {
                //Debug.Log("Out of force!");
                currentForce = Vector3.zero;
            }

            if(charController.isGrounded)
            {
                //Debug.Log("Grounded!");
                currentForce = Vector3.zero;
                return;
            }
        }
    }

    public void PlayerLaunch(VectorForce block)
    {
        //rb.AddForce(Vector3.up * force, ForceMode.Impulse);
        currentForce += block.getDirectionVector();
        //currentScoreRatio = block.use();
    }

    //Detect what's in front of player
        //raycast from center of screen,
        //if there is an object
            //get the collider
        //otherwise
            //do nothing
   //If there was something in front of us, get the direction it's facing
   //Send this info over to the player, and apply force in that direction
}
