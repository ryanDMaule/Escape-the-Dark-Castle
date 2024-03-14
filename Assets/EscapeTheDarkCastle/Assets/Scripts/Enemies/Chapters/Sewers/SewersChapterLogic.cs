using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SewersChapterLogic : ChapterLogicBase
{
    #region globalVariables
    [Header("Game objects")]
    [SerializeField] public GameObject playerTurnHUD;
    [SerializeField] public TextMeshProUGUI playerTurnName;
    [SerializeField] public Button playerTurnRoll;
    [SerializeField] public Button playerTurnEndTurn;

    [SerializeField] public Image playerTurnInitialDieImage;
    [SerializeField] public Image playerTurnSecondDieImage;
    [SerializeField] public Button initialRollButton;
    [SerializeField] public Button rerollRollButton;

    [SerializeField] public GameObject winHUD;

    [SerializeField] public Text roundNumber;

    [Header("Other")]
    public Scenes scenes;

    #endregion

    void Start()
    {
        MainManager.Instance.updateGameState(GameState.CHAPTER);

        MainManager.Instance.clBase = this;

        setDescriptionPhase();
    }

    //this is called first as it just starts the process with the first character in the players list
    public void startPlayerTurnPhase()
    {  
        foreach(var player in MainManager.Instance.Players)
        {
            if (player.nominatedPlayer)
            {
                setPlayerTurnHUD();
                formatPlayerTurnHUDNew(player);
                break;
            }
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
                if (!player.getShieldActiveState() && !player.getPotionProtectionState() || player.nominatedPlayer)
                {
                     playerDead = player.RedcuceHealth(enemyBase.getDamage());
                }
                player.setShieldActiveState(false);
                player.setPotionProtectionState(false);
            }

            chapterLoop();
        }
    }

    public void win()
    {
        Debug.Log("YOU WIN!");

        foreach(var player in MainManager.Instance.Players)
        {
            player.nominatedPlayer = false;
        }

        setWinHUD();
    }

    #region HUD_methods
    public override void setDescriptionHUD()
    {
        base.setDescriptionHUD();

        playerTurnHUD.SetActive(false);

        winHUD.gameObject.SetActive(false);

        roundNumber.gameObject.SetActive(false);
    }

    public override void setCombatOptionsHUD()
    {
        base.setCombatOptionsHUD();

        playerTurnHUD.SetActive(false);

        winHUD.gameObject.SetActive(false);

        roundNumber.gameObject.SetActive(false);
    }

    void setPlayerTurnHUD()
    {
        enemyBase.HIDE_DESCRIPTION();
        enemyBase.hideOptionsHUD();
        enemyBase.SET_ENEMY_ASSETS_VISIBLE();

        playerTurnHUD.SetActive(true);

        winHUD.gameObject.SetActive(false);

        roundNumber.gameObject.SetActive(true);
    }

    private void formatPlayerTurnHUDNew(PlayerBase player)
    {
        print("formatPlayerTurnHUDNew");

        MainManager.Instance.playerTurn = player;
        state = BattleState.PLAYER_TURN;

        //SET NAME
        playerTurnName.text = player.name;

        //ROLL BUTTON
        playerTurnRoll.onClick.RemoveAllListeners();
        playerTurnRoll.onClick.AddListener(() => player.standardTurnSimplified(playerTurnRoll));

        //END TURN BUTTON
        playerTurnEndTurn.onClick.RemoveAllListeners();

        //deals the damage the player rolled
        if (player.nominatedPlayer)
        {
            //deal the damage
            playerTurnEndTurn.onClick.AddListener(() => player.getPlayerDieValue(player.initialRollValue, enemyBase));
        } 

        //determines whos turns next
        PlayerBase result;
        //IF the passed player was the nominated player then we need to search from the beggining of the player list
        if (player.nominatedPlayer)
        {
            result = MainManager.Instance.Players[0];
        } else
        {
            result = MainManager.Instance.getNextPlayer(player);
        }

        if (result == null)
        {
            playerTurnEndTurn.onClick.AddListener(() => startEnemyTurnPhase());
        }
        else
        {
            //skip nominated player as they have had their turn      
            if (result.nominatedPlayer)
            {
                result = MainManager.Instance.getNextPlayer(result);
            }

            if (result == null)
            {
                playerTurnEndTurn.onClick.AddListener(() => startEnemyTurnPhase());
            }
            else
            {
                playerTurnEndTurn.onClick.AddListener(() => setPlayerTurnPhase(result));
            }
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

        playerTurnHUD.SetActive(false);

        winHUD.gameObject.SetActive(false);

        roundNumber.gameObject.SetActive(true);
    }

    public override void setWinHUD()
    {
        base.setWinHUD();

        playerTurnHUD.SetActive(false);

        winHUD.gameObject.SetActive(true);

        roundNumber.gameObject.SetActive(false);
    }

    #endregion

    int round = 1;
    public void chapterLoop()
    {
        var dead = enemyBase.enemyDead();
        if (dead || round >= 5)
        {
            setWinHUD();
        }
        else
        {
            startPlayerTurnPhase();
        }
        round++;
        roundNumber.text = "   Attempt " + round + " / 5";
    }

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

        if(!player.nominatedPlayer)
        {
            //if player rolled WISDOM prevent taking damage
            if (player.getCharacterRollResult(player.initialRollValue) == ChapterDieOptions.WISDOM)
            {
                player.setShieldActiveState(true);
            }
        }
    }
    public void disableNextButton()
    {
        playerTurnEndTurn.interactable = false;
    }

    public void rollTypeHandler()
    {
        var player = MainManager.Instance.playerTurn;

        if (state == BattleState.PLAYER_TURN)
        {
            standardTurnChapterHander();
        }
        else
        {
            print("rollTypeHandler: ISSUE ENCOUNTERED");
        }
    }
}
