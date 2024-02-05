using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrewOfMight : Card
{
    public override void useItem(PlayerBase player)
    {
        //MainManager.Instance.cl.enemyBase.reduceEnemyMight(1);
        MainManager.Instance.clBase.enemyBase.reduceEnemyMight(1);

        dl.discardPile.Add(this);
        player.ih.hideCardOptions();
    }

}
