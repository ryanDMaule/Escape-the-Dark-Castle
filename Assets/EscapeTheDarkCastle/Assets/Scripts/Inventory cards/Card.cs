using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{

    [SerializeField] public Sprite cardFace;

    public void discardCard(Card card, DeckLogic dl)
    {
        //Destroy(card);
        card.gameObject.SetActive(false);

        dl.deck.Add(card);
    }
}
