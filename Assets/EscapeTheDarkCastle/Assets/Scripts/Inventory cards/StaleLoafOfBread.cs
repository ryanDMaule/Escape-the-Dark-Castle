using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaleLoafOfBread : Card
{
    public override void useItem(PlayerBase player)
    {
        Debug.Log("EAT THE FUCKING BREAD");

        player.IncreaseHealth(2);
        dl.discardPile.Add(this);
        player.ih.hideCardOptions();
    }
}
