using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHandler : MonoBehaviour
{

    [SerializeField] public GameObject optionsOverlay;
    [SerializeField] public Image cardImage;

    private PlayerBase Player;
    private Card Card;

    [SerializeField] public DeckLogic dl;

    public void showCardOptions(Card card, PlayerBase player)
    {
        Debug.Log("ASSSIGNED PLAYER : " + player);
        Player = player;
        Card = card;

        optionsOverlay.gameObject.SetActive(true);
        cardImage.sprite = card.cardFace;
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

    public void tradeItem()
    {

    }

    public void hideCardOptions()
    {
        optionsOverlay.gameObject.SetActive(false);
    }

}
