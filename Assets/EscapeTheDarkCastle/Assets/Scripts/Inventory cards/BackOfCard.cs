using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackOfCard : MonoBehaviour
{
    public DeckLogic dl;

    public void FlipCard()
    {
        dl.DrawCard(this.transform);
        Destroy(this);
    }
}