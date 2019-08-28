using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    #region GeneralFields
    public static GameManager instance;

    [HideInInspector]
    public bool dayIsFinished;
    [HideInInspector]
    public Transform theStartingPoint;

    [Header("Indication Points")]

    //Cached Variables
    [Tooltip("Place the prefab for indicating the picking point")]
    public GameObject pickUpGO;
    [Tooltip("Place the prefab for indicating the delivering point")]
    public GameObject deliverGO;

    [Header("UI")]
    public GameObject dayIntroGO;
    public Text dayIntroText;
    public Image theItemUI;

    //Variables that need to be public for another script to access them
    [HideInInspector]
    public int currentTaskIndex;
    [HideInInspector]
    public int currentDayIndex;
    [HideInInspector]
    public bool taskFinished;
    [HideInInspector]
    public Transform pickUpPoint;
    [HideInInspector]
    public Transform deliverPoint;
    [HideInInspector]
    public bool itemDelivered;
    [HideInInspector]
    public GameObject theCurrentPickUpPoint;

    //Private Variables
    private Task currentTask;
    private Day currentDay;
    private bool readyToStartNewDay;
    #endregion

    #region Resume_Panel

    [Header("Resume Panel Configuration")]

    public GameObject resumePanel;

    public Image theResumeCharacter;
    public Text theResumeTitle;
    public Text theResumeCompletedTasksText;
    public Text theResumeDayText;

    [HideInInspector]
    public bool resumePanelisActive;
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
    public Text theCharacterName;

    [HideInInspector]
    public bool taskPanelActive;
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

    #region Camera
    [Header("Camera Configuration")]
    public int cameraStartingSize;
    public int cameraMinSize;
    public int cameraMaxSize;
    [HideInInspector]
    public int currentCameraSize;

    #endregion

    #region PlayerMood

    public int currentMood;
    public int maxMood;
    public int minMood;

    #endregion

    [Header("Days Configuration")]
    public Day[] days;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        currentDayIndex = 0;
        StartNewDay(currentDayIndex);
        theItemUI.gameObject.SetActive(false);

        //Camera settings
        currentCameraSize = cameraStartingSize;
    }

    // Update is called once per frame
    void Update()
    {
        Chronometer();

        if (taskFinished == true)
        {
            taskFinished = false;
            currentTaskIndex++;

            if (currentTaskIndex >= currentDay.tasks.Length)
            {
                timerON = false;
                PlayerController.instance.canMove = false;
                StartCoroutine(DisableChronometer());
                StartCoroutine(FinishingDay());
            }
            else
            {
                StartNewTask(currentTaskIndex);
            }
        }

        if (dayIsFinished == true)
        {
            dayIsFinished = false;
            resumePanel.SetActive(true);
            currentDayIndex++;

            if (currentDayIndex >= days.Length)
            {
                Debug.Log("Game Finished");
                //Ending Sequence
            }
            else
            {
                StartCoroutine(TimeForStartingNewDay());
            }
        }

        if (taskPanelActive == true)
        {
            taskPanel.SetActive(true);
            timerText.gameObject.SetActive(false);

            if (Input.GetKeyDown(KeyCode.X))
            {
                timerON = true;
                theCurrentPickUpPoint = Instantiate(pickUpGO, pickUpPoint.position, Quaternion.identity);
                taskPanelActive = false;
                dayIntroGO.SetActive(false);
                PlayerController.instance.canMove = true;
            }
        }
        else
        {
            taskPanel.SetActive(false);
        }

        CameraSize();

        //Camera increasing
        if (itemDelivered == true)
        {
            itemDelivered = false;

            if (currentCameraSize < cameraMaxSize)
            {
                currentCameraSize++;
            }
            else
            {
                currentCameraSize = cameraMaxSize;
            }
        }
    }

    public void StartNewDay(int index)
    {
        //Beggining Day
        dayIntroText.text = "Día " + (currentDayIndex + 1).ToString();
        dayIntroGO.SetActive(true);

        currentTaskIndex = 0;
        resumePanel.SetActive(false);
        currentDay = days[index];
        theStartingPoint = currentDay.newStartingPoint;

        //Day Ending Panel
        theResumeCharacter.sprite = currentDay.newResumeCharacter;
        theResumeTitle.text = "Resumen del día "+ (currentDayIndex + 1).ToString();
        theResumeDayText.text = currentDay.newDayResumeText;

        StartCoroutine(DelayForStartingFirstTask());
    }

    public void StartNewTask(int index)
    {
        taskPanelActive = true;
        currentTask = currentDay.tasks[index];
        timeForTask = currentTask.newTimeForTask;

        //Asigning the value to the current pickup point from the pickup values of the Task sub class
        pickUpPoint = currentTask.newPickUpPoint;

        //Asinging the values of the TaskPanel
        theCharacter.sprite = currentTask.newCharacter;
        thePickingBuiling.sprite = currentTask.newPickingBuilding;
        theDeliveringBuilding.sprite = currentTask.newDeliveringBuilding;
        theItem.sprite = currentTask.newItem;
        theTaskText.text = currentTask.newTaskText;
        theCharacterName.text = currentTask.newCharacterName;
        //theTime.text = currentTask.newTimeForTask.ToString(); //Se van a mostrar los segundo porque no hay formato
        int minutes = Mathf.FloorToInt(timeForTask / 60F);
        int seconds = Mathf.FloorToInt(timeForTask - minutes * 60);
        timeCounter = string.Format("{0:00}:{1:00}", minutes, seconds);
        theTime.text = timeCounter.ToString();
        theTaskNumber.text = "Pedido #" + (currentTaskIndex + 1).ToString();

        //Asigning the value to the current deliver point from the deliver values of the Task sub class
        deliverPoint = currentTask.newDeliverPoint;

        //Asigning UI elements
        theItemUI.sprite = currentTask.newItem;
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

            if (timeForTask > 1 && !taskPanelActive)
            {
                timeForTask -= Time.deltaTime;
            }
            else//When the Timer reaches to CERO
            {
                //Your mood lowers by 1
                Debug.Log("Pedido no entregado.");
                taskFinished = true;
                timerON = false;

                //Camera Zoom Out
                if (currentCameraSize > cameraMinSize)
                {
                    currentCameraSize--;
                }
                else
                {
                    currentCameraSize = cameraMinSize;
                }

                PlayerController.instance.canMove = false;
                var StopVelocity = PlayerController.instance.theRigidBody.velocity = new Vector2(0f, 0f);
                PlayerController.instance.myAnim.SetFloat("moveX", StopVelocity.x);
                PlayerController.instance.myAnim.SetFloat("moveY", StopVelocity.y);

                StartCoroutine(DisableChronometer());

                //Disable current pick and deliver points
                Destroy(theCurrentPickUpPoint);
                Destroy(PickUpSystem.instance.theCurrentDeliveryPoint);
                theItemUI.gameObject.SetActive(false);
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

    void CameraSize()
    {
        var camera = Camera.main;
        var brain = (camera == null) ? null : camera.GetComponent<CinemachineBrain>();
        var vcam = (brain == null) ? null : brain.ActiveVirtualCamera as CinemachineVirtualCamera;
        if (vcam != null)
        {
            vcam.m_Lens.OrthographicSize = currentCameraSize;
        }
    }

    IEnumerator DisableChronometer()
    {
        yield return new WaitForSeconds(2f);
        timerText.gameObject.SetActive(false);
    }
    #endregion

    IEnumerator FinishingDay()
    {
        yield return new WaitForSeconds(2f);
        dayIsFinished = true;
    }

    IEnumerator TimeForStartingNewDay()
    {
        yield return new WaitForSeconds(5);
        {
            StartNewDay(currentDayIndex);
        }
    }

    IEnumerator DelayForStartingFirstTask()
    {
        yield return new WaitForSeconds(2);
        StartNewTask(currentTaskIndex);
    }
}


[System.Serializable]
public class Day
{
    [Header("Day number")]
    public string dayNumber;
    public Sprite newResumeCharacter;
    public Transform newStartingPoint;
    [TextArea]
    public string newDayResumeText;

    public Task[] tasks;
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
    public string newCharacterName;
    [TextArea]
    public string newTaskText;

    //A variable that stores the container wich the virtual camera uses.
    //An array of Transforms where there are going to be obstacles
    //An array of obstacles

}
