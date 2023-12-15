using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{

    [SerializeField] public RawImage Abbot; 
    [SerializeField] public RawImage Miller;
    [SerializeField] public RawImage Cook;
    [SerializeField] public RawImage Smith;
    [SerializeField] public RawImage Tanner;
    [SerializeField] public RawImage Tailor;

    public Color disabledColour; 
    public Color enabledColour;

    public Button continueButton;

    public List<PlayerBase> selectedCharacters = new();

    public Scenes scenes;

    public void selectCharacter(PlayerBase player)
    {
        if (selectedCharacters.Contains(player))
        {
            deselectCharacter(player);
        } else
        {
            if (selectedCharacters.Count < 4)
            {
                selectedCharacters.Add(player);
                setCardOpactiy(player, enabledColour);
            } else
            {
                Debug.Log("Too many characters selected!");
            }
        }
        allowContinue();
    }

    public void deselectCharacter(PlayerBase player)
    {
        if (selectedCharacters.Contains(player))
        {
            selectedCharacters.Remove(player);
            setCardOpactiy(player, disabledColour);
        }
        else
        {
            Debug.Log("Player not in list");
        }
    }

    private void setCardOpactiy(PlayerBase player, Color colour)
    {
        switch (player.name)
        {
            case "Abbot":
                Abbot.color = colour;
                break;

            case "Miller":
                Miller.color = colour;
                break;

            case "Smith":
                Smith.color = colour;
                break;

            case "Cook":
                Cook.color = colour;
                break;

            case "Tanner":
                Tanner.color = colour;
                break;

            case "Tailor":
                Tailor.color = colour;
                break;

            default:
                Debug.Log("Unknown player");
                break;
        }
    }

    public void allowContinue()
    {
        if(selectedCharacters.Count >= 2 && selectedCharacters.Count <= 4)
        {
            continueButton.interactable = true;
        } else
        {
            continueButton.interactable = false;
        }
    }

    public void loadNextScene()
    {
        //ASSIGN THE SELECTED PLAYERS TO PERSISTENT STORAGE (MainManager)

        /*
        foreach (var player in selectedCharacters)
        {
            Debug.Log("4EACH:" + player);
            MainManager.Instance.Players.Add(player);
        }
        */

        MainManager.Instance.addPlayers(selectedCharacters);
        MainManager.Instance.printPlayers();

        //Loader.Load(scenes.chapterList[Random.Range(0, scenes.chapterList.Count)]);

        Loader.Load(scenes.preGame);
    }
}
