using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CastleMazeChapterLogic : ChapterLogicBase
{
    #region globalVariables
    [Header("Game objects")]
    [SerializeField] public GameObject playersCombinedHUD;

    [SerializeField] public GameObject winHUD;
    [SerializeField] public Button Continue_button;

    [SerializeField] public Text roundNumber;

    [Header("Other")]
    public Scenes scenes;

    public CastleMazeFormatChapter fc;

    #endregion

    void Start()
    {
        MainManager.Instance.updateGameState(GameState.CHAPTER);

        setDescriptionPhase();
    }

    public void setEnemyHealthPhase()
    {
        state = BattleState.SET_ENEMY_HEALTH;
        setEnemyHUD();
    }

    public void preperationPhase()
    {
        Debug.Log("preperationPhase");

        setPreperationHUD();
    }

    public void startEnemyTurnPhase()
    {
        if (!enemyBase.enemyDead())
        {
            state = BattleState.ENEMY_TURN;

            SoundFXPlayer soundFX = FindFirstObjectByType<SoundFXPlayer>();
            soundFX.PlayDamageTaken();

            foreach (var player in MainManager.Instance.Players)
            {
                player.RedcuceHealth(enemyBase.getDamage());    
                player.setPotionProtectionState(false);
            }

            continueChapter();
        }
    }


    public void win()
    {
        Debug.Log("YOU WIN!");
        setWinHUD();
    }

    #region HUD_methods
    public override void setDescriptionHUD()
    {
        base.setDescriptionHUD();

        playersCombinedHUD.SetActive(false);

        winHUD.gameObject.SetActive(false);
        Continue_button.gameObject.SetActive(false);

        roundNumber.gameObject.SetActive(false);
    }

    public override void setCombatOptionsHUD()
    {
        base.setCombatOptionsHUD();

        playersCombinedHUD.SetActive(false);

        winHUD.gameObject.SetActive(false);
        Continue_button.gameObject.SetActive(false);

        roundNumber.gameObject.SetActive(false);
    }

    void setEnemyHUD()
    {
        Debug.Log("setEnemyHUD");

        enemyBase.HIDE_DESCRIPTION();
        enemyBase.hideOptionsHUD();
        enemyBase.SET_ENEMY_ASSETS_VISIBLE();

        playersCombinedHUD.SetActive(true);

        winHUD.gameObject.SetActive(false);
        Continue_button.gameObject.SetActive(false);

        roundNumber.gameObject.SetActive(true);
    }

    void setPreperationHUD()
    {
        Debug.Log("setPreperationHUD");

        enemyBase.HIDE_DESCRIPTION();
        enemyBase.hideOptionsHUD();
        enemyBase.SET_ENEMY_ASSETS_VISIBLE();

        //set the rest and fight buttons to active
        playersCombinedHUD.SetActive(true);
        foreach (var player in MainManager.Instance.Players)
        {
            player.PREPERATION_HUD_NEW();
        }

        winHUD.gameObject.SetActive(false);
        Continue_button.gameObject.SetActive(true);

        roundNumber.gameObject.SetActive(true);
    }

    public override void setWinHUD()
    {
        base.setWinHUD();

        playersCombinedHUD.SetActive(false);

        winHUD.gameObject.SetActive(true);
        Continue_button.gameObject.SetActive(false);

        roundNumber.gameObject.SetActive(false);
    }

    #endregion

    int round = 1;
    public void continueChapter()
    {
        var dead = enemyBase.enemyDead();
        if (dead || round >= 3)
        {
            setWinHUD();
        } else
        {
            fc.formatBlock();
        }
        round++;
        roundNumber.text = "Attempt " + round + " / 3";
    }

}
