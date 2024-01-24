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

    [SerializeField] public GameObject hud;
    [SerializeField] public GameObject roundHud;

    [SerializeField] public Die chapterDiePrefab;
    [SerializeField] public Die characterDiePrefab;

    [SerializeField] public Die chapterDie;
    [SerializeField] public Die characterDie;

    [SerializeField] private new string name;
    [SerializeField] public Text healthText;
    [SerializeField] private Text nameBattleText;

    private bool shieldActive = false;
    private bool isResting = false;
    private bool potionProtection = false;

    [SerializeField] public Image combatState;
    [SerializeField] public Button restButton;
    [SerializeField] public Button fightButton;
    [SerializeField] public Button rollCharacterDieButton;
    [SerializeField] public Button rollChapterDieButton;

    [SerializeField] public Sprite restSprite;
    [SerializeField] public Sprite fightSprite;

    [SerializeField] public Image InventorySlot1;
    [SerializeField] public Image InventorySlot2;
    [SerializeField] public Image TwoHandedSlot;

    [SerializeField] public Placeholder InventoryPlaceholder;
    public Card[] InventoryArray = new Card[2];

    public InventoryHandler ih;

    [Header("Events")]
    public GameEvent InventoryUpdate;

    public void Start()
    {
        InventoryArray[0] = InventoryPlaceholder;
        InventoryArray[1] = InventoryPlaceholder;
    }
    public abstract int getPlayerMight();
    public abstract int getPlayerCunning();
    public abstract int getPlayerWisdom();
    public abstract void getPlayerDieValue(string rollValue, EnemyBase enemy);
    public abstract ChapterDieOptions getCharacterRollResult(string rollValue);
    
    public void rollLogic(EnemyBase enemy)
    {
        StartCoroutine(rollDelay(enemy));
    }

    IEnumerator rollDelay(EnemyBase enemy)
    {
        Die characterDie = getCharacterDie();
        if (!characterDie.isRolling)
        {
            //show the dice and spawn it rolling in the air above the camera
            //characterDie.transform.position = new Vector3(0, 15, 0);
            characterDie.gameObject.SetActive(true);
            characterDie.Roll();

            while (characterDie.isRolling)
            {
                yield return null;
            }

            //when rolling is false hide the die itself
            characterDie.gameObject.SetActive(false);
            getCharacterDieButton().gameObject.SetActive(false);

            string dieValue = characterDie.dieSides.GetDieSideMatchInfo().closestMatch.ValuesAsString();
            getPlayerDieValue(dieValue, enemy);
        }
    }

    public void rollLogicNew(EnemyBase enemy, Button roll, Button next)
    {
        StartCoroutine(rollDelayNew(enemy, roll, next));
    }

    public void standardTurn(EnemyBase enemy, Button roll, Button next, Image rolledFace, Image secondRolledFace, Button face1, Button face2, ChapterLogicNew cl)
    {
        StartCoroutine(standardTurnIE(enemy, roll, next, rolledFace, secondRolledFace, face1, face2, cl));
    }

    public void reRoll(EnemyBase enemy, Button roll, Button next, Image rolledFace, Image secondRolledFace, Button face1, Button face2, ChapterLogicNew cl)
    {
        StartCoroutine(reRollIE(enemy, roll, next, rolledFace, secondRolledFace, face1, face2, cl));
    }

    IEnumerator rollDelayNew(EnemyBase enemy, Button roll, Button next)
    {
        Die characterDie = getCharacterDie();

        if (!characterDie.isRolling)
        {
            //show the dice and spawn it rolling in the air above the camera
            //characterDie.transform.position = new Vector3(0, 15, 0);
            characterDie.gameObject.SetActive(true);
            roll.interactable = false;

            characterDie.Roll();

            while (characterDie.isRolling)
            {
                yield return null;
            }

            next.interactable = true;

            //when rolling is false hide the die itself
            characterDie.gameObject.SetActive(false);

            string dieValue = characterDie.dieSides.GetDieSideMatchInfo().closestMatch.ValuesAsString();
            getPlayerDieValue(dieValue, enemy);

            enemy.enemyDead();
        }
    }

    //stores the rolled value so that it can be applied at the end of the turn
    public string initialRollValue = "";
    public string reRollValue = "";
    public string selectedValue = "";

    IEnumerator standardTurnIE(EnemyBase enemy, Button roll, Button next, Image rolledFace, Image secondRolledFace, Button face1, Button face2, ChapterLogicNew cl)
    {
        Die characterDie = getCharacterDie();

        if (!characterDie.isRolling)
        {
            //show the dice and spawn it rolling in the air above the camera
            //characterDie.transform.position = new Vector3(0, 15, 0);
            characterDie.gameObject.SetActive(true);
            roll.interactable = false;

            characterDie.Roll();

            while (characterDie.isRolling)
            {
                yield return null;
            }

            next.interactable = true;

            //when rolling is false hide the die itself
            characterDie.gameObject.SetActive(false);
            rolledFace.gameObject.SetActive(true);

            string dieValue = characterDie.dieSides.GetDieSideMatchInfo().closestMatch.ValuesAsString();
            initialRollValue = dieValue;
            rolledFace.sprite = GetCharacterDieFace(dieValue);

            if (reRollEligilble(dieValue))
            {
                roll.onClick.RemoveAllListeners();
                roll.onClick.AddListener(() => reRoll(enemy, roll, next, rolledFace, secondRolledFace, face1, face2, cl));
                roll.interactable = true;
            }
        }
    }

    private bool reRollEligilble(string rollResult)
    {
        if(this.getCharacterRollResult(rollResult) == ChapterDieOptions.WISDOM && this.inventoryContainsCard("decayed blade_0"))
        {
            return true;
        }
        if (this.getCharacterRollResult(rollResult) == ChapterDieOptions.CUNNING && this.inventoryContainsCard("Rusted flail"))
        {
            return true;
        }
        return false;
    }

    IEnumerator reRollIE(EnemyBase enemy, Button roll, Button next, Image rolledFace, Image secondRolledFace, Button face1, Button face2, ChapterLogicNew cl)
    {
        Die characterDie = getCharacterDie();

        if (!characterDie.isRolling)
        {
            next.onClick.RemoveAllListeners();
            next.onClick.AddListener(() => getPlayerDieValue(selectedValue, enemy));

            PlayerBase result = MainManager.Instance.getNextPlayer(this);
            if (result == null)
            {
                next.onClick.AddListener(() => cl.startEnemyTurnPhase());
            }
            else
            {
                next.onClick.AddListener(() => cl.setPlayerTurnPhase(result));
            }
            next.onClick.AddListener(() => enemy.enemyDead());

            //show the dice and spawn it rolling in the air above the camera
            //characterDie.transform.position = new Vector3(0, 15, 0);
            characterDie.gameObject.SetActive(true);
            roll.interactable = false;
            next.interactable = false;

            characterDie.Roll();

            while (characterDie.isRolling)
            {
                yield return null;
            }

            rerollOnClickFormatting(face1, face2, next);

            secondRolledFace.gameObject.SetActive(true);

            //when rolling is false hide the die itself
            characterDie.gameObject.SetActive(false);

            string dieValue = characterDie.dieSides.GetDieSideMatchInfo().closestMatch.ValuesAsString();
            reRollValue = dieValue;
            secondRolledFace.sprite = GetCharacterDieFace(dieValue);
        }
    }

    private void rerollOnClickFormatting(Button face1, Button face2, Button next)
    {
        face1.onClick.AddListener(() => dieOption1Selected(next));
        face2.onClick.AddListener(() => dieOption2Selected(next));
    }

    private void dieOption1Selected(Button next)
    {
        Debug.Log("dieOption1Selected");
        selectedValue = initialRollValue;

        next.interactable = true;
    }
    private void dieOption2Selected(Button next)
    {
        Debug.Log("dieOption2Selected");
        selectedValue = reRollValue;

        next.interactable = true;
    }

    public void CrackedAxeRoll(EnemyBase enemy, Button roll, Button next, Image face1, Image face2, ChapterLogicNew cl)
    {
        StartCoroutine(CrackedAxeRollIE(enemy, roll, next, face1, face2, cl));
    }

    IEnumerator CrackedAxeRollIE(EnemyBase enemy, Button roll, Button next, Image face1, Image face2, ChapterLogicNew cl)
    {
        Die characterDie = getCharacterDie();
        Die chapterDie = getChapterDie();

        if (!characterDie.isRolling && !chapterDie.isRolling)
        {
            next.onClick.RemoveAllListeners();
            next.onClick.AddListener(() => getPlayerDieValue(initialRollValue, enemy));
            next.onClick.AddListener(() => chapterDieDamage(reRollValue, enemy));

            PlayerBase result = MainManager.Instance.getNextPlayer(this);
            if (result == null)
            {
                next.onClick.AddListener(() => cl.startEnemyTurnPhase());
            }
            else
            {
                next.onClick.AddListener(() => cl.setPlayerTurnPhase(result));
            }
            next.onClick.AddListener(() => enemy.enemyDead());

            //show the dice and spawn it rolling in the air above the camera
            //characterDie.transform.position = new Vector3(0, 15, 0);
            characterDie.gameObject.SetActive(true);
            chapterDie.gameObject.SetActive(true);

            roll.interactable = false;

            characterDie.Roll();
            chapterDie.Roll();

            while (characterDie.isRolling)
            {
                yield return null;
            }

            while (chapterDie.isRolling)
            {
                yield return null;
            }

            next.interactable = true;

            //when rolling is false hide the die itself
            characterDie.gameObject.SetActive(false);
            chapterDie.gameObject.SetActive(false);

            face1.gameObject.SetActive(true);
            face2.gameObject.SetActive(true);

            string characterDieValue = characterDie.dieSides.GetDieSideMatchInfo().closestMatch.ValuesAsString();
            initialRollValue = characterDieValue;
            face1.sprite = GetCharacterDieFace(characterDieValue);

            string chapterDieValue = chapterDie.dieSides.GetDieSideMatchInfo().closestMatch.ValuesAsString();
            reRollValue = chapterDieValue;
            face2.sprite = GetChapterDieFace(chapterDieValue);

            enemy.enemyDead();
        }
    }

    public GameObject panel;
    private bool InventoryOpen = false;

    public void printInventory()
    {
        Debug.Log("SLOT 1 : " + InventoryArray[0].name);
        Debug.Log("SLOT 2 : " + InventoryArray[1].name);
    }

    public void openInventory()
    {
        Animator animator = panel.GetComponent<Animator>();
        if (!InventoryOpen)
        {
            animator.SetTrigger("Open");
            InventoryOpen = true;
        }
        else
        {
            closeInventory();
        }
    }

    public void openInventory(List<PlayerBase> Players)
    {
        Animator animator = panel.GetComponent<Animator>();
        if (!InventoryOpen)
        {
            animator.SetTrigger("Open");
            InventoryOpen = true;

            foreach (var player in Players)
            {
                if (player != this)
                {
                    player.closeInventory();
                }
            }
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
        ih.closeTradePlayersList();
        ih.showCardOptions(InventoryArray[0], this);
    }

    public void slot2Pressed()
    {
        ih.closeTradePlayersList();
        ih.showCardOptions(InventoryArray[1], this);
    }

    //only here for clarity, can just call either of the above 2 functions and will still work.
    public void TwoHandedSlotPressed()
    {
        ih.closeTradePlayersList();
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
                if (InventorySlotsFree() >= 1)
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
        Debug.Log("KARD SIZE: " + card.size);

        if (card.size == 1)
        {
            if (InventoryArray[0] == InventoryPlaceholder)
            {
                Debug.Log("MEEP");

                InventoryArray[0] = card;
                InventorySlot1.sprite = card.cardFace;
                InventorySlot1.gameObject.SetActive(true);

                InventoryUpdate.Raise();
            }
            else if (InventoryArray[1] == InventoryPlaceholder)
            {
                Debug.Log("MOOP");

                InventoryArray[1] = card;
                InventorySlot2.sprite = card.cardFace;
                InventorySlot2.gameObject.SetActive(true);

                InventoryUpdate.Raise();
            }
            else
            {
                Debug.Log("ERROR");
            }
        } else if (card.size == 2)
        {
            Debug.Log("MORP");

            InventoryArray[0] = card;
            InventoryArray[1] = card;
            TwoHandedSlot.sprite = card.cardFace;
            TwoHandedSlot.gameObject.SetActive(true);

            InventoryUpdate.Raise();
        }
        else
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

                InventoryUpdate.Raise();
            }
            else if (InventoryArray[1] == card)
            {
                InventorySlot2.gameObject.SetActive(false);
                InventoryArray[1] = InventoryPlaceholder;
                dl.discardPile.Add(card);

                InventoryUpdate.Raise();
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

                InventoryUpdate.Raise();
            }
            else
            {
                Debug.Log("Card not in inventory");
            }

        }
    
    }

    public void tradeInventoryCard(Card card)
    {
        if (card.size == 1)
        {
            if (InventoryArray[0] == card)
            {
                InventorySlot1.gameObject.SetActive(false);
                InventoryArray[0] = InventoryPlaceholder;

                InventoryUpdate.Raise();
            }
            else if (InventoryArray[1] == card)
            {
                InventorySlot2.gameObject.SetActive(false);
                InventoryArray[1] = InventoryPlaceholder;

                InventoryUpdate.Raise();
            }
            else
            {
                Debug.Log("Card not in inventory");
            }
        }
        else if (card.size == 2)
        {
            if (InventoryArray[0] == card && InventoryArray[1] == card)
            {
                TwoHandedSlot.gameObject.SetActive(false);
                InventoryArray[0] = InventoryPlaceholder;
                InventoryArray[1] = InventoryPlaceholder;

                InventoryUpdate.Raise();
            }
            else
            {
                Debug.Log("Card not in inventory");
            }

        }

    }

    public bool inventoryContainsCard(string cardName)
    {
        if (InventoryArray[0].name == cardName || InventoryArray[1].name == cardName)
        {
            return true;
        }
        return false;
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

    //if true player should take no damage on enemy turn
    public bool getPotionProtectionState()
    {
        if (potionProtection)
        {
            return true;
        }
        else return false;
    }

    public void setPotionProtectionState(bool state)
    {
        potionProtection = state;
    }

    //set a character to fight in the next player turn phase
    public void fightState()
    {
        setIsRestingState(false);
        combatState.sprite = fightSprite;
    }

    [SerializeField] public Sprite mightSprite;
    [SerializeField] public Sprite cunningSprite;
    [SerializeField] public Sprite wisdomSprite;

    //returns the face a chapter die lands on
    public ChapterDieOptions getChapterRolResult(string rollValue)
    {
        return rollValue switch
        {
            "2" or "3" => ChapterDieOptions.CUNNING,
            "6" or "5" => ChapterDieOptions.MIGHT,
            "1" or "4" => ChapterDieOptions.WISDOM,
            _ => ChapterDieOptions.FAIL,
        };
    }

    public Sprite GetChapterDieFace(string rollValue)
    {
        return rollValue switch
        {
            "2" or "3" => cunningSprite,
            "6" or "5" => mightSprite,
            "1" or "4" => wisdomSprite,
            _ => throw new Exception(),
        };
    }


    public void chapterDieDamage(string rollValue, EnemyBase enemy)
    {
        switch (rollValue)
        {
            case "2" or "3":
                enemy.reduceEnemyCunning(1);
                break;

            case "6" or "5":
                enemy.reduceEnemyMight(1);
                break;

            case "1" or "4":
                enemy.reduceEnemyWisdom(1);
                break;

            default:
                break;
        }
    }

    public void rollEnemyHealth(EnemyBase enemy)
    {
        StartCoroutine(rollEnemyHealthIEOld(enemy));
    }

    
    public void rollEnemyHealthNew(EnemyBase enemy, Button button)
    {
        StartCoroutine(rollEnemyHealthIE(enemy, button));
    }
    

    IEnumerator rollEnemyHealthIEOld(EnemyBase enemy)
    {
        Debug.Log("rollEnemyHealthIE!");

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

    public bool hasRolled = false;

    IEnumerator rollEnemyHealthIE(EnemyBase enemy, Button button)
    {
        Debug.Log("rollEnemyHealthIE!");

        if (!chapterDie.isRolling)
        {
            button.interactable = false;

            chapterDie.gameObject.SetActive(true);
            chapterDie.Roll();

            while (chapterDie.isRolling)
            {
                yield return null;
            }

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
            hasRolled = true;
            MainManager.Instance.playersRolled();
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

    public void restNew()
    {
        foreach (var player in MainManager.Instance.Players)
        {
            if(player == this)
            {
                setIsRestingState(true);
            } else
            {
                player.setIsRestingState(false);
            }
        }
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

    public void PREPERATION_HUD_NEW()
    {
        roundHud.gameObject.SetActive(true);
        combatState.gameObject.SetActive(true);
        rollChapterDieButton.gameObject.SetActive(false);
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

    #region characterDieSprites

    [SerializeField] public Sprite side_0;
    [SerializeField] public Sprite side_1;
    [SerializeField] public Sprite side_2;
    [SerializeField] public Sprite side_3;
    [SerializeField] public Sprite side_4;
    [SerializeField] public Sprite side_5;

    public Sprite GetCharacterDieFace(string rollValue)
    {
        return rollValue switch
        {
            "0" => side_0,
            "1" => side_1,
            "2" => side_2,
            "3" => side_3,
            "4" => side_4,
            "5" => side_5,
            _ => throw new System.Exception(),
        };
    }

    #endregion

}
