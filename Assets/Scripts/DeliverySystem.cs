using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverySystem : MonoBehaviour
{
    #region Fields

    public static DeliverySystem instance;

    [Range(0, 5)]
    public float timeWaiting = 2.5f;
    public bool insideArea;
    public bool itemDelivered;

    //Cached Variables
    private VerticalVehicles[] verticalCars;
    private HorizontalVehicles[] horizontalCars;


    //Private Variables
    private float timeCounter;

    //Private Cached Variables
    private Animator theAnimator;
    private SpriteRenderer deliverSpriteRenderer;

    #endregion

    public void Start()
    {
        instance = this;
        theAnimator = GetComponent<Animator>();
        deliverSpriteRenderer = GetComponentInChildren<SpriteRenderer>();

        horizontalCars = FindObjectsOfType<HorizontalVehicles>();
        verticalCars = FindObjectsOfType<VerticalVehicles>();

    }

    public void Update()
    {
        Delivering();

        if (timeCounter > 0 && insideArea == true)
        {
            timeCounter -= Time.deltaTime;
        }

        if (timeCounter < 0 && insideArea == true && GameManager.instance.itemDelivered == false)
        {
            Debug.Log("Item Delivered");
            GameManager.instance.arrowUpUI.SetActive(true);
            GameManager.instance.theItemUI.gameObject.SetActive(false);

            foreach (HorizontalVehicles horizontaVehics in horizontalCars)
            {
                horizontaVehics.canMove = false;
            }

            foreach (VerticalVehicles verticalVehics in verticalCars)
            {
               verticalVehics.canMove = false;
            }

            //HorizontalVehicles.instance.canMove = false;
            //VerticalVehicles.instance.canMove = false;
            //DuplicateVehicle.instance.canMove = false;

            PlayerController.instance.canMove = false;
            var StopVelocity = PlayerController.instance.theRigidBody.velocity = new Vector2(0f, 0f);
            PlayerController.instance.myAnim.SetFloat("moveX", StopVelocity.x);
            PlayerController.instance.myAnim.SetFloat("moveY", StopVelocity.y);

            deliverSpriteRenderer.color = new Color(1, 1, 1, 0);
            GameManager.instance.timerON = false;
            Debug.Log("Initiate another task");
            StartCoroutine(EndingTask());
        }
    }

    public void Delivering()
    {
        if (!insideArea)
        {
            theAnimator.SetBool("inArea", false);
            timeCounter = timeWaiting;
        }
        else
        {
            theAnimator.SetBool("inArea", true);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            insideArea = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            insideArea = false;
        }
    }

    IEnumerator EndingTask()
    {
        yield return new WaitForSeconds(GameManager.instance.waitForNewTask);
        GameManager.instance.taskFinished = true;
        GameManager.instance.currentMoney += GameManager.instance.moneyToSum;
        GameManager.instance.itemDelivered = true;
        gameObject.SetActive(false);
    }
}
