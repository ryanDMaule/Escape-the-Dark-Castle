using System.Collections;
using System.Collections.Generic;
using InnerDriveStudios.DiceCreator;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { SET_ENEMY_HEALTH, PREPERATION, PLAYER_TURN, ENEMY_TURN, WON, LOST }


public class ChapterLogic : MonoBehaviour
{
    #region globalVariables

    [SerializeField] public Image enemyImage;
    [SerializeField] public Image enemy_might;
    [SerializeField] public Image enemy_cunning;
    [SerializeField] public Image enemy_wisdom;
    [SerializeField] public Image enemy_damage_image;

    [SerializeField] public Text enemy_might_text;
    [SerializeField] public Text enemy_cunning_text;
    [SerializeField] public Text enemy_wisdom_text;

    [SerializeField] public Text win_lose_text;
    [SerializeField] public Button Continue_button;

    [SerializeField] public int enemy_damage;
    [SerializeField] public Abbot Abbot;
    [SerializeField] public Miller Miller;

    [SerializeField] private Enemy enemy;

    public BattleState state;

    #endregion

    void Start()
    {
        state = BattleState.SET_ENEMY_HEALTH;
        setEnemyHealthPhase();
    }


    void setEnemyHealthPhase()
    {
        setEnemyHUD();
        enemy.setEnemyMight(2);

        //both player to roll their chapter die
        StartCoroutine(setEnemyhealthIE());
    }

    IEnumerator setEnemyhealthIE()
    {
        while (Abbot.getChapterDie().gameObject.activeSelf)
        {
            yield return null;
        }

        while (Miller.getChapterDie().gameObject.activeSelf)
        {
            yield return null;
        }

        enemy.setEnemyMight(Abbot.getRolledMight() + Miller.getRolledMight());
        enemy.setEnemyCunning(Abbot.getRolledCunning() + Miller.getRolledCunning());
        enemy.setEnemyWisdom(Abbot.getRolledWisdom() + Miller.getRolledWisdom());

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

        enemy.reduceEnemyMight(Abbot.getMightDamage() + Miller.getMightDamage());
        enemy.reduceEnemyCunning(Abbot.getCunningDamage() + Miller.getCunningDamage());
        enemy.reduceEnemyWisdom(Abbot.getWisdomDamage() + Miller.getWisdomDamage());

        Debug.Log("CONTINUE");


        if (enemy.enemyDead())
        {
            yield return new WaitForSeconds(2f);
            setWinHUD();
            Debug.Log("setWinHUD");
        }
        else
        {
            Abbot.mightDamage = 0;
            Abbot.cunningDamage = 0;
            Abbot.wisdomDamage = 0;

            Miller.mightDamage = 0;
            Miller.cunningDamage = 0;
            Miller.wisdomDamage = 0;


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
            abbotDead = Abbot.RedcuceHealth(1);
        }
        if (!Miller.getShieldActiveState() && !Miller.getIsRestingState())
        {
            MillerDead = Miller.RedcuceHealth(1);
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
    void setEnemyHUD()
    {
        Debug.Log("setEnemyHUD");

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
    }

    void setPreperationHUD()
    {
        Debug.Log("setPreperationHUD");

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

        Abbot.setShieldActiveState(false);
        Miller.setShieldActiveState(false);
    }

    void setPlayerTurnHUD()
    {
        Debug.Log("setPlayerTurnHUD");

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

        win_lose_text.gameObject.SetActive(false);
        Continue_button.gameObject.SetActive(false);
    }

    void setEnemyTurnHUD()
    {
        Debug.Log("setEnemyTurnHUD");

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

        win_lose_text.gameObject.SetActive(false);
        Continue_button.gameObject.SetActive(false);
    }

    void setWinHUD()
    {
        Debug.Log("setWinHUD");

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

        win_lose_text.gameObject.SetActive(true);
        win_lose_text.text = "YOU WIN!";
        Continue_button.gameObject.SetActive(false);
    }

    void setLoseHUD()
    {
        Debug.Log("setLoseHUD");

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

        win_lose_text.gameObject.SetActive(true);
        win_lose_text.text = "YOU LOSE!";
        Continue_button.gameObject.SetActive(false);
    }

    #endregion
}
