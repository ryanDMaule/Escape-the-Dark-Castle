using InnerDriveStudios.DiceCreator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ChapterDieOptions { MIGHT, CUNNING, WISDOM, FAIL }

public abstract class PlayerBase : MonoBehaviour
{
    #region globalVariables

    [Header("Health values")]
    public int currentHealth = 9;
    private const int MAX_HEALTH = 18;
    private const int MIN_HEALTH = 0;

    [Header("YOU stuffs")]
    public bool YOU = false;
    [SerializeField] public GameObject YOUHudIcon;
    [SerializeField] public GameObject YOUSelectorItem;

    [Header("HUDs")]
    [SerializeField] public GameObject hud;
    [SerializeField] public GameObject roundHud;

    [Header("Dice")]
    [SerializeField] public Die chapterDiePrefab;
    [SerializeField] public Die characterDiePrefab;

    [SerializeField] public Die chapterDie;
    [SerializeField] public Die characterDie;

    [Header("Die side sprites")]
    [SerializeField] public Sprite mightSprite;
    [SerializeField] public Sprite cunningSprite;
    [SerializeField] public Sprite wisdomSprite;

    [SerializeField] public Sprite side_0;
    [SerializeField] public Sprite side_1;
    [SerializeField] public Sprite side_2;
    [SerializeField] public Sprite side_3;
    [SerializeField] public Sprite side_4;
    [SerializeField] public Sprite side_5;

    [Header("Roll values")]
    public bool hasRolled = false;
    public bool hasReRolled = false;
    public string initialRollValue = "";
    public string reRollValue = "";
    public string selectedValue = "";

    [Header("Name values")]
    [SerializeField] private new string name;
    [SerializeField] public Text healthText;
    [SerializeField] private Text nameBattleText;

    private bool shieldActive = false;
    private bool isResting = false;
    private bool potionProtection = false;

    [Header("Combat state assets")]
    [SerializeField] public Image combatState;
    [SerializeField] public Button restButton;
    [SerializeField] public Button fightButton;
    [SerializeField] public Sprite restSprite;
    [SerializeField] public Sprite fightSprite;

    [Header("Roll buttons")]
    [SerializeField] public Button rollCharacterDieButton;
    [SerializeField] public Button rollChapterDieButton;

    [Header("Inventory")]
    [SerializeField] public Image InventorySlot1;
    [SerializeField] public Image InventorySlot2;
    [SerializeField] public Image TwoHandedSlot;

    public GameObject panel;

    [SerializeField] public Placeholder InventoryPlaceholder;
    public Card[] InventoryArray = new Card[2];

    public InventoryHandler ih;

    [Header("Events")]
    public GameEvent InventoryUpdate;
    public GameEvent PlayerDead;
    public GameEvent RollStarted;
    public GameEvent RollFinished;

    #endregion

    public void Start()
    {
        InventoryArray[0] = InventoryPlaceholder;
        InventoryArray[1] = InventoryPlaceholder;
    }

    #region abstractMethods
    public abstract int getPlayerMight();
    public abstract int getPlayerCunning();
    public abstract int getPlayerWisdom();
    public abstract void getPlayerDieValue(string rollValue, EnemyBase enemy);
    public abstract ChapterDieOptions getCharacterRollResult(string rollValue);

    #endregion

    #region rollLogic
    public void standardTurnSimplified(Button roll)
    {
        StartCoroutine(standardTurnSimplifiedIE(roll));
    }

    IEnumerator standardTurnSimplifiedIE(Button roll)
    {
        Die characterDie = getCharacterDie();

        if (!characterDie.isRolling)
        {
            //show the dice and spawn it rolling in the air above the camera
            //characterDie.transform.position = new Vector3(0, 15, 0);
            characterDie.gameObject.SetActive(true);
            roll.interactable = false;

            characterDie.Roll();
            RollStarted.Raise();

            while (characterDie.isRolling)
            {
                yield return null;
            }

            //when rolling is false hide the die itself
            characterDie.gameObject.SetActive(false);

            string dieValue = characterDie.dieSides.GetDieSideMatchInfo().closestMatch.ValuesAsString();
            initialRollValue = dieValue;

            if (reRollEligilble(dieValue))
            {
                roll.onClick.RemoveAllListeners();
                roll.onClick.AddListener(() => reRollSimplified(roll));
                roll.interactable = true;
            }

            RollFinished.Raise();
        }
    }

    private bool reRollEligilble(string rollResult)
    {
        if (this.getCharacterRollResult(rollResult) == ChapterDieOptions.WISDOM && this.inventoryContainsCard("decayed blade_0"))
        {
            return true;
        }
        if (this.getCharacterRollResult(rollResult) == ChapterDieOptions.CUNNING && this.inventoryContainsCard("Rusted flail"))
        {
            return true;
        }
        return false;
    }
    public void reRollSimplified(Button roll)
    {
        StartCoroutine(reRollIESimplified(roll));
    }

    IEnumerator reRollIESimplified(Button roll)
    {
        Die characterDie = getCharacterDie();

        if (!characterDie.isRolling)
        {
            hasReRolled = true;

            //show the dice and spawn it rolling in the air above the camera
            //characterDie.transform.position = new Vector3(0, 15, 0);
            characterDie.gameObject.SetActive(true);
            roll.interactable = false;

            characterDie.Roll();
            RollStarted.Raise();

            while (characterDie.isRolling)
            {
                yield return null;
            }
            //when rolling is false hide the die itself
            characterDie.gameObject.SetActive(false);

            string dieValue = characterDie.dieSides.GetDieSideMatchInfo().closestMatch.ValuesAsString();
            reRollValue = dieValue;

            RollFinished.Raise();
        }
    }

    public void rerollOnClickFormatting(Button face1, Button face2, Button next)
    {
        face1.onClick.AddListener(() => dieOption1Selected(face1, face2, next));
        face2.onClick.AddListener(() => dieOption2Selected(face1, face2, next));
    }

    private void dieOption1Selected(Button face1, Button face2, Button next)
    {
        Debug.Log("dieOption1Selected");

        if(selectedValue != initialRollValue)
        {
            Animator animator1 = face1.GetComponent<Animator>();
            Animator animator2 = face2.GetComponent<Animator>();

            animator1.SetTrigger("Expand");
            if (selectedValue == reRollValue)
            {
                animator2.SetTrigger("Contract");
            }

            selectedValue = initialRollValue;

            next.interactable = true;
        }
    }
    private void dieOption2Selected(Button face1, Button face2, Button next)
    {
        Debug.Log("dieOption2Selected");

        if(selectedValue != reRollValue)
        {
            Animator animator1 = face1.GetComponent<Animator>();
            Animator animator2 = face2.GetComponent<Animator>();

            animator2.SetTrigger("Expand");
            if (selectedValue == initialRollValue)
            {
                animator1.SetTrigger("Contract");
            }

            selectedValue = reRollValue;

            next.interactable = true;
        }
    }

    public void CrackedAxeRollSimplified(Button roll)
    {
        StartCoroutine(CrackedAxeRollIESimplified(roll));
    }

    IEnumerator CrackedAxeRollIESimplified(Button roll)
    {
        Die characterDie = getCharacterDie();
        Die chapterDie = getChapterDie();

        if (!characterDie.isRolling && !chapterDie.isRolling)
        {
            //show the dice and spawn it rolling in the air above the camera
            //characterDie.transform.position = new Vector3(0, 15, 0);
            characterDie.gameObject.SetActive(true);
            chapterDie.gameObject.SetActive(true);

            //set the roll button uninteractable while its rolling
            roll.interactable = false;

            //roll both dice
            characterDie.Roll();
            chapterDie.Roll();

            //raise roll started, this is to show the fast forward button
            RollStarted.Raise();

            //WAIT FOR BOTH DICE TO STOP ROLLING
            while (characterDie.isRolling)
            {
                yield return null;
            }

            while (chapterDie.isRolling)
            {
                yield return null;
            }

            //when rolling is false hide the die itself
            characterDie.gameObject.SetActive(false);
            chapterDie.gameObject.SetActive(false);

            string characterDieValue = characterDie.dieSides.GetDieSideMatchInfo().closestMatch.ValuesAsString();
            initialRollValue = characterDieValue;

            string chapterDieValue = chapterDie.dieSides.GetDieSideMatchInfo().closestMatch.ValuesAsString();
            reRollValue = chapterDieValue;

            //Roll finished, hide the fast forward button
            RollFinished.Raise();
        }
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

    public void rollEnemyHealthNew(EnemyBase enemy, Button button)
    {
        StartCoroutine(rollEnemyHealthIE(enemy, button));
    }

    IEnumerator rollEnemyHealthIE(EnemyBase enemy, Button button)
    {
        Debug.Log("rollEnemyHealthIE!");

        if (!chapterDie.isRolling)
        {
            button.interactable = false;

            chapterDie.gameObject.SetActive(true);
            chapterDie.Roll();
            RollStarted.Raise();

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

    #endregion

    #region inventoryLogic

    private bool InventoryOpen = false;

    public void printInventory()
    {
        Debug.Log("SLOT 1 : " + InventoryArray[0].name);
        Debug.Log("SLOT 2 : " + InventoryArray[1].name);
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
        }
        else if (card.size == 2)
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
        }
        else if (card.size == 2)
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

    #endregion

    #region dieGetters

    public Die getChapterDie()
    {
        return chapterDie;
    }

    public Die getCharacterDie()
    {
        return characterDie;
    }

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

    #region playerState

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
            PlayerDead.Raise();
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

    public void restNew()
    {
        foreach (var player in MainManager.Instance.Players)
        {
            if (player == this)
            {
                setIsRestingState(true);
            }
            else
            {
                player.setIsRestingState(false);
            }
        }
    }

    public void setYouTrue(Button button)
    {
        if (!YOU)
        {
            //Set You to true
            YOU = true;

            button.interactable = true;

            //apply YOU icon
            YOUHudIcon.gameObject.SetActive(true);

            //sort out animation stuff
            Animator animator = YOUSelectorItem.GetComponent<Animator>();
            animator.SetTrigger("Expand");

            //revoke YOU status from any other players
            foreach (var player in MainManager.Instance.Players)
            {
                if(player != this)
                {
                    player.setYouFalseAnim();
                }
            }
        }
    }

    public void setYouFalseAnim()
    {
        if (YOU)
        {
            //Set You to false
            YOU = false;

            //remove YOU icon
            YOUHudIcon.gameObject.SetActive(false);

            //sort out animation stuff
            Animator animator = YOUSelectorItem.GetComponent<Animator>();
            animator.SetTrigger("Contract");
        }
    }

    public void setYouFalse()
    {
        if (YOU)
        {
            //Set You to false
            YOU = false;

            //remove YOU icon
            YOUHudIcon.gameObject.SetActive(false);
        }
    }

    #endregion

    #region HUD_methods
    public void PREPERATION_HUD_NEW()
    {
        roundHud.gameObject.SetActive(true);
        combatState.gameObject.SetActive(true);
        rollChapterDieButton.gameObject.SetActive(false);
        restButton.gameObject.SetActive(true);
        fightButton.gameObject.SetActive(true);
    }

    #endregion

    #region helperMethods

    public void determineEnemyDamage(ChapterDieOptions damageType, EnemyBase enemy, PlayerBase player)
    {
        bool doubleDamage = player.inventoryContainsCard("the replication stones_0");
        switch (damageType)
        {
            case ChapterDieOptions.MIGHT:
                if (doubleDamage)
                {
                    enemy.reduceEnemyMight(2);
                }
                else
                {
                    enemy.reduceEnemyMight(1);
                }
                break;

            case ChapterDieOptions.CUNNING:
                if (doubleDamage)
                {
                    enemy.reduceEnemyCunning(2);
                }
                else
                {
                    enemy.reduceEnemyCunning(1);
                }
                break;

            case ChapterDieOptions.WISDOM:
                if (doubleDamage)
                {
                    enemy.reduceEnemyWisdom(2);
                }
                else
                {
                    enemy.reduceEnemyWisdom(1);
                }
                break;

            default:
                break;
        }
    }

    #endregion

}
