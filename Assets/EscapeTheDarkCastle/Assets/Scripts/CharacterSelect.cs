using System.Collections;
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

    public List<PlayerBase> selectedCharacters = new();

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

    public bool allowContinue()
    {
        if(selectedCharacters.Count >= 2 && selectedCharacters.Count <= 4)
        {
            return true;
        } else
        {
            return false;
        }
    }
}
