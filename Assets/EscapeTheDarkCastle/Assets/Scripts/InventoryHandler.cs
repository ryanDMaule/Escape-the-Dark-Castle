using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHandler : MonoBehaviour
{
    [Header("General Assets")]
    [SerializeField] public GameObject optionsOverlay;
    [SerializeField] public Image cardImage;

    [Header("Usage buttons")]
    [SerializeField] public Button useButton;
    [SerializeField] public Button tradeButton;
    [SerializeField] public Button discardButton;

    [Header("Trade assets")]
    [SerializeField] public GameObject tradeButtonList;
    [SerializeField] public Button trade_Player1;
    [SerializeField] public Button trade_Player2;
    [SerializeField] public Button trade_Player3;

    [Header("Other")]
    [SerializeField] public DeckLogic dl;

    private PlayerBase Player;
    private Card Card;

    public void toggleTradePlayersList()
    {
        if (tradeButtonList.gameObject.activeInHierarchy)
        {
            tradeButtonList.gameObject.SetActive(false);
        }
        else
        {
            tradeButtonList.gameObject.SetActive(true);
        }
    }

    public void closeTradePlayersList()
    {
        tradeButtonList.gameObject.SetActive(false);
    }

    public void deleteUnusedTradeButtons()
    {
        int count = MainManager.Instance.Players.Count - 1;
        switch (count)
        {
            case 1:
                Destroy(trade_Player2.gameObject);
                Destroy(trade_Player3.gameObject);
                break;

            case 2:
                Destroy(trade_Player3.gameObject);
                break;

            default:
                Debug.Log("Error!");
                break;
        }
    }

    public void showCardOptions(Card card, PlayerBase player)
    {
        Debug.Log("ASSSIGNED PLAYER : " + player);
        Player = player;
        Card = card;

        optionsOverlay.gameObject.SetActive(true);
        cardImage.sprite = card.cardFace;

        tradeButtonInteractable();
        useButtonInteractable();
        formatTradePlayersList();
    }

    private void formatTradePlayersList()
    {
        int index = 0;
        foreach(var player in MainManager.Instance.Players)
        {
            if(player != Player)
            {
                formatTradeButton(player, index);
                index++;
            } 
        }
    }

    private void formatTradeButton(PlayerBase player, int index)
    {
        switch (index)
        {
            case 0:
                buttonInteractable(trade_Player1, player);
                break;

            case 1:
                buttonInteractable(trade_Player2, player);
                break;

            case 2:
                buttonInteractable(trade_Player3, player);
                break;

            default:
                Debug.Log("Error!");
                break;
        }
    }

    private void buttonInteractable(Button button, PlayerBase player)
    {
        //set the name on the trade button
        button.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = player.name;
         
        //if the player does not have space in their inventory, disable the button
        if (player.InventorySlotsFree() >= Card.size)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }

        //set on click logic
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => tradeItem(player));
    }

    public void useItem()
    {
        if (Card.cardUsageType.Equals(CardUsageType.ACTIVE))
        {
            Card.useItem(Player);
            Player.removeInventoyCard(Card, dl);
        }
        else
        {
            Debug.Log("CARD TYPE CAN NOT BE USED RIGHT NOW");
        }
    }

    public void discardItem()
    {
        Card.discardCard(Card);
        Player.removeInventoyCard(Card, dl);
        hideCardOptions();
    }

    public void tradeItem(PlayerBase receiver)
    {
        if(receiver.InventorySlotsFree() >= Card.size)
        {
            Player.tradeInventoryCard(Card);
            receiver.assignInventoryCard(Card);
            hideCardOptions();
        }
    }

    public void hideCardOptions()
    {
        optionsOverlay.gameObject.SetActive(false);
    }

    private void useButtonInteractable()
    {
        if(MainManager.Instance.getCurrentGameState() == GameState.CHAPTER && Card.cardUsageType == CardUsageType.ACTIVE)
        {
            useButton.interactable = true;
        } else
        {
            useButton.interactable = false;
        }
    }

    public void tradeButtonInteractable()
    {
        if(MainManager.Instance.getCurrentGameState() == GameState.ITEMS_PHASE)
        {
            tradeButton.interactable = true;
            discardButton.interactable = true;
        }
        else
        {
            tradeButton.interactable = false;
            discardButton.interactable = false;
        }
    }

}
