using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskPanel : MonoBehaviour
{
    [Header("Configuration")]

    public bool panelActive;

    public Image _theCharacter;
    public Image _thePickingBuiling;
    public Image _theDeliveringBuilding;
    public Image _theItem;
    public Text _theTime;
    public Text _theTaskText;
    public Text _theTaskNumber;

    public void Start()
    {
        /*
        _theCharacter.sprite = TestManager.instance.theCharacter;
        _thePickingBuiling.sprite = TestManager.instance.thePickingBuiling;
        _theDeliveringBuilding.sprite = TestManager.instance.theDeliveringBuilding;
        _theItem.sprite = TestManager.instance.theItem;
        _theTime = TestManager.instance.timerText;
        _theTaskText.text = TestManager.instance.theTaskText.ToString();
        _theTaskNumber.text = "Pedido #" + TestManager.instance.currentTaskIndex.ToString();*/
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (panelActive == true)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                Instantiate(TestManager.instance.pickUp, TestManager.instance.pickUpPoint.position, Quaternion.identity);
                panelActive = false;
                gameObject.SetActive(false);
            }
        }*/
    }
}
