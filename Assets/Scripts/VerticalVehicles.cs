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
    public float timeDamaged;
    private float damageCounter;
    [SerializeField]
    private SpriteRenderer thePlayerSprite;

    private Rigidbody2D myRigidbody;
    private Animator theAnimator;


    // Use this for initialization
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        theAnimator = GetComponent<Animator>();
        movingUp = true;

        thePlayerSprite = FindObjectOfType<PlayerController>().GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        MovingVehicleVerticaly();

        if (damageCounter > 0)
        {
            damageCounter -= Time.deltaTime;
        }
        else
        {
            thePlayerSprite.color = new Color(1, 1, 1, 1);
        }
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

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "PlayerGO")
        {
            GameManager.instance.MoodDamaged(1);
            thePlayerSprite.color = new Color(1, 0, 0, 1);
            damageCounter = timeDamaged;
        }
    }

}
