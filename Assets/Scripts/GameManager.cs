using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    #region Fields

    #region GeneralFields

    public static GameManager instance;

    public float waitForNewTask;

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
    public GameObject arrowDownUI;
    public GameObject arrowUpUI;
    public GameObject smsUI;
    public Text dayIntroText;
    public Image theItemUI;

    //Variables that need to be public for another script to access them
    [HideInInspector]
    public int currentTaskIndex;

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
    public int completedTasks;

    [HideInInspector]
    public bool resumePanelisActive;
    #endregion

    #region TaskPanel
    [Header("Task Panel Configuration")]

    public GameObject taskPanel;
    public Image theCharacter;
    //public Image thePickingBuiling;
    //public Image theDeliveringBuilding;
    public Image theItem;
    public Text theTime;
    public Text theRewardMoney;
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
    public float cameraStartingSize;
    public float cameraMinSize;
    public float cameraMaxSize;
    public float cameraChangeIndex;
    [HideInInspector]
    public float currentCameraSize;
    #endregion

    #region PlayerMood
    [Header("Mood Configuration")]
    public int maxMood;
    public int minMood;
    public Image veryHappy;
    public Image Happy;
    public Image Neutral;
    public Image Sad;
    public Image verySad;
    public Image flare1;
    public Image flare2;
    public Image flare3;
    public Image flare4;
    public Image flare5;

    [HideInInspector]
    public int currentMood;
    #endregion

    #region Money
    [Header("Money")]
    public Text moneyUIDisplay;
    [HideInInspector]
    public int currentMoney;
    [HideInInspector]
    public int moneyToSum;
    #endregion

    [Header("Obstacles")]
    public GameObject obstacle;
    [HideInInspector]
    public int obstacleCount;

    [Header("Vehicles")]
    public GameObject[] theHorizontalCars;
    public GameObject[] theVerticalCars;

    public GameObject[] vehiclesToActivate = new GameObject[5];

    private VerticalVehicles[] verticalVehicles;
    private HorizontalVehicles[] horizontalVehicles;

    [Header("Days Configuration")]
    public Day[] days;

    public GameObject directionArrow;
    private GameObject target;
    private Transform targetPos;
    [HideInInspector]
    public int currentDayIndex;
    [HideInInspector]
    public Day currentDay;

    #endregion

    #region Methods

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        currentDayIndex = 0;
        StartNewDay(currentDayIndex);
        theItemUI.gameObject.SetActive(false);

        currentMood = 3;
        UpdateMood();
        UpdatingVehicles();

        //Camera settings
        currentCameraSize = cameraStartingSize;

        currentMoney = 0;

        completedTasks = 0;

        smsUI.SetActive(true);
        StartCoroutine(TurnOFFSMS());

    }

    // Update is called once per frame
    void Update()
    {
        verticalVehicles = FindObjectsOfType<VerticalVehicles>();
        horizontalVehicles = FindObjectsOfType<HorizontalVehicles>();

        Chronometer();

        moneyUIDisplay.text = "$ " + currentMoney;

        if (taskFinished == true)
        {
            taskFinished = false;
            currentTaskIndex++;
            DestroyObstacles();

            if (currentTaskIndex >= currentDay.tasks.Length)
            {
                timerON = false;
                PlayerController.instance.canMove = false;
                var StopVelocity = PlayerController.instance.theRigidBody.velocity = new Vector2(0f, 0f);
                PlayerController.instance.myAnim.SetFloat("moveX", StopVelocity.x);
                PlayerController.instance.myAnim.SetFloat("moveY", StopVelocity.y);
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
                StartCoroutine(DelayForGameFinishScreen());
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
            UpdatingVehicles();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                timerON = true;
                theCurrentPickUpPoint = Instantiate(pickUpGO, pickUpPoint.position, Quaternion.identity);
                taskPanelActive = false;
                dayIntroGO.SetActive(false);
                PlayerController.instance.canMove = true;
                // HorizontalVehicles.instance.canMove = true;
                foreach (HorizontalVehicles horizontalCar in horizontalVehicles)
                {
                    horizontalCar.canMove = true;
                }

                foreach (VerticalVehicles verticalCar in verticalVehicles)
                {
                    verticalCar.canMove = true;
                }
            }
        }
        else
        {
            taskPanel.SetActive(false);
        }

        CameraSize();

        //Camera increasing and counting deliveries
        if (itemDelivered == true)
        {
            completedTasks++;
            itemDelivered = false;
            theResumeCompletedTasksText.text = "Pedidos Completados: " + completedTasks + " / " + currentDay.tasks.Length;

            if (currentCameraSize < cameraMaxSize)
            {
                currentCameraSize += cameraChangeIndex;
            }
            else
            {
                currentCameraSize = cameraMaxSize;
            }

            MoodIncrement(1);//Im hard coing here I have to specify how much the mood is going to increment

        }
    }

    public void DestroyObstacles()
    {
        GameObject[] obstaclesToDestroy = GameObject.FindGameObjectsWithTag("Obstacle");

        for (int i = 0; i < obstaclesToDestroy.Length; i++)
        {
            Destroy(obstaclesToDestroy[i]);
        }
    }

    public void StartNewDay(int index)
    {
        //Beggining Day
        dayIntroText.text = "Día " + (currentDayIndex + 1).ToString();
        dayIntroGO.SetActive(true);

        currentMood = 3;
        currentTaskIndex = 0;
        resumePanel.SetActive(false);
        currentDay = days[index];
        theStartingPoint = currentDay.newStartingPoint;

        //Day Ending Panel
        theResumeCharacter.sprite = currentDay.newResumeCharacter;
        theResumeTitle.text = "Resumen del día " + (currentDayIndex + 1).ToString();
        theResumeDayText.text = currentDay.newDayResumeText;


        StartCoroutine(DelayForStartingFirstTask());
    }

    public void StartNewTask(int index)
    {
        arrowDownUI.SetActive(false);
        arrowUpUI.SetActive(false);
        taskPanelActive = true;
        currentTask = currentDay.tasks[index];
        timeForTask = currentTask.newTimeForTask;
        obstacleCount = currentTask.newObstacles.Length;

        theRewardMoney.text = "$ " + currentTask.newTaskReward;
        moneyToSum = currentTask.newTaskReward;

        //Asigning the value to the current pickup point from the pickup values of the Task sub class
        pickUpPoint = currentTask.newPickUpPoint;

        //Asinging the values of the TaskPanel
        theCharacter.sprite = currentTask.newCharacter;
        //thePickingBuiling.sprite = currentTask.newPickingBuilding;
        //theDeliveringBuilding.sprite = currentTask.newDeliveringBuilding;
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

        //Instantiate obstacles

        for (int i = 0; i < obstacleCount; i++)
        {
            Transform obstaclePosition = currentTask.newObstacles[i];

            Instantiate(obstacle, obstaclePosition.position, Quaternion.identity);
        }

    }

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
                Debug.Log("Pedido no entregado.");
                timerON = false;

                arrowDownUI.SetActive(true);

                PlayerController.instance.canMove = false;
                var StopVelocity = PlayerController.instance.theRigidBody.velocity = new Vector2(0f, 0f);
                PlayerController.instance.myAnim.SetFloat("moveX", StopVelocity.x);
                PlayerController.instance.myAnim.SetFloat("moveY", StopVelocity.y);

                StartCoroutine(DelayForNewTask());
                StartCoroutine(DisableChronometer());

                //Disable current pick and deliver points
                Destroy(theCurrentPickUpPoint);
                Destroy(PickUpSystem.instance.theCurrentDeliveryPoint);
                theItemUI.gameObject.SetActive(false);

                //Decrease Mood
                MoodDamaged(1);//Im hard coding here but i have to specify how much damage to the mood is going to happen

                //Stopping Cars
                //HorizontalVehicles.instance.canMove = false;

                foreach (HorizontalVehicles horizontalCar in horizontalVehicles)
                {
                    horizontalCar.canMove = false;
                }

                foreach (VerticalVehicles verticalCar in verticalVehicles)
                {
                    verticalCar.canMove = false;
                }

                //VerticalVehicles.instance.canMove = false;
                //verticalCar.canMove = false;
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

    public void MoodDamaged(int moodDecreased)
    {
        if (currentMood > 1)
        {
            currentMood -= moodDecreased;
        }
        else
        {
            currentMood = minMood;
            //GameOVER
            //Change to GameOverScene
            SceneManager.LoadScene("Final");
            Debug.Log("GAMEOVER");
        }

        UpdateMood();
    }

    public void MoodIncrement(int moodIncreased)
    {
        if (currentMood < 5)
        {
            currentMood += moodIncreased;
        }
        else
        {
            currentMood = maxMood;
        }

        UpdateMood();
    }

    public void UpdateMood()
    {
        switch (currentMood)
        {
            case 5:
                veryHappy.color = new Color(1, 1, 1, 1);
                flare1.gameObject.SetActive(true);
                Happy.color = new Color(1, 1, 1, 0.2f);
                flare2.gameObject.SetActive(false);
                Neutral.color = new Color(1, 1, 1, 0.2f);
                flare3.gameObject.SetActive(false);
                Sad.color = new Color(1, 1, 1, 0.2f);
                flare4.gameObject.SetActive(false);
                verySad.color = new Color(1, 1, 1, 0.2f);
                flare5.gameObject.SetActive(false);
                return;

            case 4:
                veryHappy.color = new Color(1, 1, 1, 0.2f);
                flare1.gameObject.SetActive(false);
                Happy.color = new Color(1, 1, 1, 1);
                flare2.gameObject.SetActive(true);
                Neutral.color = new Color(1, 1, 1, 0.2f);
                flare3.gameObject.SetActive(false);
                Sad.color = new Color(1, 1, 1, 0.2f);
                flare4.gameObject.SetActive(false);
                verySad.color = new Color(1, 1, 1, 0.2f);
                flare5.gameObject.SetActive(false);
                return;

            case 3:
                veryHappy.color = new Color(1, 1, 1, 0.2f);
                flare1.gameObject.SetActive(false);
                Happy.color = new Color(1, 1, 1, 0.2f);
                flare2.gameObject.SetActive(false);
                Neutral.color = new Color(1, 1, 1, 1f);
                flare3.gameObject.SetActive(true);
                Sad.color = new Color(1, 1, 1, 0.2f);
                flare4.gameObject.SetActive(false);
                verySad.color = new Color(1, 1, 1, 0.2f);
                flare5.gameObject.SetActive(false);
                return;

            case 2:
                veryHappy.color = new Color(1, 1, 1, 0.2f);
                flare1.gameObject.SetActive(false);
                Happy.color = new Color(1, 1, 1, 0.2f);
                flare2.gameObject.SetActive(false);
                Neutral.color = new Color(1, 1, 1, 0.2f);
                flare3.gameObject.SetActive(false);
                Sad.color = new Color(1, 1, 1, 1f);
                flare4.gameObject.SetActive(true);
                verySad.color = new Color(1, 1, 1, 0.2f);
                flare5.gameObject.SetActive(false);
                return;

            case 1:
                veryHappy.color = new Color(1, 1, 1, 0.2f);
                flare1.gameObject.SetActive(false);
                Happy.color = new Color(1, 1, 1, 0.2f);
                flare2.gameObject.SetActive(false);
                Neutral.color = new Color(1, 1, 1, 0.2f);
                flare3.gameObject.SetActive(false);
                Sad.color = new Color(1, 1, 1, 0.2f);
                flare4.gameObject.SetActive(false);
                verySad.color = new Color(1, 1, 1, 1);
                flare5.gameObject.SetActive(true);
                return;

            case 0:
                veryHappy.gameObject.SetActive(false);
                flare1.gameObject.SetActive(false);
                Happy.gameObject.SetActive(false);
                flare2.gameObject.SetActive(false);
                Neutral.gameObject.SetActive(false);
                flare3.gameObject.SetActive(false);
                Sad.gameObject.SetActive(false);
                flare4.gameObject.SetActive(false);
                verySad.gameObject.SetActive(false);
                flare5.gameObject.SetActive(false);
                return;

            default:
                veryHappy.gameObject.SetActive(false);
                flare1.gameObject.SetActive(false);
                Happy.gameObject.SetActive(false);
                flare2.gameObject.SetActive(false);
                Neutral.gameObject.SetActive(false);
                flare3.gameObject.SetActive(false);
                Sad.gameObject.SetActive(false);
                flare4.gameObject.SetActive(false);
                verySad.gameObject.SetActive(false);
                flare5.gameObject.SetActive(false);
                return;
        }
    }

    public void UpdatingVehicles()
    {
        switch (currentTaskIndex)
        {
            case 0:
                vehiclesToActivate[0].SetActive(true);
                vehiclesToActivate[1].SetActive(false);
                vehiclesToActivate[2].SetActive(false);
                vehiclesToActivate[3].SetActive(false);
                vehiclesToActivate[4].SetActive(false);
                vehiclesToActivate[5].SetActive(false);
                vehiclesToActivate[6].SetActive(false);
                return;

            case 1:
                vehiclesToActivate[0].SetActive(true);
                vehiclesToActivate[1].SetActive(false);
                vehiclesToActivate[2].SetActive(false);
                vehiclesToActivate[3].SetActive(false);
                vehiclesToActivate[4].SetActive(false);
                vehiclesToActivate[5].SetActive(false);
                vehiclesToActivate[6].SetActive(false);
                return;

            case 2:
                vehiclesToActivate[0].SetActive(true);
                vehiclesToActivate[1].SetActive(true);
                vehiclesToActivate[2].SetActive(false);
                vehiclesToActivate[3].SetActive(false);
                vehiclesToActivate[4].SetActive(false);
                vehiclesToActivate[5].SetActive(false);
                vehiclesToActivate[6].SetActive(false);
                return;

            case 3:
                vehiclesToActivate[0].SetActive(true);
                vehiclesToActivate[1].SetActive(true);
                vehiclesToActivate[2].SetActive(true);
                vehiclesToActivate[3].SetActive(false);
                vehiclesToActivate[4].SetActive(false);
                vehiclesToActivate[5].SetActive(false);
                vehiclesToActivate[6].SetActive(false);
                return;

            case 4:
                vehiclesToActivate[0].SetActive(true);
                vehiclesToActivate[1].SetActive(true);
                vehiclesToActivate[2].SetActive(true);
                vehiclesToActivate[3].SetActive(true);
                vehiclesToActivate[4].SetActive(true);
                vehiclesToActivate[5].SetActive(false);
                vehiclesToActivate[6].SetActive(false);
                return;

            case 5:
                vehiclesToActivate[0].SetActive(true);
                vehiclesToActivate[1].SetActive(true);
                vehiclesToActivate[2].SetActive(true);
                vehiclesToActivate[3].SetActive(false);
                vehiclesToActivate[4].SetActive(true);
                vehiclesToActivate[5].SetActive(true);
                vehiclesToActivate[6].SetActive(false);
                return;

            case 6:
                vehiclesToActivate[0].SetActive(true);
                vehiclesToActivate[1].SetActive(true);
                vehiclesToActivate[2].SetActive(true);
                vehiclesToActivate[3].SetActive(false);
                vehiclesToActivate[4].SetActive(true);
                vehiclesToActivate[5].SetActive(false);
                vehiclesToActivate[6].SetActive(true);
                return;

            default:
                vehiclesToActivate[0].SetActive(false);
                vehiclesToActivate[1].SetActive(false);
                vehiclesToActivate[2].SetActive(false);
                vehiclesToActivate[3].SetActive(false);
                vehiclesToActivate[4].SetActive(false);
                vehiclesToActivate[5].SetActive(false);
                vehiclesToActivate[6].SetActive(false);
                return;
        }
    }

    #endregion

    #region CoRoutines

    IEnumerator DelayForNewTask()
    {
        yield return new WaitForSeconds(waitForNewTask);
        taskFinished = true;

        //Camera Zoom Out
        if (currentCameraSize > cameraMinSize)
        {
            currentCameraSize -= cameraChangeIndex;
        }
        else
        {
            currentCameraSize = cameraMinSize;
        }
    }

    IEnumerator DisableChronometer()
    {
        yield return new WaitForSeconds(2f);
        timerText.gameObject.SetActive(false);
    }

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

    IEnumerator DelayForGameFinishScreen()
    {
        yield return new WaitForSeconds(8f);
        SceneManager.LoadScene("GameFinished");
    }

    IEnumerator TurnOFFSMS()
    {
        yield return new WaitForSeconds(2);
        smsUI.SetActive(false);
    }
    #endregion
}

#region SubClasses
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
    public int newTaskReward;

    [Header("Obstacles")]
    public Transform[] newObstacles;

    [TextArea]
    public string newTaskText;

    //An array of Transforms where there are going to be obstacles
    //An array of obstacles
}

#endregion 
