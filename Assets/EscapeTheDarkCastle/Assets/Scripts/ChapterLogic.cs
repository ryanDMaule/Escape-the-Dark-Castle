using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { DESCRIPTION, COMBAT_OPTIONS, SET_ENEMY_HEALTH, PREPERATION, PLAYER_TURN, ENEMY_TURN, WON, LOST }


public class ChapterLogic : MonoBehaviour
{
    #region globalVariables

    [SerializeField] public Image divider;

    [SerializeField] public Text win_lose_text;
    [SerializeField] public Button Continue_button;

    [SerializeField] public Abbot Abbot;
    [SerializeField] public Miller Miller;

    [SerializeField] private EnemyBase enemyBase;

    public BattleState state;

    public Scenes scenes;

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
        //enemy.setEnemyMight(2);

        //both player to roll their chapter die
        StartCoroutine(setEnemyhealthIE());
    }

    IEnumerator setEnemyhealthIE()
    {
        while (Abbot.getChapterDieButton().gameObject.activeSelf)
        {
            yield return null;
        }

        while (Miller.getChapterDieButton().gameObject.activeSelf)
        {
            yield return null;
        }

        yield return new WaitForSeconds(2f);
        preperationPhase();
    }

    public void preperationPhase()
    {
        Debug.Log("preperationPhase");

        setPreperationHUD();
    }

    public void setPlayerTurnPhase()
    {
        setPlayerTurnHUD();

        //if player is resting, disable elements
        if (Abbot.getIsRestingState())
        {
            Debug.Log("ABBOT : RESTING");

            Abbot.HIDE_CHARACTER_DICE_BUTTON();
            Abbot.IncreaseHealth(1);
        }
        else if (Miller.getIsRestingState())
        {
            Debug.Log("MILLER : RESTING");

            Miller.HIDE_CHARACTER_DICE_BUTTON();
            Miller.IncreaseHealth(1);
        }
        else
        {
            Debug.Log("BOTH : FIGHTING");
            Abbot.SHOW_CHARACTER_DICE_BUTTON();
            Miller.SHOW_CHARACTER_DICE_BUTTON();
        }

        StartCoroutine(enemyPhaseLock());
    }

    IEnumerator enemyPhaseLock()
    {
        Debug.Log("enemyPhaseLock");

        if (Abbot.getIsRestingState())
        {
            Debug.Log("ABBOT : RESTING");

            while (Miller.getCharacterDieButton().gameObject.activeSelf)
            {
                yield return null;
            }
        }
        else if (Miller.getIsRestingState())
        {
            Debug.Log("MILLER : RESTING");

            while (Abbot.getCharacterDieButton().gameObject.activeSelf)
            {
                yield return null;
            }
        }
        else
        {
            Debug.Log("BOTH : FIGHTING");

            //while (Abbot.getChapterDie().gameObject.activeSelf)
            while (Abbot.getCharacterDieButton().gameObject.activeSelf)
            {
                yield return null;
            }

            while (Miller.getCharacterDieButton().gameObject.activeSelf)
            {
                yield return null;
            }
        }

        if (enemyBase.enemyDead())
        {
            yield return new WaitForSeconds(2f);
            win();
        }
        else
        {
            yield return new WaitForSeconds(2f);
            setEnemyTurnHUD();
            StartCoroutine(enemyPhase());
            Debug.Log("setEnemyTurnHUD");
        }
    }

    IEnumerator enemyPhase()
    {
        bool abbotDead = false;
        bool MillerDead = false;

        if (!Abbot.getShieldActiveState() && !Abbot.getIsRestingState())
        {
            abbotDead = Abbot.RedcuceHealth(enemyBase.getDamage());
        }
        if (!Miller.getShieldActiveState() && !Miller.getIsRestingState())
        {
            MillerDead = Miller.RedcuceHealth(enemyBase.getDamage());
        }

        if (abbotDead || MillerDead)
        {
            yield return new WaitForSeconds(2f);
            setLoseHUD();
        }
        else
        {
            yield return new WaitForSeconds(2f);
            setPreperationHUD();
        }

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

        Abbot.SET_DESCRIPTION_HUD();
        Miller.SET_DESCRIPTION_HUD();

        win_lose_text.gameObject.SetActive(false);
        Continue_button.gameObject.SetActive(false);

        divider.gameObject.SetActive(false);
    }

    void setCombatOptionsHUD()
    {
        Debug.Log("setCombatOptionsHUD");

        enemyBase.SHOW_DESCRIPTION();
        enemyBase.showOptionsHUD();
        enemyBase.SET_ENEMY_IMAGE_ONLY();

        Abbot.SET_DESCRIPTION_HUD();
        Miller.SET_DESCRIPTION_HUD();

        win_lose_text.gameObject.SetActive(false);
        Continue_button.gameObject.SetActive(false);

        divider.gameObject.SetActive(false);
    }

    void setEnemyHUD()
    {
        Debug.Log("setEnemyHUD");

        enemyBase.HIDE_DESCRIPTION();
        enemyBase.hideOptionsHUD();
        enemyBase.SET_ENEMY_ASSETS_VISIBLE();

        Abbot.SET_ENEMY_HEALTH_HUD();
        Miller.SET_ENEMY_HEALTH_HUD();

        win_lose_text.gameObject.SetActive(false);
        Continue_button.gameObject.SetActive(false);

        divider.gameObject.SetActive(true);
    }

    void setPreperationHUD()
    {
        Debug.Log("setPreperationHUD");

        enemyBase.HIDE_DESCRIPTION();
        enemyBase.hideOptionsHUD();
        enemyBase.SET_ENEMY_ASSETS_VISIBLE();

        Abbot.PREPERATION_HUD();
        Miller.PREPERATION_HUD();

        win_lose_text.gameObject.SetActive(false);
        Continue_button.gameObject.SetActive(true);

        divider.gameObject.SetActive(true);

        Abbot.setShieldActiveState(false);
        Miller.setShieldActiveState(false);
    }
    void setPlayerTurnHUD()
    {
        Debug.Log("setPlayerTurnHUD");

        enemyBase.HIDE_DESCRIPTION();
        enemyBase.hideOptionsHUD();
        enemyBase.SET_ENEMY_ASSETS_VISIBLE();

        Abbot.PLAYER_TURN_HUD();
        Miller.PLAYER_TURN_HUD();

        divider.gameObject.SetActive(true);

        win_lose_text.gameObject.SetActive(false);
        Continue_button.gameObject.SetActive(false);
    }

    void setEnemyTurnHUD()
    {
        Debug.Log("setEnemyTurnHUD");

        enemyBase.HIDE_DESCRIPTION();
        enemyBase.hideOptionsHUD();
        enemyBase.SET_ENEMY_ASSETS_VISIBLE();

        Abbot.ENENMY_TURN_WON_LOST_HUD();
        Miller.ENENMY_TURN_WON_LOST_HUD();

        divider.gameObject.SetActive(true);

        win_lose_text.gameObject.SetActive(false);
        Continue_button.gameObject.SetActive(false);
    }

    void setWinHUD()
    {
        Debug.Log("setWinHUD");

        enemyBase.HIDE_DESCRIPTION();
        enemyBase.hideOptionsHUD();
        enemyBase.SET_ENEMY_ASSETS_VISIBLE();

        Abbot.ENENMY_TURN_WON_LOST_HUD();
        Miller.ENENMY_TURN_WON_LOST_HUD();

        divider.gameObject.SetActive(false);

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

        Abbot.ENENMY_TURN_WON_LOST_HUD();
        Miller.ENENMY_TURN_WON_LOST_HUD();

        divider.gameObject.SetActive(false);

        win_lose_text.gameObject.SetActive(true);
        win_lose_text.text = "YOU LOSE!";
        Continue_button.gameObject.SetActive(false);
    }

    #endregion
}
