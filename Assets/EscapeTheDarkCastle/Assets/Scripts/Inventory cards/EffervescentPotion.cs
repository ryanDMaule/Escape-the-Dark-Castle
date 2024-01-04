using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffervescentPotion : Card
{
    public override void useItem(PlayerBase player)
    {
        player.setPotionProtectionState(true);
        dl.discardPile.Add(this);
        player.ih.hideCardOptions();
    }
}
