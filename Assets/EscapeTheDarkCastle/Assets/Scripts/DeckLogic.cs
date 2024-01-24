using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckLogic : MonoBehaviour
{

    //card prefabs
    /*
    [SerializeField] public Card decayedBlade;
    [SerializeField] public Card staleLoafOfBread;
    [SerializeField] public Card rottenShield;
    [SerializeField] public Card crackedAxe;
    [SerializeField] public Card theReplicationStones;
    */
     
    public List<Card> deck = new List<Card>();
    public List<Card> discardPile = new List<Card>();

    [SerializeField] Image drawnCardimage;

    [SerializeField] GameObject CardOptionButtons;

    public Card drawnCard;

    public void DrawCardOld(Transform position)
    {
        if (deck.Count >= 1)
        {
            Card randCard = deck[Random.Range(0, deck.Count)];

            randCard.gameObject.SetActive(true);
            randCard.transform.position = position.position;

            drawnCard = randCard;

            deck.Remove(randCard);

            CardOptionButtons.gameObject.SetActive(true);
        }
        else
        {
            if (discardPile.Count >= 1)
            {
                foreach (Card card in discardPile)
                {
                    deck.Add(card);
                }
                discardPile.Clear();
                DrawCardOld(position);
            }
            else
            {
                Debug.Log("NO MORE CARDS");
            }
        }
    }

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
