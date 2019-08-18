using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    public DayManagement[] gameDays;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class DayManagement
{
    [Header("Day Number")]
    public string dayNumber;

    [Header("Map to Load")]
    public string mapName;

    [Header("Ending Phrase")]
    public string endingPhrase;

    [Header("Number of Tasks")]
    public Tasks[] tasks;
}

[System.Serializable]
public class Tasks
{
    [Header("Task Number")]
    public string taskNumber;

    [Header("Task Window")]
    public string taskMessage;
    public Sprite character;
    public Sprite pickUpPlace;
    public Sprite deliveryPlace;
    public Sprite itemToPickUp;

    [Header("Location Points")]
    public Transform pickPoint;
    public Transform deliverPoint;

    [Header("Time to Deliver")]
    public float deliveryTime;

    [Header("Obstacles")]
    public GameObject[] obstacles;
}