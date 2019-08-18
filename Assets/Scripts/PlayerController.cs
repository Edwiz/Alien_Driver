using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Fields
    public static PlayerController instance;

    [Header("Player Configuration")]
    [Range(0, 10)]
    public float normalMoveSpeed;
    [Range(0, 10)]
    public float diagonalMoveSpeed;
    [Range(0, 10)]
    public float acceleration;
    [Range(1, 10)]
    public float brakeTime;

    [Header("Components")]
    //Cached Variables
    public Rigidbody2D theRigidBody;
    public Animator myAnim;

    //Private Variables
    [SerializeField]
    private bool canMove;
    private float speedIncrement;
    private float initSpeed;
    private float xInput;
    private float yInput;
    private bool isMovingLeft, isMovingRight, isMovingUp, isMovingDown;
    #endregion

    // Use this for initialization
    void Start()
    {
        instance = this;
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(theRigidBody.velocity);

        if (speedIncrement > 0 && (Input.GetAxis("Acceleration") == 0 || Input.GetButtonUp("Fire1")))
        {
            speedIncrement -= Time.deltaTime * brakeTime;
        }
    }

    void PlayerMovement()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        if (Input.GetAxisRaw("Acceleration") == 1 || Input.GetButton("Fire1"))
        {
            speedIncrement = acceleration;
        }

        // The actual code for moving

        if (isMovingLeft || isMovingRight)
        {
            theRigidBody.velocity = new Vector2(xInput, (yInput * diagonalMoveSpeed)) * (normalMoveSpeed + speedIncrement);
        }

        if (isMovingUp || isMovingDown)
        {
            theRigidBody.velocity = new Vector2((xInput * diagonalMoveSpeed), yInput) * (normalMoveSpeed + speedIncrement);
        }

        //Passing value to animation parameters for movement direction
        myAnim.SetFloat("moveX", theRigidBody.velocity.x);
        myAnim.SetFloat("moveY", theRigidBody.velocity.y);

        //Passing value to animation parameters for Idle animation
        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            myAnim.SetFloat("lastMoveX", Input.GetAxis("Horizontal"));
            myAnim.SetFloat("lastMoveY", Input.GetAxis("Vertical"));
        }

        //Conditions for preventing diagonal movement
        if (Input.GetAxisRaw("Horizontal") == 1)
        {
            isMovingRight = true;
            isMovingLeft = false;
            isMovingUp = false;
            isMovingDown = false;
            //YspeedDefault = Yspeed
            //YspeedDefault = 0f 
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
    }
}

//_____________________________________________________________________________________

//Input without diagonal movement
/*
//Right Movement
if (xInput == 1 && yInput == 0)
{
    theRigidBody.velocity = new Vector2(initSpeed + speedIncrement, 0f);
}
//Left Movement
if (xInput == -1 && yInput == 0)
{
    theRigidBody.velocity = new Vector2(-initSpeed - speedIncrement, 0f);
}
//Up
if (xInput == 0 && yInput == 1)
{
    theRigidBody.velocity = new Vector2(0f, initSpeed + speedIncrement);
}
//Down Movement
if (xInput == 0 && yInput == -1)
{
    theRigidBody.velocity = new Vector2(0f, -initSpeed - speedIncrement);
}
*/

//CODE FOR DIAGONAL SPEED
//Horizontal Movement
/*if (isMovingRight || isMovingLeft)
{
    theRigidBody.velocity = new Vector2(xInput * normalMoveSpeed, yInput * diagonalMoveSpeed);
}

//Vertical Movement
if (isMovingUp || isMovingDown)
{
    theRigidBody.velocity = new Vector2(xInput * diagonalMoveSpeed, yInput * normalMoveSpeed);
}*/

//CODE FOR NOT MOVING IN DIAGONAL

/*
//Clamp to horizontal axis avoiding diagonal movement
if (isMovingRight || isMovingLeft)
{
    theRigidBody.velocity = new Vector2(xInput, 0) * normalMoveSpeed;
}

//Clamp to vertical axis avoiding diagonal movement
if (isMovingUp || isMovingDown)
{
    theRigidBody.velocity = new Vector2(0, yInput) * normalMoveSpeed;
}
*/
