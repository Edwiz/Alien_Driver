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
            StartCoroutine(DisplayingStars_All());
        }
        else if (GameManager.instance.completedTasks < GameManager.instance.currentDay.tasks.Length && GameManager.instance.completedTasks > 1)
        {
            StartCoroutine(DisplayingStars_Half());
        }
        else
        {
            StartCoroutine(DisplayingStars_One());
        }
    }

    IEnumerator DisplayingStars_All()
    {
        star1.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        star2.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        star3.gameObject.SetActive(true);
    }

    IEnumerator DisplayingStars_Half()
    {
        star3.gameObject.SetActive(false);

        star1.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        star2.gameObject.SetActive(true);
    }

    IEnumerator DisplayingStars_One()
    {
        star3.gameObject.SetActive(false);
        star2.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);
        star1.gameObject.SetActive(true);
    }
}
