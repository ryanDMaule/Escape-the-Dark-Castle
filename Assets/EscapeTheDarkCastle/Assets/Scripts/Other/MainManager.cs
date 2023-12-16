using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{

    public static MainManager Instance;

    public List<PlayerBase> Players = new();

    //player 1
    public string player1_name = null; 
    public int player1_health = 18; 
    public Card player1_inventory0 = null; 
    public Card player1_inventory1 = null;

    //player 2
    public string player2_name = null;
    public int player2_health = 18;
    public Card player2_inventory0 = null;
    public Card player2_inventory1 = null;

    //player 3
    public string player3_name = null;
    public int player3_health = 18;
    public Card player3_inventory0 = null;
    public Card player3_inventory1 = null;

    //player 4
    public string player4_name = null;
    public int player4_health = 18;
    public Card player4_inventory0 = null;
    public Card player4_inventory1 = null;

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
            updatePlayer(players[i], i);
        }
    }

    public string getPlayerName(int arrayPos)
    {
        switch (arrayPos)
        {
            case 0:
                return player1_name;

            case 1:
                return player2_name;

            case 2:
                return player3_name;

            case 3:
                return player4_name;

            default:
                Debug.Log("Error!");
                return "";
        }
    }

    public int getPlayerHealth(int arrayPos)
    {
        switch (arrayPos)
        {
            case 0:
                return player1_health;

            case 1:
                return player2_health;

            case 2:
                return player3_health;

            case 3:
                return player4_health;

            default:
                Debug.Log("Error!");
                return -1;
        }
    }

    public void updatePlayer(PlayerBase player, int arrayPos)
    {
        switch (arrayPos)
        {
            case 0:
                player1_name = player.name;
                player1_health = player.currentHealth;
                player1_inventory0 = player.InventoryArray[0];
                player1_inventory1 = player.InventoryArray[1];
                break;

            case 1:
                player2_name = player.name;
                player2_health = player.currentHealth;
                player2_inventory0 = player.InventoryArray[0];
                player2_inventory1 = player.InventoryArray[1];
                break;

            case 2:
                player3_name = player.name;
                player3_health = player.currentHealth;
                player3_inventory0 = player.InventoryArray[0];
                player3_inventory1 = player.InventoryArray[1];
                break;

            case 3:
                player4_name = player.name;
                player4_health = player.currentHealth;
                player4_inventory0 = player.InventoryArray[0];
                player4_inventory1 = player.InventoryArray[1];
                break;

            default:
                Debug.Log("Error!");
                break;
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

}
