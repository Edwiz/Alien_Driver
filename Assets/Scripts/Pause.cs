using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public static Pause instance;

    public GameObject pauseMenu;

    public bool pauseActive;
    public bool canActivate;

    // Use this for initialization
    void Start()
    {
        instance = this;
        pauseActive = false;
        canActivate = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && !pauseActive && canActivate)
        {
            Cursor.visible = false;
            pauseMenu.SetActive(true);
            pauseActive = true;
            Time.timeScale = 0f;
        }
        else if (Input.GetButtonDown("Cancel") && pauseActive)
        {
            pauseMenu.SetActive(false);
            pauseActive = false;
            Time.timeScale = 1f;
        }
    }

    public void ContinueButton()
    {
        pauseMenu.SetActive(false);
        pauseActive = false;
        Time.timeScale = 1f;
    }

    public void MainMenu()
    {
        pauseActive = false;
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
