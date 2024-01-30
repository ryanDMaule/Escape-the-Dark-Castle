using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverHandler : MonoBehaviour
{
    [Header("Overrides")]
    [SerializeField] public AudioSource musicSource;
    [SerializeField] public TextMeshProUGUI bio;

    [Header("Game objects")]
    public GameObject endGameScreen;
    public GameObject adButton;

    [Header("Events")]
    public GameEvent GameEnd;

    void hideAdButton()
    {
        print("hideAdButton");
        adButton.gameObject.SetActive(false);
    }

    void closeGameOverOverlay()
    {
        endGameScreen.gameObject.SetActive(false);
    }

    void showGameOverOverlay()
    {
        endGameScreen.gameObject.SetActive(true);
    }

    void adReward()
    {
        print("adReward");
        foreach (var player in MainManager.Instance.Players)
        {
            player.IncreaseHealth(2);
        }
    }

    public void adComplete()
    {
        print("adComplete");

        hideAdButton();
        closeGameOverOverlay();

        adReward();
    }

    public void formatDeathScreen()
    {
        //trigger this as game event 
        Debug.Log("formatDeathScreen");

        showGameOverOverlay();

        musicSource.clip = MainManager.Instance.cl.deathClip;
        bio.text = MainManager.Instance.cl.deathBio;
    }

    public void endGameButton()
    {
        GameEnd.Raise();

        //TODO: return to the main menu
        //#if UNITY_STANDALONE
        //                Application.Quit();
        //#endif
        //#if UNITY_EDITOR
        //        UnityEditor.EditorApplication.isPlaying = false;
        //        #endif
    }

}
