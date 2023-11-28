using System.Collections;
using System.Collections.Generic;
using InnerDriveStudios.DiceCreator;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { DESCRIPTION, COMBAT_OPTIONS, SET_ENEMY_HEALTH, PREPERATION, PLAYER_TURN, ENEMY_TURN, WON, LOST }


public class ChapterLogic : MonoBehaviour
{
    #region globalVariables

    [SerializeField] public Image enemyImage;
    [SerializeField] public Image enemy_might;
    [SerializeField] public Image enemy_cunning;
    [SerializeField] public Image enemy_wisdom;
    [SerializeField] public Image enemy_damage_image;
    [SerializeField] public Image divider;

    [SerializeField] public Text enemy_might_text;
    [SerializeField] public Text enemy_cunning_text;
    [SerializeField] public Text enemy_wisdom_text;

    [SerializeField] public ScrollRect description;
    [SerializeField] public Button description_button;

    [SerializeField] public Text win_lose_text;
    [SerializeField] public Button Continue_button;

    [SerializeField] public int enemy_damage;
    [SerializeField] public Abbot Abbot;
    [SerializeField] public Miller Miller;

    [SerializeField] private Enemy enemy;
    [SerializeField] private EnemyHud enemyHud;

    public BattleState state;

    #endregion

    void Start()
    {
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

        if (enemy.enemyDead())
        {
            yield return new WaitForSeconds(2f);
            setWinHUD();
            Debug.Log("setWinHUD");
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
            abbotDead = Abbot.RedcuceHealth(enemy.getDamage());
        }
        if (!Miller.getShieldActiveState() && !Miller.getIsRestingState())
        {
            MillerDead = Miller.RedcuceHealth(enemy.getDamage());
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

    #region HUD_methods
    void setDescriptionHUD()
    {
        Debug.Log("setDescriptionHUD");

        description.gameObject.SetActive(true);
        description_button.gameObject.SetActive(true);

        enemyHud.hideOptionsHUD();

        enemyImage.gameObject.SetActive(true);
        enemy_might.gameObject.SetActive(false);
        enemy_cunning.gameObject.SetActive(false);
        enemy_wisdom.gameObject.SetActive(false);
        enemy_damage_image.gameObject.SetActive(false);
        enemy_might_text.gameObject.SetActive(false);
        enemy_cunning_text.gameObject.SetActive(false);
        enemy_wisdom_text.gameObject.SetActive(false);

        Abbot.SET_DESCRIPTION_HUD();
        Miller.SET_DESCRIPTION_HUD();

        win_lose_text.gameObject.SetActive(false);
        Continue_button.gameObject.SetActive(false);

        divider.gameObject.SetActive(false);
    }

    void setCombatOptionsHUD()
    {
        Debug.Log("setCombatOptionsHUD");

        description.gameObject.SetActive(true);
        description_button.gameObject.SetActive(true);

        enemyHud.showOptionsHUD();

        enemyImage.gameObject.SetActive(true);
        enemy_might.gameObject.SetActive(false);
        enemy_cunning.gameObject.SetActive(false);
        enemy_wisdom.gameObject.SetActive(false);
        enemy_damage_image.gameObject.SetActive(false);
        enemy_might_text.gameObject.SetActive(false);
        enemy_cunning_text.gameObject.SetActive(false);
        enemy_wisdom_text.gameObject.SetActive(false);

        Abbot.SET_DESCRIPTION_HUD();
        Miller.SET_DESCRIPTION_HUD();

        win_lose_text.gameObject.SetActive(false);
        Continue_button.gameObject.SetActive(false);

        divider.gameObject.SetActive(false);
    }

    void setEnemyHUD()
    {
        Debug.Log("setEnemyHUD");

        description.gameObject.SetActive(false);
        description_button.gameObject.SetActive(false);

        enemyHud.hideOptionsHUD();

        enemyImage.gameObject.SetActive(true);
        enemy_might.gameObject.SetActive(true);
        enemy_cunning.gameObject.SetActive(true);
        enemy_wisdom.gameObject.SetActive(true);
        enemy_damage_image.gameObject.SetActive(true);
        enemy_might_text.gameObject.SetActive(true);
        enemy_cunning_text.gameObject.SetActive(true);
        enemy_wisdom_text.gameObject.SetActive(true);

        Abbot.SET_ENEMY_HEALTH_HUD();
        Miller.SET_ENEMY_HEALTH_HUD();

        win_lose_text.gameObject.SetActive(false);
        Continue_button.gameObject.SetActive(false);

        divider.gameObject.SetActive(true);
    }

    void setPreperationHUD()
    {
        Debug.Log("setPreperationHUD");

        description.gameObject.SetActive(false);
        description_button.gameObject.SetActive(false);

        enemyHud.hideOptionsHUD();

        enemyImage.gameObject.SetActive(true);
        enemy_might.gameObject.SetActive(true);
        enemy_cunning.gameObject.SetActive(true);
        enemy_wisdom.gameObject.SetActive(true);
        enemy_damage_image.gameObject.SetActive(true);
        enemy_might_text.gameObject.SetActive(true);
        enemy_cunning_text.gameObject.SetActive(true);
        enemy_wisdom_text.gameObject.SetActive(true);

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

        description.gameObject.SetActive(false);
        description_button.gameObject.SetActive(false);

        enemyHud.hideOptionsHUD();

        enemyImage.gameObject.SetActive(true);
        enemy_might.gameObject.SetActive(true);
        enemy_cunning.gameObject.SetActive(true);
        enemy_wisdom.gameObject.SetActive(true);
        enemy_damage_image.gameObject.SetActive(true);
        enemy_might_text.gameObject.SetActive(true);
        enemy_cunning_text.gameObject.SetActive(true);
        enemy_wisdom_text.gameObject.SetActive(true);

        Abbot.PLAYER_TURN_HUD();
        Miller.PLAYER_TURN_HUD();

        divider.gameObject.SetActive(true);

        win_lose_text.gameObject.SetActive(false);
        Continue_button.gameObject.SetActive(false);
    }

    void setEnemyTurnHUD()
    {
        Debug.Log("setEnemyTurnHUD");

        description.gameObject.SetActive(false);
        description_button.gameObject.SetActive(false);

        enemyHud.hideOptionsHUD();

        enemyImage.gameObject.SetActive(true);
        enemy_might.gameObject.SetActive(true);
        enemy_cunning.gameObject.SetActive(true);
        enemy_wisdom.gameObject.SetActive(true);
        enemy_damage_image.gameObject.SetActive(true);
        enemy_might_text.gameObject.SetActive(true);
        enemy_cunning_text.gameObject.SetActive(true);
        enemy_wisdom_text.gameObject.SetActive(true);

        Abbot.ENENMY_TURN_WON_LOST_HUD();
        Miller.ENENMY_TURN_WON_LOST_HUD();

        divider.gameObject.SetActive(true);

        win_lose_text.gameObject.SetActive(false);
        Continue_button.gameObject.SetActive(false);
    }

    void setWinHUD()
    {
        Debug.Log("setWinHUD");

        description.gameObject.SetActive(false);
        description_button.gameObject.SetActive(false);

        enemyHud.hideOptionsHUD();

        enemyImage.gameObject.SetActive(true);
        enemy_might.gameObject.SetActive(true);
        enemy_cunning.gameObject.SetActive(true);
        enemy_wisdom.gameObject.SetActive(true);
        enemy_damage_image.gameObject.SetActive(true);
        enemy_might_text.gameObject.SetActive(true);
        enemy_cunning_text.gameObject.SetActive(true);
        enemy_wisdom_text.gameObject.SetActive(true);

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

        description.gameObject.SetActive(false);
        description_button.gameObject.SetActive(false);

        enemyHud.hideOptionsHUD();

        enemyImage.gameObject.SetActive(true);
        enemy_might.gameObject.SetActive(true);
        enemy_cunning.gameObject.SetActive(true);
        enemy_wisdom.gameObject.SetActive(true);
        enemy_damage_image.gameObject.SetActive(true);
        enemy_might_text.gameObject.SetActive(true);
        enemy_cunning_text.gameObject.SetActive(true);
        enemy_wisdom_text.gameObject.SetActive(true);

        Abbot.ENENMY_TURN_WON_LOST_HUD();
        Miller.ENENMY_TURN_WON_LOST_HUD();

        divider.gameObject.SetActive(false);

        win_lose_text.gameObject.SetActive(true);
        win_lose_text.text = "YOU LOSE!";
        Continue_button.gameObject.SetActive(false);
    }

    #endregion
}
