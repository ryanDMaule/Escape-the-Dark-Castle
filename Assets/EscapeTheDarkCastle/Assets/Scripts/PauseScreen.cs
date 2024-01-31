using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    [Header("Game objects")]
    public GameObject pauseScreen;
    public void closePauseScren()
    {
        pauseScreen.SetActive(false);
    }

    public void openPauseScren()
    {
        pauseScreen.SetActive(true);
    }

}
