using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToGame : MonoBehaviour
{
    // Start is called before the first frame update

    public void TransitionToGame()
    {
        SceneManager.LoadScene("LoadingScene");
    }

}
