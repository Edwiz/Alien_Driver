using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    #region Fields

    public static PickUpSystem instance;

    [Range(0,5)]
    public float timeWaitingForPickingUp = 2.5f;
    public bool insideArea;

    [Header("Private Variables")]
    //Cached Variables
    [SerializeField]
    private GameObject m_delivery;
    [SerializeField]
    private Transform m_deliveryPoint;

    //Private Variables
    private float timeCounter;

    //Private Cached Variables
    private Animator theAnimator;

    #endregion

    public GameObject theCurrentDeliveryPoint;

    public void Start()
    {
        instance = this;
        theAnimator = GetComponent<Animator>();
        m_delivery = GameManager.instance.deliverGO;
        m_deliveryPoint = GameManager.instance.deliverPoint;
    }

    public void Update()
    {
        PickingUp();

        if (timeCounter > 0 && insideArea == true)
        {
            timeCounter -= Time.deltaTime;
        }

        if (timeCounter < 0 && insideArea == true)
        {
            Debug.Log("Item Picked");
            GameManager.instance.theItemUI.gameObject.SetActive(true);
            //Instantiate the deliver point
            theCurrentDeliveryPoint = Instantiate(m_delivery, m_deliveryPoint.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }

    public void PickingUp()
    {
        if (!insideArea)
        {
            theAnimator.SetBool("inArea", false);
            timeCounter = timeWaitingForPickingUp;
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
}