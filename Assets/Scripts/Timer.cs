using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    #region Fields

    public static Timer instance;

    [Header("Timer Configuration")]

    public float timeForTask;
    public bool timerON;

    //Cached variables
    public Text timerText;

    //Private variables

    private string timeCounter;

    #endregion

    public void Start()
    {
        timerON = true;
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        Chronometer();

        if (DeliverySystem.instance.itemDelivered == true)
        {
            timerON = false;
            StartCoroutine(DisableChronometer());
        }
    }

    public void Chronometer()
    {
        if(timerON)
        {
            timerText.gameObject.SetActive(true);

            int minutes = Mathf.FloorToInt(timeForTask / 60F);
            int seconds = Mathf.FloorToInt(timeForTask - minutes * 60);
            timeCounter = string.Format("{0:00}:{1:00}", minutes, seconds);
            timerText.text = timeCounter.ToString();

            if (timeForTask > 1)
            {
                timeForTask -= Time.deltaTime;
            }
            else
            {
                timerON = false;
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
        timerText.gameObject.SetActive(false);
    }
}
