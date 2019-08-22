using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region GeneralFields
    public static GameManager instance;

    [Header("Indication Points")]
    //Cached Variables
    public GameObject pickUpGO;
    public GameObject deliverGO;

    //Variables that need to be public for another script to access them
    [HideInInspector]
    public int currentTaskIndex;
    [HideInInspector]
    public bool taskFinished;
    [HideInInspector]
    public Transform pickUpPoint;
    [HideInInspector]
    public Transform deliverPoint;

    //Private Variables
    private Task currentTask;
    #endregion

    #region TaskPanel
    [Header("Task Panel Configuration")]

    public GameObject taskPanel;
    public Image theCharacter;
    public Image thePickingBuiling;
    public Image theDeliveringBuilding;
    public Image theItem;
    public Text theTime;
    public Text theTaskText;
    public Text theTaskNumber;

    [HideInInspector]
    public bool panelActive;
    #endregion

    #region Timer
    [Header("Timer Configuration")]

    //Cached variables
    public Text timerText;

    [HideInInspector]
    public bool timerON;

    //Private variables
    private float timeForTask;
    private string timeCounter;
    #endregion

    [Header("Tasks Configuration")]
    public Task[] tasks;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        currentTaskIndex = 0;
        StartNewTask(currentTaskIndex);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(currentTaskIndex);

        Chronometer();

        if (taskFinished == true)
        {
            taskFinished = false;
            currentTaskIndex++;

            if (currentTaskIndex >= tasks.Length)
            {
                Debug.Log("Game Finished");
                timerON = false;
                PlayerController.instance.canMove = false;
                StartCoroutine(DisableChronometer());
            }
            else
            {
                StartNewTask(currentTaskIndex);
            }
        }

        if (panelActive == true)
        {
            taskPanel.SetActive(true);
            timerText.gameObject.SetActive(false);

            if (Input.GetKeyDown(KeyCode.X))
            {
                timerON = true;
                Instantiate(pickUpGO, pickUpPoint.position, Quaternion.identity);
                panelActive = false;
                PlayerController.instance.canMove = true;
            }
        }
        else
        {
            taskPanel.SetActive(false);
        }
    }

    public void StartNewTask(int index)
    {
        panelActive = true;
        currentTask = tasks[index];
        timeForTask = currentTask.newTimeForTask;

        //Asigning the value to the current pickup point from the pickup values of the Task sub class
        pickUpPoint = currentTask.newPickUpPoint;

        //Asinging the values of the TaskPanel
        theCharacter.sprite = currentTask.newCharacter;
        thePickingBuiling.sprite = currentTask.newPickingBuilding;
        theDeliveringBuilding.sprite = currentTask.newDeliveringBuilding;
        theItem.sprite = currentTask.newItem;
        theTaskText.text = currentTask.newTaskText;
        //theTime.text = currentTask.newTimeForTask.ToString(); //Se van a mostrar los segundo porque no hay formato
        int minutes = Mathf.FloorToInt(timeForTask / 60F);
        int seconds = Mathf.FloorToInt(timeForTask - minutes * 60);
        timeCounter = string.Format("{0:00}:{1:00}", minutes, seconds);
        theTime.text = timeCounter.ToString();
        theTaskNumber.text = "Pedido #" + (currentTaskIndex + 1).ToString();

        //Asigning the value to the current deliver point from the deliver values of the Task sub class
        deliverPoint = currentTask.newDeliverPoint;
    }

    #region TimerMethod
    public void Chronometer()
    {
        if (timerON)
        {
            timerText.gameObject.SetActive(true);

            int minutes = Mathf.FloorToInt(timeForTask / 60F);
            int seconds = Mathf.FloorToInt(timeForTask - minutes * 60);
            timeCounter = string.Format("{0:00}:{1:00}", minutes, seconds);
            timerText.text = timeCounter.ToString();

            if (timeForTask > 1 && !panelActive)
            {
                timeForTask -= Time.deltaTime;
            }
            else
            {
                timerON = false;
                StartCoroutine(DisableChronometer());
            }

            //Change Timer color to Red
            if (timeForTask > 0 && timeForTask < 11)
            {
                timerText.color = new Color(1, 0, 0, 1);
            }
            else
            {
                timerText.color = new Color(1, 1, 1, 1);
            }
        }
    }

    IEnumerator DisableChronometer()
    {
        yield return new WaitForSeconds(2f);
        timerText.gameObject.SetActive(false);
    }
    #endregion
}

[System.Serializable]
public class Task
{
    [Header("Task Number")]
    public string taskNumber;

    [Header("Configuration")]
    public float newTimeForTask;
    public Transform newPickUpPoint;
    public Transform newDeliverPoint;

    [Header("Task Panel")]
    public Sprite newCharacter;
    public Sprite newPickingBuilding;
    public Sprite newDeliveringBuilding;
    public Sprite newItem;
    public string newTaskText;
}
