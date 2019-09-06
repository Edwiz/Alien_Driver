using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public AkEvent audioSeleccionar;

    public void play_Seleccionar()
    {
        if(audioSeleccionar != null)
        {
            audioSeleccionar.HandleEvent(gameObject);
        }
    }

}
