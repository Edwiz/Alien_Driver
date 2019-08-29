using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResumeDisplay : MonoBehaviour
{
    public Image star1;
    public Image star2;
    public Image star3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.instance.completedTasks == GameManager.instance.currentDay.tasks.Length)
        {
            star1.gameObject.SetActive(true);
            star2.gameObject.SetActive(true);
            star3.gameObject.SetActive(true);
        }
        else if (GameManager.instance.completedTasks < GameManager.instance.currentDay.tasks.Length && GameManager.instance.completedTasks > 1)
        {
            star1.gameObject.SetActive(true);
            star2.gameObject.SetActive(true);
            star3.gameObject.SetActive(false);
        }
        else
        {
            star1.gameObject.SetActive(true);
            star2.gameObject.SetActive(false);
            star3.gameObject.SetActive(false);
        }
    }
}
