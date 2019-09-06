using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    private GameObject target;
    private Transform targetPos;
    private float distance;
    private SpriteRenderer theSpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        theSpriteRenderer = GetComponent<SpriteRenderer>();
        theSpriteRenderer.color = new Color(1, 1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.timerON == true)
        {
            theSpriteRenderer.color = new Color(1, 1, 1, 1);
        }
        else
        {
            theSpriteRenderer.color = new Color(1, 1, 1, 0);
        }

        //For rotation towards the target
        target = GameObject.FindGameObjectWithTag("Indicator");
        targetPos = target.transform;

        Vector2 direction = targetPos.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;

        /*
        //Checking distance between arrow and target
        distance = Vector2.Distance(transform.position, targetPos.position);

        if (distance > 5)
        {
            transform.localScale = new Vector3(0.12536f, 0.12536f, 0.12536f);
        }
        else if (distance > 0 && distance < 5)
        {
            transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        }
        else
        {
            transform.localScale = new Vector3(0f, 0f, 0f);
        }
        */
    }
}
