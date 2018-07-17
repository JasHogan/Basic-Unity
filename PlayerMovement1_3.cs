using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement1_3 : MonoBehaviour
{
    public float speed = 2f;
    public float speedAir = 1f;
    public float jumpPower = 1f;

    Vector3 jumping;
    Quaternion rotate;
    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidbody;
    int floorMask;
    float camRayLength = 100f;
    //bool canDoubleJump;
    bool jumpRequest;
    float jumpTimes;
    bool playerOnGround;

    private void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        rotate = Quaternion.Euler(0, Camera.main.transform.rotation.y, 0);  
    }
    private void Update()
    {
        Jump();
    }
    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        float j = Input.GetAxis("Jump");

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 1);
        }
        
        Move(h, v);
        //Turning();
        //Animating(h, v);          
    }

  //detect if player is touching "Ground"
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            playerOnGround = true;
            //Debug.Log("On Ground");
        }
    }

    //detect if player is in air
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            playerOnGround = false;
        }
    }   

    void Jump()
    {      

        if (Input.GetButtonDown("Jump"))
        {           
            jumpRequest = true;
            jumpTimes += 1;
            //if jump btn hit more than 2 times player cannot jump (fix: if player hits jump btn in air >= 3 times, extra jump happens when player hits ground)
            if (jumpTimes >= 2)
            {
                jumpRequest = false;
            } 
            //Debug.Log("space hit: " + jumpTimes);
        }

        //if player in contact with ground reset jumpTimes to 0
        if (playerOnGround)
        {
            jumpTimes = 0;           
        }
  
        if (jumpRequest && jumpTimes <= 2)
        {
            //try to get player y velocity to 0 so they can have same power on second jump.
            playerRigidbody.velocity = Vector3.zero;
            //Debug.Log("fall speed: " + playerRigidbody.velocity.y);
            playerRigidbody.AddForce(Vector3.up * jumpPower);
            jumpRequest = false;
            // Debug.Log("jumping"); */

            Debug.Log("jumpTimes: " + jumpTimes);
            Debug.Log("playerOnGround: " + playerOnGround);
        }
        
    }

    private void Move(float h, float v)
    {
        movement.Set(h, 0f, v);

        movement = movement.normalized * speed * Time.deltaTime;


        playerRigidbody.MovePosition(transform.position + movement);
    }

    /*void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(gameObject.transform.position);

        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            Vector3 playerToCam = floorHit.point;

            playerToCam.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToCam);
            playerRigidbody.MoveRotation(newRotation);
        }
    } */

    /*void Animating(float h, float v)
    {
        bool walking = h != 0f || v != 0f;
        anim.SetBool("IsWalking", walking);
    }*/
}
