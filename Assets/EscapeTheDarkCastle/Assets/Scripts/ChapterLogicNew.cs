using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ChapterLogicNew : MonoBehaviour
{
    #region globalVariables

    [SerializeField] public GameObject playersCombinedHUD;

    [SerializeField] public Text win_lose_text;
    [SerializeField] public Button Continue_button;

    [SerializeField] private EnemyBase enemyBase;

    public BattleState state;

    public Scenes scenes;

    public FormatChapter fc;

    #endregion

    public BattleState getState()
    {
        return state;
    }

    void Start()
    {
        MainManager.Instance.printPlayers();

        setDescriptionPhase();
    }

    public void setDescriptionPhase()
    {
        state = BattleState.DESCRIPTION;
        setDescriptionHUD();
    }

    public void setCombatOptionsPhase()
    {
        state = BattleState.COMBAT_OPTIONS;
        setCombatOptionsHUD();
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

    public void setPlayerTurnPhase()
    {
        setPlayerTurnHUD();
    }

    public void win()
    {
        Debug.Log("YOU WIN!");
        //setWinHUD();

        Loader.Load(scenes.itemScreen);
    }

    #region HUD_methods
    void setDescriptionHUD()
    {
        Debug.Log("setDescriptionHUD");

        enemyBase.SHOW_DESCRIPTION();
        enemyBase.hideOptionsHUD();
        enemyBase.SET_ENEMY_IMAGE_ONLY();

        playersCombinedHUD.SetActive(false);

        win_lose_text.gameObject.SetActive(false);
        Continue_button.gameObject.SetActive(false);
    }

    void setCombatOptionsHUD()
    {
        Debug.Log("setCombatOptionsHUD");

        enemyBase.SHOW_DESCRIPTION();
        enemyBase.showOptionsHUD();
        enemyBase.SET_ENEMY_IMAGE_ONLY();

        playersCombinedHUD.SetActive(false);

        win_lose_text.gameObject.SetActive(false);
        Continue_button.gameObject.SetActive(false);
    }

    void setEnemyHUD()
    {
        Debug.Log("setEnemyHUD");

        enemyBase.HIDE_DESCRIPTION();
        enemyBase.hideOptionsHUD();
        enemyBase.SET_ENEMY_ASSETS_VISIBLE();

        playersCombinedHUD.SetActive(true);

        win_lose_text.gameObject.SetActive(false);
        Continue_button.gameObject.SetActive(false);
    }

    void setPreperationHUD()
    {
        Debug.Log("setPreperationHUD");

        enemyBase.HIDE_DESCRIPTION();
        enemyBase.hideOptionsHUD();
        enemyBase.SET_ENEMY_ASSETS_VISIBLE();

        //set the rest and fight buttons to active
        playersCombinedHUD.SetActive(true);
        foreach(var player in MainManager.Instance.Players)
        {
            //player.prepHUD();
            player.PREPERATION_HUD_NEW();
        }
        
        win_lose_text.gameObject.SetActive(false);
        Continue_button.gameObject.SetActive(true);
    }
    void setPlayerTurnHUD()
    {
        Debug.Log("setPlayerTurnHUD");

        enemyBase.HIDE_DESCRIPTION();
        enemyBase.hideOptionsHUD();
        enemyBase.SET_ENEMY_ASSETS_VISIBLE();

        playersCombinedHUD.SetActive(false);

        win_lose_text.gameObject.SetActive(false);
        Continue_button.gameObject.SetActive(false);
    }

    void setEnemyTurnHUD()
    {
        Debug.Log("setEnemyTurnHUD");

        enemyBase.HIDE_DESCRIPTION();
        enemyBase.hideOptionsHUD();
        enemyBase.SET_ENEMY_ASSETS_VISIBLE();

        playersCombinedHUD.SetActive(false);

        win_lose_text.gameObject.SetActive(false);
        Continue_button.gameObject.SetActive(false);
    }

    void setWinHUD()
    {
        Debug.Log("setWinHUD");

        enemyBase.HIDE_DESCRIPTION();
        enemyBase.hideOptionsHUD();
        enemyBase.SET_ENEMY_ASSETS_VISIBLE();

        playersCombinedHUD.SetActive(false);

        win_lose_text.gameObject.SetActive(true);
        win_lose_text.text = "YOU WIN!";
        Continue_button.gameObject.SetActive(false);
    }

    void setLoseHUD()
    {
        Debug.Log("setLoseHUD");

        enemyBase.HIDE_DESCRIPTION();
        enemyBase.hideOptionsHUD();
        enemyBase.SET_ENEMY_ASSETS_VISIBLE();

        playersCombinedHUD.SetActive(false);

        win_lose_text.gameObject.SetActive(true);
        win_lose_text.text = "YOU LOSE!";
        Continue_button.gameObject.SetActive(false);
    }

    #endregion
}
