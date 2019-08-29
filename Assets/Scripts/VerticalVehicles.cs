using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalVehicles : MonoBehaviour
{
    public Transform upPoint;
    public Transform downPoint;
    public float moveSpeed;
    public bool movingUp;
    public bool moveVerticaly;

    private Rigidbody2D myRigidbody;
    private Animator theAnimator;


    // Use this for initialization
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        theAnimator = GetComponent<Animator>();
        movingUp = true;
    }

    // Update is called once per frame
    void Update()
    {
        MovingVehicleVerticaly();
    }

    void MovingVehicleVerticaly()
    {
        if (moveVerticaly)
        {
            if (movingUp && transform.position.y > upPoint.position.y)
            {
                movingUp = false;
                theAnimator.SetBool("movingDown", true);
            }

            if (!movingUp && transform.position.y < downPoint.position.y)
            {
                movingUp = true;
                theAnimator.SetBool("movingDown", false);
            }

            if (movingUp)
            {
                myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, moveSpeed, 0f);
            }
            else
            {
                myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, -moveSpeed, 0f);
            }
        }
        else
        {
            myRigidbody.velocity = new Vector3(0f, 0f, 0f);
        }
    }

}
