using System.Collections.Generic;
using UnityEngine;

public enum GameState { CHARACTER_SELECT, PRE_GAME, CHAPTER, ITEMS_PHASE }

public class MainManager : MonoBehaviour
{

    public static MainManager Instance;

    public List<PlayerBase> Players = new();

    [SerializeField] public ChapterLogicNew cl;
    [SerializeField] public DeckLogic dl;

    private GameState currentGameState;

    public int drawCards = 1;

    [Header("Events")]
    public GameEvent GameStateUpdate;

    public void updateGameState(GameState gs)
    {
        currentGameState = gs;

        //update radio to inidicate the state has changed
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

    public void setChapters()
    {

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
            }
            cl.preperationPhase();
        } else
        {
            Debug.Log("ALL PLAYERS HAVE NOT ROLLED");
        }
    }

}
