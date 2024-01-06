using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartiallyRottenApple : Card
{
    public override void useItem(PlayerBase player)
    {
        player.IncreaseHealth(1);
        dl.discardPile.Add(this);
        player.ih.hideCardOptions();
    }

}
