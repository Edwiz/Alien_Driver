using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalVehicles : MonoBehaviour
{
    public Transform leftPoint;
    public Transform rightPoint;
    public float moveSpeed;
    public bool movingRight;
    public bool moveHorizontal;

    private Rigidbody2D myRigidbody;
    private Animator theAnimator;


    // Use this for initialization
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        theAnimator = GetComponent<Animator>();
        movingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        MovingVehicleHorizontaly();
    }

    void MovingVehicleHorizontaly()
    {
        if (moveHorizontal)
        {
            if (movingRight && transform.position.x > rightPoint.position.x)
            {
                movingRight = false;
                theAnimator.SetBool("movingLeft", true);
            }

            if (!movingRight && transform.position.x < leftPoint.position.x)
            {
                movingRight = true;
                theAnimator.SetBool("movingLeft", false);
            }

            if (movingRight)
            {
                myRigidbody.velocity = new Vector3(moveSpeed, myRigidbody.velocity.y, 0f);
            }
            else
            {
                myRigidbody.velocity = new Vector3(-moveSpeed, myRigidbody.velocity.y, 0f);
            }
        }
        else
        {
            myRigidbody.velocity = new Vector3(0f, 0f, 0f); 
        }
    }
}
