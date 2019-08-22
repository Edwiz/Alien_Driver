using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    #region Fields

    [Header("Task Manager Configuration")]

    //Array variables
    public Task[] tasks;

    //Private variables
    private Task currentTask;
    private int currentTaskIndex;

    #endregion

    #region TimerFields

    [Header("Timer Configuration")]
    
    public bool timerON;

    //Cached variables
    public Text timerText;

    //Private variables
    private float timeForTask;
    private string timeCounter;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        currentTaskIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(currentTaskIndex);

        Chronometer();

        StartNewTask(currentTaskIndex);
    }

    void StartNewTask(int index)
    {
        currentTask = tasks[index];
        //timeForTask = currentTask.deliveryTime;
        timerON = true;
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

            if (timeForTask > 0)
            {
                timeForTask -= Time.deltaTime;
            }
            else
            {
                StartCoroutine(DisableChronometer());
            }

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
        //timerON = false;
        //timerText.gameObject.SetActive(false);
        currentTaskIndex++;
    }
}

[System.Serializable]
public class Taskit
{
    [Header("Task Number")]
    public string taskNumber;

    [Header("Location Points")]
    public Transform pickPoint;
    public Transform deliverPoint;

    [Header("Time to Deliver")]
    public float deliveryTime;

    /*[Header("Task Window")]
    public string taskMessage;
    public Sprite character;
    public Sprite pickUpPlace;
    public Sprite deliveryPlace;
    public Sprite itemToPickUp;

    [Header("Obstacles")]
    public GameObject[] obstacles;*/
}
