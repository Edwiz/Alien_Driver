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
        movingRight = true;

        thePlayerSprite = FindObjectOfType<PlayerController>().GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        MovingVehicleHorizontaly();

        if (damageCounter > 0)
        {
            damageCounter -= Time.deltaTime;
        }
        else
        {
            thePlayerSprite.color = new Color(1, 1, 1, 1);
        }
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
