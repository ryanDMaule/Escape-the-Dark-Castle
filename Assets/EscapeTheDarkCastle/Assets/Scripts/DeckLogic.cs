using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckLogic : MonoBehaviour
{

    [Header("Card stacks")]
    public List<Card> deck = new List<Card>();
    public List<Card> discardPile = new List<Card>();

    [Header("Drawn card")]
    [SerializeField] Image drawnCardimage;
    public Card drawnCard;

    [Header("Game objects")]
    [SerializeField] GameObject CardOptionButtons;

    public void DrawCard()
    {
        Debug.Log("DrawCard - DECK SIZE: " + deck.Count);

        if (deck.Count >= 1)
        {
            Card randCard = deck[Random.Range(0, deck.Count)];

            drawnCardimage.gameObject.SetActive(true);
            drawnCardimage.sprite = randCard.cardFace;

            drawnCard = randCard;
            Debug.Log("Drawn Card : " + drawnCard.name);

            deck.Remove(randCard);

            CardOptionButtons.gameObject.SetActive(true);
        } else
        {
            if(discardPile.Count >= 1)
            {
                foreach (Card card in discardPile)
                {
                    deck.Add(card);
                }
                discardPile.Clear();
                DrawCard();
            } else
            {
                Debug.Log("NO MORE CARDS");
            }
        }
    }

    public void discardCard()
    {
        discardPile.Add(drawnCard);
        hide();
    }

    public void hide()
    {
        drawnCardimage.gameObject.SetActive(false);
        drawnCard.gameObject.SetActive(false);
        CardOptionButtons.gameObject.SetActive(false);
    }

    public void setDrawnCardImage(Image image)
    {
        Debug.Log("setDrawnCardImage");
        this.drawnCardimage = image;
    }
    public void setCardOptionHud(GameObject overlay)
    {
        Debug.Log("setCardOptionHud");
        this.CardOptionButtons = overlay;
    }

}
