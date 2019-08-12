using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public Rigidbody2D theRigidBody;
    public float moveSpeed;

    //public Animator myAnim;

    public bool isMovingLeft, isMovingRight, isMovingUp, isMovingDown;

    public bool canMove;

    // Use this for initialization
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        /*
        //Clamp to horizontal axis avoiding diagonal movement
        if (isMovingRight || isMovingLeft)
        {
            theRigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), 0f) * moveSpeed;
        }
        //Clamp to vertical axis avoiding diagonal movement
        if (isMovingUp || isMovingDown)
        {
            theRigidBody.velocity = new Vector2(0f, Input.GetAxisRaw("Vertical")) * moveSpeed;
        }
        */

        theRigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))* moveSpeed;

        //Passing value to animation parameters for movement direction
        /*myAnim.SetFloat("moveX", theRigidBody.velocity.x);
        myAnim.SetFloat("moveY", theRigidBody.velocity.y);

        //Passing value to animation parameters for Idle animation
        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            myAnim.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
            myAnim.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
        }*/

        /*
        //Conditions for preventing diagonal movement
        if (Input.GetAxisRaw("Horizontal") == 1)
        {
            isMovingRight = true;
            isMovingLeft = false;
            isMovingUp = false;
            isMovingDown = false;
        }

        if (Input.GetAxisRaw("Horizontal") == -1)
        {
            isMovingRight = false;
            isMovingLeft = true;
            isMovingUp = false;
            isMovingDown = false;
        }

        if (Input.GetAxisRaw("Vertical") == 1)
        {
            isMovingRight = false;
            isMovingLeft = false;
            isMovingUp = true;
            isMovingDown = false;
        }

        if (Input.GetAxisRaw("Vertical") == -1)
        {
            isMovingRight = false;
            isMovingLeft = false;
            isMovingUp = false;
            isMovingDown = true;
        }
        */
    }
}
