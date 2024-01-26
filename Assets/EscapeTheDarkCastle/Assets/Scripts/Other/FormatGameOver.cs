using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FormatGameOver : MonoBehaviour
{
    [Header("Overrides")]
    [SerializeField] public AudioSource musicSource;
    [SerializeField] public TextMeshProUGUI bio;

    //TODO: find a way to set these active just using "this"
    [Header("Game objects")]
    [SerializeField] public GameObject gradient;
    [SerializeField] public GameObject frame;
    [SerializeField] public GameObject audioPlayer;
    [SerializeField] public GameObject scrollView;
    [SerializeField] public GameObject button;

    public void formatDeathScreen()
    {
        //trigger this as game event 
        Debug.Log("formatDeathScreen");

        showElements();

        musicSource.clip = MainManager.Instance.cl.deathClip;
        bio.text = MainManager.Instance.cl.deathBio;
    }

    private void showElements()
    {
        gradient.gameObject.SetActive(true);
        frame.gameObject.SetActive(true);
        audioPlayer.gameObject.SetActive(true);
        scrollView.gameObject.SetActive(true);
        button.gameObject.SetActive(true);
    }

    public void endGameButton()
    {
        //TODO: return to the main menu
        #if UNITY_STANDALONE
                Application.Quit();
        #endif
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

}
