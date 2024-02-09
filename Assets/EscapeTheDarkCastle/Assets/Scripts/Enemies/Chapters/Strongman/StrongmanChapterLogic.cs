using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StrongmanChapterLogic : ChapterLogicBase
{
    #region globalVariables
    [Header("Game objects")]
    [SerializeField] public GameObject playersCombinedHUD;
    [SerializeField] public GameObject playerTurnHUD;
    [SerializeField] public TextMeshProUGUI playerTurnName;
    [SerializeField] public Button playerTurnRoll;
    [SerializeField] public Button playerTurnEndTurn;

    [SerializeField] public Image playerTurnInitialDieImage;
    [SerializeField] public Image playerTurnSecondDieImage;
    [SerializeField] public Button initialRollButton;
    [SerializeField] public Button rerollRollButton;

    [SerializeField] public GameObject winHUD;
    [SerializeField] public Button Continue_button;

    [Header("Other")]
    public Scenes scenes;

    public StrongmanFormatChapter fc;

    [SerializeField] public Sprite defaultStateSprite;

    private bool initialTurnValue = true;

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

    //this is called after the above duplicate function but this takes the previous player as a paramter to get the next player 
    public void setPlayerTurnPhase()
    {
        setPlayerTurnHUD();
        formatPlayerTurnHUDNew(MainManager.Instance.playerTurn);
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

            PlayerBase selectedPlayer = MainManager.Instance.playerTurn;
            if (!selectedPlayer.getShieldActiveState() && !selectedPlayer.getPotionProtectionState())
            {
                //TODO : update cards to have an ENUM title
                if (selectedPlayer.inventoryContainsCard("rotten shield_0"))
                {
                    int damage = enemyBase.getDamage();
                    if (damage > 1)
                    {
                        damage--;
                    }
                    playerDead = selectedPlayer.RedcuceHealth(damage);
                }
                else
                {
                    playerDead = selectedPlayer.RedcuceHealth(enemyBase.getDamage());
                }
            }
            selectedPlayer.setShieldActiveState(false);
            selectedPlayer.setPotionProtectionState(false);

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

    private void initialTurn()
    {
        Debug.Log("initialTurn");

        //Set YOU player to fight
        foreach (var player in MainManager.Instance.Players)
        {
            if (player.YOU)
            {
                selectFighter(player);
                break;
            }
        }

        //disable all "Select" buttons
        fc.setPlayerSelection(false);
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

        //Force YOU to fight first on initial tuirn
        if (initialTurnValue)
        {
            initialTurn();
            Continue_button.interactable = true;
        } else
        {
            fc.setPlayerSelection(true);
        }

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
            playerTurnRoll.onClick.AddListener(() => player.standardTurnSimplified(playerTurnRoll));
        }

        //END TURN BUTTON
        playerTurnEndTurn.onClick.RemoveAllListeners();

        //deals the damage the player rolled
        playerTurnEndTurn.onClick.AddListener(() => player.getPlayerDieValue(player.initialRollValue, enemyBase));

        //Move onto enemy turn
        playerTurnEndTurn.onClick.AddListener(() => startEnemyTurnPhase());

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

        initialTurnValue = false;
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

    public void standardTurnChapterHander()
    {
        var player = MainManager.Instance.playerTurn;

        playerTurnEndTurn.interactable = true;
        playerTurnInitialDieImage.gameObject.SetActive(true);
        playerTurnInitialDieImage.sprite = player.GetCharacterDieFace(player.initialRollValue);
    }

    public void reRollChapterHandler()
    {
        print("reRollChapterHandler");
        var player = MainManager.Instance.playerTurn;

        playerTurnEndTurn.onClick.RemoveAllListeners();
        playerTurnEndTurn.onClick.AddListener(() => player.getPlayerDieValue(player.selectedValue, enemyBase));

        playerTurnEndTurn.onClick.AddListener(() => startEnemyTurnPhase());

        playerTurnEndTurn.onClick.AddListener(() => enemyBase.enemyDead());

        playerTurnEndTurn.interactable = false;

        player.rerollOnClickFormatting(initialRollButton, rerollRollButton, playerTurnEndTurn);

        playerTurnSecondDieImage.gameObject.SetActive(true);
        playerTurnSecondDieImage.sprite = player.GetCharacterDieFace(player.reRollValue);
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

        playerTurnEndTurn.onClick.AddListener(() => startEnemyTurnPhase());

        playerTurnEndTurn.onClick.AddListener(() => enemyBase.enemyDead());

        playerTurnEndTurn.interactable = true;

        playerTurnInitialDieImage.gameObject.SetActive(true);
        playerTurnSecondDieImage.gameObject.SetActive(true);

        playerTurnInitialDieImage.sprite = player.GetCharacterDieFace(player.initialRollValue);
        playerTurnSecondDieImage.sprite = player.GetChapterDieFace(player.reRollValue);

        enemyBase.enemyDead();
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

    public void selectFighter(PlayerBase selectedPlayer)
    {
        MainManager.Instance.playerTurn = selectedPlayer;
        selectedPlayer.combatState.sprite = selectedPlayer.fightSprite;

        //update the combat state image of all othe rplayers to the dash
        foreach (var player in MainManager.Instance.Players)
        {
            if (player != selectedPlayer)
            {
                player.combatState.sprite = defaultStateSprite;
            }
        }
    }

}
