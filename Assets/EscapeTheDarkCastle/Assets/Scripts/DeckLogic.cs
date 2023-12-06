using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DeckLogic : MonoBehaviour
{

    public List<Card> deck = new List<Card>();
    public Transform location;

    public void DrawCard()
    {
        Debug.Log("DrawCard");

        if (deck.Count >= 1)
        {
            Card randCard = deck[Random.Range(0, deck.Count)];
            randCard.gameObject.SetActive(true);
            randCard.transform.position = location.position;
            deck.Remove(randCard);
        }
    }

    public void DrawCard(Transform position)
    {
        Debug.Log("DrawCard");

        if (deck.Count >= 1)
        {
            Card randCard = deck[Random.Range(0, deck.Count)];
            randCard.gameObject.SetActive(true);
            randCard.transform.position = position.position;
            deck.Remove(randCard);
        }
    }

}
