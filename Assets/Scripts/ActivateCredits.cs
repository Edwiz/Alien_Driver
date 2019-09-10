using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCredits : MonoBehaviour
{
    public GameObject creditsPanel;
    public GameObject menuButton;

    public void ActivatingCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void ActivatingMenuButton()
    {
        menuButton.SetActive(true);
    }

}
