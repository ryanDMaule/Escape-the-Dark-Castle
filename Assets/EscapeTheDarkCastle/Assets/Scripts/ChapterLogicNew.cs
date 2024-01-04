using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class ChapterLogicNew : MonoBehaviour
{
    #region globalVariables

    [SerializeField] public GameObject playersCombinedHUD;
    [SerializeField] public GameObject playerTurnHUD;
    [SerializeField] public Text playerTurnName;
    [SerializeField] public Button playerTurnRoll;
    [SerializeField] public Button playerTurnInventory;
    [SerializeField] public Button playerTurnEndTurn;

    [SerializeField] public Text win_lose_text;
    [SerializeField] public Button Continue_button;

    [SerializeField] private EnemyBase enemyBase;

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
        MainManager.Instance.printPlayers();

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
            //formatPlayerTurnHUD(MainManager.Instance.Players[0]);
            formatPlayerTurnHUDNew(MainManager.Instance.Players[0]);
        }
    }

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
        bool playerDead = false;

        setEnemyTurnHUD();
        foreach (var player in MainManager.Instance.Players)
        {
            if (!player.getShieldActiveState() && !player.getIsRestingState() && !player.getPotionProtectionState())
            {
                //TODO : update cards to have an ENUM title
                if(player.inventoryContainsCard("rotten shield_0"))
                {
                    int damage = enemyBase.getDamage();
                    if(damage > 1)
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

        if (!playerDead)
        {
            setPreperationHUD();
        } else
        {
            setLoseHUD();
        }
    }


    public void win()
    {
        Debug.Log("YOU WIN!");
        //setWinHUD();

        Loader.Load(scenes.itemScreen);
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

        win_lose_text.gameObject.SetActive(false);
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

        win_lose_text.gameObject.SetActive(false);
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

        win_lose_text.gameObject.SetActive(false);
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

        win_lose_text.gameObject.SetActive(false);
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

        win_lose_text.gameObject.SetActive(false);
        Continue_button.gameObject.SetActive(false);
    }

    private void formatPlayerTurnHUD(PlayerBase player)
    {
        var textFields = playerTurnHUD.GetComponentsInChildren<Text>();
        foreach (var item in textFields)
        {
            if (item.tag == "HUD-name")
            {
                item.text = player.name;
                break;
            }
        }

        var buttons = playerTurnHUD.GetComponentsInChildren<Button>();
        foreach (var button in buttons)
        {
            //ROLL
            if (button.tag == "CharacterDie")
            {
                button.onClick.RemoveAllListeners();
                //button.onClick.AddListener(() => player.rollLogicNew(enemyBase));
                button.onClick.AddListener(() => player.rollLogicNew(enemyBase, playerTurnRoll, playerTurnEndTurn));
                continue;
            }

            //INVENTORY
            if (button.tag == "Inventory")
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => player.openInventory(MainManager.Instance.Players));
                continue;
            }

            //END TURN
            if (button.tag == "Finish")
            {
                button.onClick.RemoveAllListeners();
                PlayerBase result = MainManager.Instance.getNextPlayer(player);
                if(result == null)
                {
                    button.onClick.AddListener(() => startEnemyTurnPhase());
                } else
                {
                    button.onClick.AddListener(() => setPlayerTurnPhase(result));
                }
                continue;
            }
        }
    }

    private void formatPlayerTurnHUDNew(PlayerBase player)
    {
        playerTurnName.text = player.name;
    
        playerTurnRoll.onClick.RemoveAllListeners();
        playerTurnRoll.onClick.AddListener(() => player.rollLogicNew(enemyBase, playerTurnRoll, playerTurnEndTurn));

        playerTurnInventory.onClick.RemoveAllListeners();
        playerTurnInventory.onClick.AddListener(() => player.openInventory(MainManager.Instance.Players));

        playerTurnEndTurn.onClick.RemoveAllListeners();
        PlayerBase result = MainManager.Instance.getNextPlayer(player);
        if (result == null)
        {
            playerTurnEndTurn.onClick.AddListener(() => startEnemyTurnPhase());
        }
        else
        {
            playerTurnEndTurn.onClick.AddListener(() => setPlayerTurnPhase(result));
        }

        playerTurnRoll.interactable = true;
        playerTurnEndTurn.interactable = false;
    }


    void setEnemyTurnHUD()
    {
        Debug.Log("setEnemyTurnHUD");

        enemyBase.HIDE_DESCRIPTION();
        enemyBase.hideOptionsHUD();
        enemyBase.SET_ENEMY_ASSETS_VISIBLE();

        playersCombinedHUD.SetActive(false);
        playerTurnHUD.SetActive(false);

        win_lose_text.gameObject.SetActive(false);
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

        win_lose_text.gameObject.SetActive(true);
        win_lose_text.text = "YOU WIN!";
        Continue_button.gameObject.SetActive(false);
    }

    void setLoseHUD()
    {
        Debug.Log("setLoseHUD");

        enemyBase.HIDE_DESCRIPTION();
        enemyBase.hideOptionsHUD();
        enemyBase.SET_ENEMY_ASSETS_VISIBLE();

        playersCombinedHUD.SetActive(false);
        playerTurnHUD.SetActive(false);

        win_lose_text.gameObject.SetActive(true);
        win_lose_text.text = "YOU LOSE!";
        Continue_button.gameObject.SetActive(false);
    }

    #endregion
}
