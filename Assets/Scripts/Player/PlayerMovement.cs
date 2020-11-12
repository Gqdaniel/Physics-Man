using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("==MOVEMENT==")]
    [SerializeField]
    private float walkSpeed = 5f;
    [SerializeField]
    private float runSpeed = 10f;

    [Header("==JUMPING==")]
    [SerializeField]
    private float jumpSpeed = 10f;
    [SerializeField]
    private float groundCheckDistance = 1f;
    [SerializeField]
    private float groundCheckRadius = 1f;
    [SerializeField]
    private LayerMask groundCheckMask;

    [Header("==CAMERA==")]
    [SerializeField]
    private Camera playerCamera;
    [SerializeField]
    private MouseLook mouseLook;

    private Transform thisTransform;
    private Rigidbody rb;
    private Vector3 moveDir;
    private bool hasJumped = false;
    private bool isGrounded = true;

    private void Start()
    {
        thisTransform = this.transform;
        rb = GetComponent<Rigidbody>();
        mouseLook.Init(transform, playerCamera.transform);
    }

    private void Update()
    {
        mouseLook.LookRotation(thisTransform, playerCamera.transform);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float y = 0f;
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            hasJumped = true;
            y = jumpSpeed;
        }
        
        moveDir = new Vector3(x, y, z);
    }

    private void FixedUpdate()
    {
        Vector3 forwardDir = thisTransform.forward * moveDir.z;
        Vector3 strafeDir = thisTransform.right * moveDir.x;
        Vector3 finalDir = (forwardDir + strafeDir) * walkSpeed;

        if(rb.velocity.magnitude < walkSpeed)
        {
            rb.AddForce(finalDir, ForceMode.VelocityChange);
        }
        
        if(rb.velocity.magnitude > walkSpeed)
        {
            rb.velocity = new Vector3(finalDir.x, rb.velocity.y, finalDir.z);
        }

        if(hasJumped)
        {
            Vector3 jumpDir = moveDir.y * thisTransform.up;
            rb.AddForce(jumpDir, ForceMode.VelocityChange);
            hasJumped = false;
            isGrounded = false;
            Debug.Log("Jumped!");
        }

        CheckGrounded();
    }

    private void CheckGrounded()
    {
        RaycastHit hit;
        if(Physics.SphereCast(thisTransform.position, groundCheckRadius, -thisTransform.up, out hit, groundCheckDistance - groundCheckRadius, groundCheckMask, QueryTriggerInteraction.Ignore))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position + (-this.transform.up * (groundCheckDistance - groundCheckRadius)), groundCheckRadius);
    }
}
