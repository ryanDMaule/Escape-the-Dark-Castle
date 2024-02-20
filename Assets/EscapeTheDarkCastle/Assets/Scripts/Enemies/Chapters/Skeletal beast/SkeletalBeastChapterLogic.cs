using InnerDriveStudios.DiceCreator;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkeletalBeastChapterLogic : ChapterLogicBase
{
    #region globalVariables
    [Header("Game objects")]
    [SerializeField] public GameObject playersCombinedHUD;
    [SerializeField] public GameObject playerTurnHUD;
    [SerializeField] public TextMeshProUGUI playerTurnName;
    [SerializeField] public Button playerTurnRoll;
    [SerializeField] public Button playerTurnEndTurn;
    [SerializeField] public Button rollChapterDieButton;

    [SerializeField] public Image playerTurnInitialDieImage;
    [SerializeField] public Image playerTurnSecondDieImage;
    [SerializeField] public Button initialRollButton;
    [SerializeField] public Button rerollRollButton;

    [SerializeField] public GameObject winHUD;
    [SerializeField] public Button Continue_button;

    [Header("Other")]
    public Scenes scenes;

    public SkeletalBeastFormatChapter fc;

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

    //this is called first as it just starts the process with the first character in the players list
    public void startPlayerTurnPhase()
    {
        foreach (var p in MainManager.Instance.Players)
        {
            if (p.getIsRestingState())
            {
                p.IncreaseHealth(1);
            }
        }

        PlayerBase player = MainManager.Instance.Players[0];
        if (player.getIsRestingState())
        {
            Debug.Log("PLAYER RESTING SKIP");

            PlayerBase result = MainManager.Instance.getNextPlayer(player);
            if (result == null)
            {
                startEnemyTurnPhase();
            }
            else
            {
                setPlayerTurnPhase(result);
            }
        }
        else
        {
            setPlayerTurnHUD();
            formatPlayerTurnHUDNew(MainManager.Instance.Players[0]);
        }
    }

    //this is called after the above duplicate function but this takes the previous player as a paramter to get the next player 
    public void setPlayerTurnPhase(PlayerBase player)
    {
        if (player.getIsRestingState())
        {
            Debug.Log("PLAYER RESTING SKIP");

            PlayerBase result = MainManager.Instance.getNextPlayer(player);
            if (result == null)
            {
                startEnemyTurnPhase();
            }
            else
            {
                setPlayerTurnPhase(result);
            }
        }
        else
        {
            setPlayerTurnHUD();
            formatPlayerTurnHUDNew(player);
        }
    }

    public void startEnemyTurnPhase()
    {
        if (!enemyBase.enemyDead())
        {
            state = BattleState.ENEMY_TURN;

            SoundFXPlayer soundFX = FindFirstObjectByType<SoundFXPlayer>();
            soundFX.PlayDamageTaken();

            bool playerDead = false;

            setEnemyTurnHUD();
            foreach (var player in MainManager.Instance.Players)
            {
                if (!player.getShieldActiveState() && !player.getIsRestingState() && !player.getPotionProtectionState())
                {
                    //TODO : update cards to have an ENUM title
                    if (player.inventoryContainsCard("rotten shield_0"))
                    {
                        int damage = enemyBase.getDamage();
                        if (damage > 1)
                        {
                            damage--;
                        }
                        playerDead = player.RedcuceHealth(damage);
                    }
                    else
                    {
                        playerDead = player.RedcuceHealth(enemyBase.getDamage());
                    }
                }
                player.setShieldActiveState(false);
                player.setPotionProtectionState(false);
            }

            setPreperationHUD();
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
        playerTurnHUD.SetActive(false);

        winHUD.gameObject.SetActive(false);
        Continue_button.gameObject.SetActive(false);
    }

    public override void setCombatOptionsHUD()
    {
        base.setCombatOptionsHUD();

        playersCombinedHUD.SetActive(false);
        playerTurnHUD.SetActive(false);

        winHUD.gameObject.SetActive(false);
        Continue_button.gameObject.SetActive(false);
    }

    void setEnemyHUD()
    {
        Debug.Log("setEnemyHUD");

        enemyBase.HIDE_DESCRIPTION();
        enemyBase.hideOptionsHUD();
        enemyBase.SET_ENEMY_ASSETS_VISIBLE();

        playersCombinedHUD.SetActive(true);
        playerTurnHUD.SetActive(false);

        winHUD.gameObject.SetActive(false);
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
        foreach (var player in MainManager.Instance.Players)
        {
            player.PREPERATION_HUD_NEW();
        }
        playerTurnHUD.SetActive(false);

        winHUD.gameObject.SetActive(false);
        Continue_button.gameObject.SetActive(true);
    }
    void setPlayerTurnHUD()
    {
        Debug.Log("setPlayerTurnHUD");

        enemyBase.HIDE_DESCRIPTION();
        enemyBase.hideOptionsHUD();
        enemyBase.SET_ENEMY_ASSETS_VISIBLE();

        playersCombinedHUD.SetActive(false);
        playerTurnHUD.SetActive(true);

        winHUD.gameObject.SetActive(false);
        Continue_button.gameObject.SetActive(false);
    }

    public void standardTurnSimplified(Button roll, PlayerBase player)
    {
        StartCoroutine(standardTurnSimplifiedIE(roll, player));
    }

    IEnumerator standardTurnSimplifiedIE(Button roll, PlayerBase player)
    {
        Die characterDie = player.getCharacterDie();

        if (!characterDie.isRolling)
        {
            //show the dice and spawn it rolling in the air above the camera
            //characterDie.transform.position = new Vector3(0, 15, 0);
            characterDie.gameObject.SetActive(true);
            roll.interactable = false;

            characterDie.Roll();
            player.RollStarted.Raise();

            while (characterDie.isRolling)
            {
                yield return null;
            }

            //when rolling is false hide the die itself
            characterDie.gameObject.SetActive(false);

            string dieValue = characterDie.dieSides.GetDieSideMatchInfo().closestMatch.ValuesAsString();
            player.initialRollValue = dieValue;

            if (player.reRollEligilble(dieValue))
            {
                roll.onClick.RemoveAllListeners();
                roll.onClick.AddListener(() => reRollSimplified(roll, player));
                roll.onClick.AddListener(() => playerTurnEndTurn.gameObject.SetActive(false));
                roll.onClick.AddListener(() => rollChapterDieButton.gameObject.SetActive(false));

                roll.interactable = true;
            }

            player.RollFinished.Raise();
        }
    }

    public void reRollSimplified(Button roll, PlayerBase player)
    {
        StartCoroutine(reRollIESimplified(roll, player));
    }

    IEnumerator reRollIESimplified(Button roll, PlayerBase player)
    {
        Die characterDie = player.getCharacterDie();

        if (!characterDie.isRolling)
        {
            player.hasReRolled = true;

            //show the dice and spawn it rolling in the air above the camera
            //characterDie.transform.position = new Vector3(0, 15, 0);
            characterDie.gameObject.SetActive(true);
            roll.interactable = false;

            characterDie.Roll();
            player.RollStarted.Raise();

            while (characterDie.isRolling)
            {
                yield return null;
            }
            //when rolling is false hide the die itself
            characterDie.gameObject.SetActive(false);

            string dieValue = characterDie.dieSides.GetDieSideMatchInfo().closestMatch.ValuesAsString();
            player.reRollValue = dieValue;

            player.RollFinished.Raise();
        }
    }

    private void formatPlayerTurnHUDNew(PlayerBase player)
    {
        MainManager.Instance.playerTurn = player;
        state = BattleState.PLAYER_TURN;

        //SET NAME
        playerTurnName.text = player.name;

        //ROLL BUTTON
        playerTurnRoll.onClick.RemoveAllListeners();
        if (player.inventoryContainsCard("Cracked axe"))
        {
            playerTurnRoll.onClick.AddListener(() => player.CrackedAxeRollSimplified(playerTurnRoll));
        }
        else
        {
            playerTurnRoll.onClick.AddListener(() => standardTurnSimplified(playerTurnRoll, player));
        }

        //ROLL CHAPTER DIE BUTTON

        //roll players chapter die
        rollChapterDieButton.onClick.RemoveAllListeners();
        rollChapterDieButton.onClick.AddListener(() => rollChapterLogic(enemyBase, player));

        //END TURN BUTTON
        playerTurnEndTurn.onClick.RemoveAllListeners();

        //deals the damage the player rolled
        playerTurnEndTurn.onClick.AddListener(() => player.getPlayerDieValue(player.initialRollValue, enemyBase));

        //determines whos turns next
        PlayerBase result = MainManager.Instance.getNextPlayer(player);
        if (result == null)
        {
            playerTurnEndTurn.onClick.AddListener(() => startEnemyTurnPhase());
        }
        else
        {
            playerTurnEndTurn.onClick.AddListener(() => setPlayerTurnPhase(result));
        }
        //checks if the enemy is dead
        playerTurnEndTurn.onClick.AddListener(() => enemyBase.enemyDead());

        //CLEAN UP
        playerTurnInitialDieImage.gameObject.SetActive(false);
        playerTurnSecondDieImage.gameObject.SetActive(false);

        player.initialRollValue = "";
        player.reRollValue = "";
        player.selectedValue = "";

        player.hasReRolled = false;

        playerTurnRoll.interactable = true;
        playerTurnEndTurn.interactable = false;
    }

    //make a function where it listens for if a player re rolls and updates the conttinue button to check for the selectedface variable

    void setEnemyTurnHUD()
    {
        Debug.Log("setEnemyTurnHUD");

        enemyBase.HIDE_DESCRIPTION();
        enemyBase.hideOptionsHUD();
        enemyBase.SET_ENEMY_ASSETS_VISIBLE();

        playersCombinedHUD.SetActive(false);
        playerTurnHUD.SetActive(false);

        winHUD.gameObject.SetActive(false);
        Continue_button.gameObject.SetActive(false);
    }

    public override void setWinHUD()
    {
        base.setWinHUD();

        playersCombinedHUD.SetActive(false);
        playerTurnHUD.SetActive(false);

        winHUD.gameObject.SetActive(true);
        Continue_button.gameObject.SetActive(false);
    }

    #endregion

    public void formatNextTurn(PlayerBase player, Button nextButton)
    {
        //check if there are more players to roll, format the next button dependant on the result
        PlayerBase result = MainManager.Instance.getNextPlayer(player);
        if (result == null)
        {
            nextButton.onClick.AddListener(() => startEnemyTurnPhase());
        }
        else
        {
            nextButton.onClick.AddListener(() => setPlayerTurnPhase(result));
        }
    }

    public void standardTurnChapterHander()
    {
        var player = MainManager.Instance.playerTurn;

        playerTurnEndTurn.interactable = true;

        playerTurnInitialDieImage.gameObject.SetActive(true);
        playerTurnInitialDieImage.sprite = player.GetCharacterDieFace(player.initialRollValue);

        standardRollChapterDieFormatting(player);
    }

    public void reRollChapterHandler()
    {
        print("reRollChapterHandler");
        var player = MainManager.Instance.playerTurn;

        playerTurnEndTurn.onClick.RemoveAllListeners();
        playerTurnEndTurn.onClick.AddListener(() => player.getPlayerDieValue(player.selectedValue, enemyBase));

        PlayerBase result = MainManager.Instance.getNextPlayer(player);
        if (result == null)
        {
            playerTurnEndTurn.onClick.AddListener(() => startEnemyTurnPhase());
        }
        else
        {
            playerTurnEndTurn.onClick.AddListener(() => setPlayerTurnPhase(result));
        }
        playerTurnEndTurn.onClick.AddListener(() => enemyBase.enemyDead());

        playerTurnEndTurn.interactable = false;

        rerollOnClickFormatting(initialRollButton, rerollRollButton, player);

        playerTurnSecondDieImage.gameObject.SetActive(true);
        playerTurnSecondDieImage.sprite = player.GetCharacterDieFace(player.reRollValue);

        reRollChapterDieFormatting(player);
    }

    public void disableNextButton()
    {
        playerTurnEndTurn.interactable = false;
    }

    public void crackedAxeChapterHandler()
    {
        var player = MainManager.Instance.playerTurn;

        playerTurnEndTurn.onClick.RemoveAllListeners();
        playerTurnEndTurn.onClick.AddListener(() => player.getPlayerDieValue(player.initialRollValue, enemyBase));
        playerTurnEndTurn.onClick.AddListener(() => player.chapterDieDamage(player.reRollValue, enemyBase));

        PlayerBase result = MainManager.Instance.getNextPlayer(player);
        if (result == null)
        {
            playerTurnEndTurn.onClick.AddListener(() => startEnemyTurnPhase());
        }
        else
        {
            playerTurnEndTurn.onClick.AddListener(() => setPlayerTurnPhase(result));
        }
        playerTurnEndTurn.onClick.AddListener(() => enemyBase.enemyDead());

        playerTurnEndTurn.interactable = true;

        playerTurnInitialDieImage.gameObject.SetActive(true);
        playerTurnSecondDieImage.gameObject.SetActive(true);

        playerTurnInitialDieImage.sprite = player.GetCharacterDieFace(player.initialRollValue);
        playerTurnSecondDieImage.sprite = player.GetChapterDieFace(player.reRollValue);

        enemyBase.enemyDead();

        crackedAxeChapterDieFormatting(player);
    }

    private void standardRollChapterDieFormatting(PlayerBase player)
    {
        rollChapterDieButton.interactable = true;

        if (enemyHasSpecifiedHealth(player.getCharacterRollResult(player.initialRollValue)))
        {
            playerTurnEndTurn.gameObject.SetActive(false);
            rollChapterDieButton.gameObject.SetActive(true);
        } else
        {
            playerTurnEndTurn.gameObject.SetActive(true);
            rollChapterDieButton.gameObject.SetActive(false);
        }
    }

    public void rerollOnClickFormatting(Button face1, Button face2, PlayerBase player)
    {
        playerTurnEndTurn.interactable = false;
        rollChapterDieButton.interactable = false;

        face1.onClick.AddListener(() => dieOption1Selected(face1, face2, player));
        face2.onClick.AddListener(() => dieOption2Selected(face1, face2, player));
    }

    private void dieOption1Selected(Button face1, Button face2, PlayerBase player)
    {
        playerTurnEndTurn.interactable = true;
        rollChapterDieButton.interactable = true;

        if (player.selectedValue != player.initialRollValue)
        {
            Animator animator1 = face1.GetComponent<Animator>();
            Animator animator2 = face2.GetComponent<Animator>();

            animator1.SetTrigger("Expand");
            if (player.selectedValue == player.reRollValue)
            {
                animator2.SetTrigger("Contract");
            }

            player.selectedValue = player.initialRollValue;

            reRollChapterDieFormatting(player);
        }
    }
    private void dieOption2Selected(Button face1, Button face2, PlayerBase player)
    {
        playerTurnEndTurn.interactable = true;
        rollChapterDieButton.interactable = true;

        if (player.selectedValue != player.reRollValue)
        {
            Animator animator1 = face1.GetComponent<Animator>();
            Animator animator2 = face2.GetComponent<Animator>();

            animator2.SetTrigger("Expand");
            if (player.selectedValue == player.initialRollValue)
            {
                animator1.SetTrigger("Contract");
            }

            player.selectedValue = player.reRollValue;

            reRollChapterDieFormatting(player);
        }
    }

    private void reRollChapterDieFormatting(PlayerBase player)
    {
        if (enemyHasSpecifiedHealth(player.getCharacterRollResult(player.selectedValue)))
        {
            playerTurnEndTurn.gameObject.SetActive(false);
            rollChapterDieButton.gameObject.SetActive(true);
        }
        else
        {
            playerTurnEndTurn.gameObject.SetActive(true);
            rollChapterDieButton.gameObject.SetActive(false);
        }
    }

    private void crackedAxeChapterDieFormatting(PlayerBase player)
    {
        rollChapterDieButton.interactable = true;

        if (enemyHasSpecifiedHealth(player.getCharacterRollResult(player.initialRollValue)) || enemyHasSpecifiedHealth(player.getCharacterRollResult(player.reRollValue)))
        {
            playerTurnEndTurn.gameObject.SetActive(false);
            rollChapterDieButton.gameObject.SetActive(true);
        }
        else
        {
            playerTurnEndTurn.gameObject.SetActive(true);
            rollChapterDieButton.gameObject.SetActive(false);
        }
    }

    private bool enemyHasSpecifiedHealth(ChapterDieOptions rolledValue)
    {
        print("enemyHasSpecifiedHealth");
        switch (rolledValue)
        {
            case ChapterDieOptions.MIGHT:
                if(enemyBase.getEnemyMight() > 0)
                {
                    return true;
                } else
                {
                    return false;
                }

            case ChapterDieOptions.CUNNING:
                if (enemyBase.getEnemyCunning() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case ChapterDieOptions.WISDOM:
                if (enemyBase.getEnemyWisdom() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            default:
                Debug.Log("Error!");
                return false;
        }
    }

    public void rollChapterLogic(EnemyBase enemy, PlayerBase player)
    {
        StartCoroutine(rollChapterLogicIE(enemy, player));
    }

    IEnumerator rollChapterLogicIE(EnemyBase enemy, PlayerBase player)
    {
        if (!player.chapterDie.isRolling)
        {
            rollChapterDieButton.interactable = false;

            player.chapterDie.gameObject.SetActive(true);
            player.chapterDie.Roll();
            player.RollStarted.Raise();

            while (player.chapterDie.isRolling)
            {
                yield return null;
            }

            player.RollFinished.Raise();

            playerTurnEndTurn.gameObject.SetActive(true);
            playerTurnEndTurn.interactable = true;

            rollChapterDieButton.gameObject.SetActive(false);

            string dieValue = player.chapterDie.dieSides.GetDieSideMatchInfo().closestMatch.ValuesAsString();
            ChapterDieOptions rollResult = player.getChapterRolResult(dieValue);

            switch (rollResult)
            {
                case ChapterDieOptions.MIGHT or ChapterDieOptions.CUNNING:
                    //STANDARD HANDLING

                    break;

                case ChapterDieOptions.WISDOM:
                    //END TURN BUTTON
                    playerTurnEndTurn.onClick.RemoveAllListeners();

                    //Add enemy health
                    playerTurnEndTurn.onClick.AddListener(() => enemy.setEnemyWisdom(1));

                    //determines whos turns next
                    PlayerBase result = MainManager.Instance.getNextPlayer(player);
                    if (result == null)
                    {
                        playerTurnEndTurn.onClick.AddListener(() => startEnemyTurnPhase());
                    }
                    else
                    {
                        playerTurnEndTurn.onClick.AddListener(() => setPlayerTurnPhase(result));
                    }
                    //checks if the enemy is dead
                    playerTurnEndTurn.onClick.AddListener(() => enemyBase.enemyDead());

                    break;

                default:
                    break;
            }

            player.chapterDie.gameObject.SetActive(false);

        }
    }

    public void rollTypeHandler()
    {
        var player = MainManager.Instance.playerTurn;

        if (state == BattleState.SET_ENEMY_HEALTH)
        {
            preperationPhase();
        }
        else if (state == BattleState.PLAYER_TURN)
        {
            if (player.inventoryContainsCard("Cracked axe"))
            {
                crackedAxeChapterHandler();
            }
            else if (player.hasReRolled)
            {
                reRollChapterHandler();
            }
            else
            {
                standardTurnChapterHander();
            }
        }
        else
        {
            print("rollTypeHandler: ISSUE ENCOUNTERED");
        }
    }
}
