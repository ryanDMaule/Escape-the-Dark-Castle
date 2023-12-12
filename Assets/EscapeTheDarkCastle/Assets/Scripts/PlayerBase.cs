using InnerDriveStudios.DiceCreator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public enum ChapterDieOptions { MIGHT, CUNNING, WISDOM, FAIL }

public abstract class PlayerBase : MonoBehaviour
{
    private const int MAX_HEALTH = 18;
    private const int MIN_HEALTH = 0;
    public int currentHealth = 18;

    [SerializeField] private new string name;
    [SerializeField] private Text healthText;
    [SerializeField] private Text nameBattleText;

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

    [SerializeField] public Image InventorySlot1;
    [SerializeField] public Image InventorySlot2;
    [SerializeField] public Image TwoHandedSlot;

    [SerializeField] public Placeholder InventoryPlaceholder;
    public Card[] InventoryArray = new Card[2];

    public InventoryHandler ih;

    public void Start()
    {
        InventoryArray[0] = InventoryPlaceholder;
        InventoryArray[1] = InventoryPlaceholder;
    }

    public GameObject panel;
    private bool InventoryOpen = false;
    public void openInventory(PlayerBase player1)
    {
        Animator animator = panel.GetComponent<Animator>();
        if (!InventoryOpen)
        {
            animator.SetTrigger("Open");
            InventoryOpen = true;

            player1.closeInventory();
        } else
        {
            closeInventory();
        }
    }

    public void openInventory(PlayerBase player1, PlayerBase player2)
    {
        Animator animator = panel.GetComponent<Animator>();
        if (!InventoryOpen)
        {
            animator.SetTrigger("Open");
            InventoryOpen = true;

            player1.closeInventory();
            player2.closeInventory();
        }
        else
        {
            closeInventory();
        }
    }

    public void openInventory(PlayerBase player1, PlayerBase player2, PlayerBase player3)
    {
        Animator animator = panel.GetComponent<Animator>();
        if (!InventoryOpen)
        {
            animator.SetTrigger("Open");
            InventoryOpen = true;

            player1.closeInventory();
            player2.closeInventory();
            player3.closeInventory();
        }
        else
        {
            closeInventory();
        }
    }

    public void closeInventory()
    {
        if (InventoryOpen)
        {
            Animator animator = panel.GetComponent<Animator>();

            animator.SetTrigger("Close");
            InventoryOpen = false;
        }
    }

    public void slot1Pressed()
    {
        ih.showCardOptions(InventoryArray[0], this);
    }

    public void slot2Pressed()
    {
        ih.showCardOptions(InventoryArray[1], this);
    }

    //only here for clarity, can just call either of the above 2 functions and will still work.
    public void TwoHandedSlotPressed()
    {
        ih.showCardOptions(InventoryArray[0], this);
    }

    public int InventorySlotsFree()
    {
        int slotsFree = 0;
        if (InventoryArray[0] == InventoryPlaceholder)
        {
            slotsFree++;
        }
        if (InventoryArray[1] == InventoryPlaceholder)
        {
            slotsFree++;
        }
        return slotsFree;
    }

    public void addInventoryItem(DeckLogic dl)
    {
        int cardSize = dl.drawnCard.size;
        switch (cardSize)
        {
            case 1:
                if(InventorySlotsFree() >= 1)
                {
                    assignInventoryCard(dl.drawnCard);
                    dl.hide();
                }
                break;

            case 2:
                if (InventorySlotsFree() >= 2)
                {
                    assignInventoryCard(dl.drawnCard);
                    dl.hide();
                }
                break;

            default:
                break;
        }
    }

    public void assignInventoryCard(Card card)
    {
        if (card.size == 1)
        {
            if (InventoryArray[0] == InventoryPlaceholder)
            {
                InventoryArray[0] = card;
                InventorySlot1.sprite = card.cardFace;
                InventorySlot1.gameObject.SetActive(true);
            }
            else if (InventoryArray[1] == InventoryPlaceholder)
            {
                InventoryArray[1] = card;
                InventorySlot2.sprite = card.cardFace;
                InventorySlot2.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("ERROR");
            }
        } else if (card.size == 2)
        {
            InventoryArray[0] = card;
            InventoryArray[1] = card;
            TwoHandedSlot.sprite = card.cardFace;
            TwoHandedSlot.gameObject.SetActive(true);
        } else
        {
            Debug.Log("UNHANDLED CARD SIZE");
        }

    }

    public void removeInventoyCard(Card card, DeckLogic dl)
    {
        if (card.size == 1)
        {
            if (InventoryArray[0] == card)
            {
                InventorySlot1.gameObject.SetActive(false);
                InventoryArray[0] = InventoryPlaceholder;
                dl.discardPile.Add(card);
            }
            else if (InventoryArray[1] == card)
            {
                InventorySlot2.gameObject.SetActive(false);
                InventoryArray[1] = InventoryPlaceholder;
                dl.discardPile.Add(card);
            }
            else
            {
                Debug.Log("Card not in inventory");
            }
        } else if (card.size == 2)
        {
            if (InventoryArray[0] == card && InventoryArray[1] == card)
            {
                TwoHandedSlot.gameObject.SetActive(false);
                InventoryArray[0] = InventoryPlaceholder;
                InventoryArray[1] = InventoryPlaceholder;
                dl.discardPile.Add(card);
            }
            else
            {
                Debug.Log("Card not in inventory");
            }

        }
    
    }

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

    public void rollEnemyHealth(EnemyBase enemy)
    {
        StartCoroutine(rollEnemyHealthIE(enemy));
    }

    IEnumerator rollEnemyHealthIE(EnemyBase enemy)
    {
        if (!chapterDie.isRolling)
        {
            chapterDie.gameObject.SetActive(true);
            chapterDie.Roll();

            while (chapterDie.isRolling)
            {
                yield return null;
            }

            chapterDie.gameObject.SetActive(false);
            rollChapterDieButton.gameObject.SetActive(false);

            string dieValue = chapterDie.dieSides.GetDieSideMatchInfo().closestMatch.ValuesAsString();
            ChapterDieOptions rollResult = getChapterRolResult(dieValue);

            switch (rollResult)
            {
                case ChapterDieOptions.MIGHT:
                    enemy.setEnemyMight(1);
                    break;

                case ChapterDieOptions.CUNNING:
                    enemy.setEnemyCunning(1);
                    break;

                case ChapterDieOptions.WISDOM:
                    enemy.setEnemyWisdom(1);
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

    public void SET_DESCRIPTION_HUD()
    {
        rollChapterDieButton.gameObject.SetActive(false);
        rollCharacterDieButton.gameObject.SetActive(false);
        combatState.gameObject.SetActive(false);
        restButton.gameObject.SetActive(false);
        fightButton.gameObject.SetActive(false);
        nameBattleText.gameObject.SetActive(false);
    }
    public void SET_ENEMY_HEALTH_HUD()
    {
        rollChapterDieButton.gameObject.SetActive(true);
        rollCharacterDieButton.gameObject.SetActive(false);
        combatState.gameObject.SetActive(false);
        restButton.gameObject.SetActive(false);
        fightButton.gameObject.SetActive(false);
        nameBattleText.gameObject.SetActive(true);
    }

    public void PREPERATION_HUD()
    {
        rollChapterDieButton.gameObject.SetActive(false);
        rollCharacterDieButton.gameObject.SetActive(false);
        combatState.gameObject.SetActive(true);
        restButton.gameObject.SetActive(true);
        fightButton.gameObject.SetActive(true);
        nameBattleText.gameObject.SetActive(true);
    }

    public void PLAYER_TURN_HUD()
    {
        rollChapterDieButton.gameObject.SetActive(false);
        rollCharacterDieButton.gameObject.SetActive(true);
        combatState.gameObject.SetActive(true);
        restButton.gameObject.SetActive(false);
        fightButton.gameObject.SetActive(false);
        nameBattleText.gameObject.SetActive(true);
    }

    public void ENENMY_TURN_WON_LOST_HUD()
    {
        rollChapterDieButton.gameObject.SetActive(false);
        rollCharacterDieButton.gameObject.SetActive(false);
        combatState.gameObject.SetActive(true);
        restButton.gameObject.SetActive(false);
        fightButton.gameObject.SetActive(false);
        nameBattleText.gameObject.SetActive(true);
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
