using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{

    [Header("Game objects")]
    [SerializeField] public GameObject advanceButton;

    [Header("Selection colours")]
    public Color standardColour;
    public Color pressedColour;

    public void ToggleTimeSpeed()
    {
        if(Time.timeScale >= 2)
        {
            advanceButton.GetComponentInChildren<Image>().color = standardColour;
            StandardTime();
        } else
        {
            advanceButton.GetComponentInChildren<Image>().color = pressedColour;
            DoubleTime();
        }
    }

    public void DoubleTime()
    {
        Time.timeScale = 2;
    }

    public void StandardTime()
    {
        Time.timeScale = 1;
    }

    public void showTimeButton()
    {
        advanceButton.gameObject.SetActive(true);
    }

    public void hideTimeButton()
    {
        StandardTime();
        advanceButton.GetComponentInChildren<Image>().color = standardColour;
        advanceButton.gameObject.SetActive(false);
    }

}
