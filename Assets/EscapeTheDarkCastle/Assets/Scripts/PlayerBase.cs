using InnerDriveStudios.DiceCreator;
using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ChapterDieOptions { MIGHT, CUNNING, WISDOM, FAIL }

public abstract class PlayerBase : MonoBehaviour
{
    private const int MAX_HEALTH = 18;
    private const int MIN_HEALTH = 0;
    public int currentHealth = 18;

    [SerializeField] private new string name;
    [SerializeField] private Text healthText;

    [SerializeField] private Die chapterDie;
    [SerializeField] private Die characterDie;

    private bool shieldActive = false;
    private bool isResting = false;

    [SerializeField] public Image combatState;
    [SerializeField] public Button restButton;
    [SerializeField] public Button fightButton;
    [SerializeField] private Button rollCharacterDieButton;
    [SerializeField] private Button rollChapterDieButton;

    [SerializeField] public Sprite restSprite;
    [SerializeField] public Sprite fightSprite;

    public Die getChapterDie()
    {
        return chapterDie;
    }

    public Die getCharacterDie()
    {
        return characterDie;
    }

    public Button getChapterDieButton()
    {
        return rollChapterDieButton;
    }

    public Button getCharacterDieButton()
    {
        return rollCharacterDieButton;
    }

    //Increase health by passed amount
    public void IncreaseHealth(int heal)
    {
        if (currentHealth + heal < MAX_HEALTH)
        {
            SetCurrentHealth(currentHealth + heal);
        }
        else
        {
            SetCurrentHealth(18);
        }
    }

    //Reduce health by passed amount
    //Return true if player dead, false otherwise
    public bool RedcuceHealth(int damage)
    {
        if (currentHealth - damage > MIN_HEALTH)
        {
            SetCurrentHealth(currentHealth - damage);
            return false;
        }
        else
        {
            SetCurrentHealth(0);
            return true;
        }
    }

    //Update current health value and update UI element
    public void SetCurrentHealth(int health)
    {
        currentHealth = health;
        healthText.text = currentHealth.ToString();
    }

    //if true player should take no damage on enemy turn
    public bool getShieldActiveState()
    {
        if (shieldActive)
        {
            return true;
        }
        else return false;
    }

    public void setShieldActiveState(bool state)
    {
        shieldActive = state;
    }

    //if true player should recover one health next player turn phase but will not be eligible for combat
    //only one player may rest at a time
    public bool getIsRestingState()
    {
        if (isResting)
        {
            return true;
        }
        return false;
    }

    public void setIsRestingState(bool state)
    {
        isResting = state;
        if (state)
        {
            combatState.sprite = restSprite;
        }
        else
        {
            combatState.sprite = fightSprite;
        }
    }

    //set a character to fight in the next player turn phase
    public void fightState()
    {
        setIsRestingState(false);
        combatState.sprite = fightSprite;
    }

    //returns the face a chapter die lands on
    public ChapterDieOptions getChapterRolResult(string rollValue)
    {
        return rollValue switch
        {
            "0" or "5" => ChapterDieOptions.CUNNING,
            "1" or "2" => ChapterDieOptions.MIGHT,
            "3" or "4" => ChapterDieOptions.WISDOM,
            _ => ChapterDieOptions.FAIL,
        };
    }


    private int enemyMight = 0;
    private int enemyCunning = 0;
    private int enemyWisdom = 0;

    public int getRolledMight()
    {
        return enemyMight;
    }
    public int getRolledCunning()
    {
        return enemyCunning;
    }
    public int getRolledWisdom()
    {
        return enemyWisdom;
    }

    public void rollEnemyHealth()
    {
        StartCoroutine(rollEnemyHealthIE());
    }

    IEnumerator rollEnemyHealthIE()
    {
        if (!chapterDie.isRolling)
        {
            chapterDie.gameObject.SetActive(true);
            chapterDie.Roll();

            rollChapterDieButton.gameObject.SetActive(false);

            while (chapterDie.isRolling)
            {
                yield return null;
            }

            chapterDie.gameObject.SetActive(false);
            string dieValue = chapterDie.dieSides.GetDieSideMatchInfo().closestMatch.ValuesAsString();
            ChapterDieOptions rollResult = getChapterRolResult(dieValue);

            switch (rollResult)
            {
                case ChapterDieOptions.MIGHT:
                    enemyMight++;
                    break;

                case ChapterDieOptions.CUNNING:
                    enemyCunning++;
                    break;

                case ChapterDieOptions.WISDOM:
                    enemyWisdom++;
                    break;

                default:
                    break;
            }

            chapterDie.gameObject.SetActive(false);
        }
    }

    public void rest(PlayerBase player)
    {
        if (player.getIsRestingState())
        {
            player.setIsRestingState(false);
        }
        setIsRestingState(true);
    }

    #region HUD_methods
    public void SET_ENEMY_HEALTH_HUD()
    {
        rollChapterDieButton.gameObject.SetActive(true);
        rollCharacterDieButton.gameObject.SetActive(false);
        combatState.gameObject.SetActive(false);
        restButton.gameObject.SetActive(false);
        fightButton.gameObject.SetActive(false);
    }

    public void PREPERATION_HUD()
    {
        rollChapterDieButton.gameObject.SetActive(false);
        rollCharacterDieButton.gameObject.SetActive(false);
        combatState.gameObject.SetActive(true);
        restButton.gameObject.SetActive(true);
        fightButton.gameObject.SetActive(true);
    }

    public void PLAYER_TURN_HUD()
    {
        rollChapterDieButton.gameObject.SetActive(false);
        rollCharacterDieButton.gameObject.SetActive(true);
        combatState.gameObject.SetActive(true);
        restButton.gameObject.SetActive(false);
        fightButton.gameObject.SetActive(false);    
    }

    public void ENENMY_TURN_WON_LOST_HUD()
    {
        rollChapterDieButton.gameObject.SetActive(false);
        rollCharacterDieButton.gameObject.SetActive(false);
        combatState.gameObject.SetActive(true);
        restButton.gameObject.SetActive(false);
        fightButton.gameObject.SetActive(false);
    }

    #endregion

    public void HIDE_CHAPTER_DICE_BUTTON()
    {
        rollChapterDieButton.gameObject.SetActive(false);
    }

    public void HIDE_CHARACTER_DICE_BUTTON()
    {
        rollCharacterDieButton.gameObject.SetActive(false);
    }

    public void SHOW_CHARACTER_DICE_BUTTON()
    {
        rollCharacterDieButton.gameObject.SetActive(true);
    }


}
