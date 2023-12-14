using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScreenLogic : MonoBehaviour
{

    public Button continueButton;
    public GameObject drawCardAsset;
    public DeckLogic dl;

    private int cardsToDraw = MainManager.Instance.drawCards;

    public void FlipCard()
    {
        dl.DrawCard();
        cardsToDraw--;

        if(cardsToDraw == 0)
        {
            drawCardAsset.gameObject.SetActive(false);
        }
        //dl.DrawCard(this.transform);
        //this.gameObject.SetActive(false);
    }

}
