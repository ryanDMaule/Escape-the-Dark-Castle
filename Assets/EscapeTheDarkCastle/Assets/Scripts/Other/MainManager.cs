using System;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Events")]
    public GameEvent GameStateUpdate;
    public GameEvent RollFinished;

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
        for (int i = 0; i < players.Count; i++)
        {
            Players.Add(players[i]);
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

            RollFinished.Raise();

            foreach (var player in Players)
            {
                player.hasRolled = false;
                player.hasReRolled = false;
            }
        } else
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

}
