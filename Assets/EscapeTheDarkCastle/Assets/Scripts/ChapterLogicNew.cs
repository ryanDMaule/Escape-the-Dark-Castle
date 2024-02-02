using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { DESCRIPTION, COMBAT_OPTIONS, SET_ENEMY_HEALTH, PREPERATION, PLAYER_TURN, ENEMY_TURN, WON, LOST }

public class ChapterLogicNew : MonoBehaviour
{
    #region globalVariables

    [Header("Death stuffs")]
    [SerializeField] public string deathBio = "";
    [SerializeField] public AudioClip deathClip;

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
    [SerializeField] public EnemyBase enemyBase;

    public BattleState state;

    public Scenes scenes;

    public FormatChapter fc;

    #endregion

    public BattleState getState()
    {
        return state;
    }

    void Start()
    {
        MainManager.Instance.updateGameState(GameState.CHAPTER);

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
    }

    public void preperationPhase()
    {
        Debug.Log("preperationPhase");

        setPreperationHUD();
    }

    //this is called first as it just starts the process with the first character in the players list
    public void startPlayerTurnPhase()
    {
        foreach(var p in MainManager.Instance.Players)
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
        } else
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
            //formatPlayerTurnHUD(player);
            formatPlayerTurnHUDNew(player);
        }
    }

    public void startEnemyTurnPhase()
    {
        if (!enemyBase.enemyDead())
        {
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
    void setDescriptionHUD()
    {
        Debug.Log("setDescriptionHUD");

        enemyBase.SHOW_DESCRIPTION();
        enemyBase.hideOptionsHUD();
        enemyBase.SET_ENEMY_IMAGE_ONLY();

        playersCombinedHUD.SetActive(false);
        playerTurnHUD.SetActive(false);

        winHUD.gameObject.SetActive(false);
        Continue_button.gameObject.SetActive(false);
    }

    void setCombatOptionsHUD()
    {
        Debug.Log("setCombatOptionsHUD");

        enemyBase.SHOW_DESCRIPTION();
        enemyBase.showOptionsHUD();
        enemyBase.SET_ENEMY_IMAGE_ONLY();

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
        foreach(var player in MainManager.Instance.Players)
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

    private void formatPlayerTurnHUDNew(PlayerBase player)
    {
        //SET NAME
        playerTurnName.text = player.name;
    
        //ROLL BUTTON
        playerTurnRoll.onClick.RemoveAllListeners();
        if (player.inventoryContainsCard("Cracked axe"))
        {
            playerTurnRoll.onClick.AddListener(() => player.CrackedAxeRoll(enemyBase, playerTurnRoll, playerTurnEndTurn, playerTurnInitialDieImage, playerTurnSecondDieImage, this));
        }
        else
        {
            playerTurnRoll.onClick.AddListener(() => player.standardTurn(enemyBase, playerTurnRoll, playerTurnEndTurn, playerTurnInitialDieImage, playerTurnSecondDieImage, initialRollButton, rerollRollButton, this));
        }

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

    public void setWinHUD()
    {
        Debug.Log("setWinHUD");

        enemyBase.HIDE_DESCRIPTION();
        enemyBase.hideOptionsHUD();
        enemyBase.SET_ENEMY_ASSETS_VISIBLE();

        playersCombinedHUD.SetActive(false);
        playerTurnHUD.SetActive(false);

        winHUD.gameObject.SetActive(true);
        Continue_button.gameObject.SetActive(false);
    }

    public void setLoseHUD()
    {
        Debug.Log("setLoseHUD");

        enemyBase.HIDE_DESCRIPTION();
        enemyBase.hideOptionsHUD();
        enemyBase.SET_ENEMY_ASSETS_VISIBLE();

        playersCombinedHUD.SetActive(false);
        playerTurnHUD.SetActive(false);

        Continue_button.gameObject.SetActive(false);
    }

    #endregion
}
