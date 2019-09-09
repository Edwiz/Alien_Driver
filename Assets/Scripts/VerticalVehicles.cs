using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalVehicles : MonoBehaviour
{
    //public static VerticalVehicles instance;

    public Transform upPoint;
    public Transform downPoint;
    public float moveSpeed;
    public bool movingUp;
    public bool canMove;

    private Rigidbody2D myRigidbody;
    private Animator theAnimator;


    // Use this for initialization
    void Start()
    {
        //instance = this;

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
        if (canMove)
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

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "PlayerGO")
        {
            GameManager.instance.MoodDamaged(1);
            GameManager.instance.arrowDownUI.SetActive(true);
            StartCoroutine(TurningOFFArrowVertical());
        }
    }

    public IEnumerator TurningOFFArrowVertical()
    {
        yield return new WaitForSeconds(1);
        GameManager.instance.arrowDownUI.SetActive(false);
    }
}
