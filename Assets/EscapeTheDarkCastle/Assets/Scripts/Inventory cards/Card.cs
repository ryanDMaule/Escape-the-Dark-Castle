using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardUsageType { PASSIVE, ACTIVE }


public abstract class Card : MonoBehaviour
{

    [SerializeField] public Sprite cardFace;
    [SerializeField] public CardUsageType cardUsageType;
    [SerializeField] public DeckLogic dl;

    public void discardCard(Card card)
    {
        //Destroy(card);
        card.gameObject.SetActive(false);

        //dl.deck.Add(card);
        dl.discardPile.Add(card);
    }

    public abstract void useItem(PlayerBase player);
}
