using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState { CHARACTER_SELECT, PRE_GAME, CHAPTER, ITEMS_PHASE }

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    [Header("Players")]
    public List<PlayerBase> Players = new();
    public PlayerBase playerTurn;

    [Header("Global accessors")]
    [SerializeField] public ChapterLogicBase clBase;

    [SerializeField] public DeckLogic dl;
    private GameState currentGameState;

    [Header("Other")]
    public int drawCards = 1;
    public Text RoundTextHUD;

    [Header("Events")]
    public GameEvent GameStateUpdate;
    public GameEvent RollFinished;

    [Header("Chapter stuffs")]
    [SerializeField] public Scenes scenes;
    private List<string> gameChapters = new List<string>();
    public void updateGameState(GameState gs)
    {
        currentGameState = gs;
        GameStateUpdate.Raise();
    }

    public GameState getCurrentGameState()
    {
        return currentGameState;
    }

    public void printPlayers()
    {
        foreach (var player in Players)
        {
            Debug.Log("PLAYER: " + player);
        }
    }

    public void addPlayers(List<PlayerBase> players)
    {
        int playerTotal = players.Count;

        for (int i = 0; i < playerTotal; i++)
        {
            Players.Add(players[i]);
        }

        switch (playerTotal)
        {
            case 1 or 2:
                setPlayersHealthTotal(18);
                break;

            case 3:
                setPlayersHealthTotal(14);
                break;

            case 4:
                setPlayersHealthTotal(12);
                break;

            default:
                setPlayersHealthTotal(18);
                break;
        }

    }

    private void setPlayersHealthTotal(int startHealth)
    {
        foreach(var player in MainManager.Instance.Players)
        {
            player.currentHealth = startHealth;
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        GenerateChapterList();
    }

    //used to determine the next player for a turn during chapter combat
    public PlayerBase getNextPlayer(PlayerBase passedPlayer)
    {
        for (int i = 0; i < Players.Count; i++)
        {
            if (Players[i] == passedPlayer)
            {
                if(i+1 < Players.Count)
                {
                    return Players[i + 1];
                } else
                {
                    return null;
                }
            }
        }
        return null;
    }

    //checks all players in the game to see if they have rolled and whern true move onto the next phase of a chapter (prep phase)
    public void playersRolled()
    {
        bool allPlayersRolled = true;
        foreach(var player in Players)
        {
            if (!player.hasRolled)
            {
                allPlayersRolled = false;
                break;
            }
        }

        if (allPlayersRolled)
        {
            Debug.Log("ALL PLAYERS ROLLED");

            foreach (var player in Players)
            {
                player.hasRolled = false;
                player.hasReRolled = false;
            }

            RollFinished.Raise();
        }
        else
        {
            Debug.Log("ALL PLAYERS HAVE NOT ROLLED");
        }
    }

    public PlayerBase getYou()
    {
        foreach(var player in Players)
        {
            if (player.YOU)
            {
                return player;
            }
        }

        throw new Exception();
    }

    private void GenerateChapterList()
    {
        gameChapters.Add(scenes.getStartRoom());

        //SET TO 15 WHEN AVAILABLE
        for(int i = 0 ; i < 15 ; i++)
        {
            gameChapters.Add(scenes.getChapter());
        }

        gameChapters.Add(scenes.getBoss());

        gameChapters.Add(scenes.getVictory());

        foreach(var chapter in gameChapters)
        {
            print("CHAPTER: " + chapter);
        }
    }

    int currentRound = 0;
    public void LoadNextChapter()
    {
        print("LoadNextChapter: " + gameChapters[0]);

        Loader.Load(gameChapters[0]);
        SetChapterRoundHUD(currentRound);

        gameChapters.RemoveAt(0);
        currentRound++;

        //scenes.loadTest();

    }

    private void SetChapterRoundHUD(int round)
    {
       if(round == 0)
       {
           RoundTextHUD.text = "START ROOM";
       } else if (round == 15)
       {
           RoundTextHUD.text = "FINAL CHAPTER";
       }
       else
       {
           RoundTextHUD.text = "CHAPTER " + round;
       }
    }

}
