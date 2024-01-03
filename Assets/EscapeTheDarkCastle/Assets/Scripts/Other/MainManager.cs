using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MainManager : MonoBehaviour
{

    public static MainManager Instance;

    public List<PlayerBase> Players = new();

    [SerializeField] public ChapterLogicNew cl;

    public int drawCards = 1;

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
