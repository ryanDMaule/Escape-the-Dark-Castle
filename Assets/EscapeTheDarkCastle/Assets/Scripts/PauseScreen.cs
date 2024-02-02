using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    [Header("Game objects")]
    public GameObject pauseScreen;

    [Header("Events")]
    public GameEvent PauseMusic;
    public GameEvent ResumeMusic;

    public void closePauseScren()
    {
        ResumeMusic.Raise();
        pauseScreen.SetActive(false);
    }

    public void openPauseScren()
    {
        pauseScreen.SetActive(true);
        PauseMusic.Raise();
    }

    public void togglePauseScreen()
    {
        if (pauseScreen.gameObject.activeInHierarchy)
        {
            closePauseScren();
        } else
        {
            openPauseScren();
        }
    }

}
