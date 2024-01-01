using InnerDriveStudios.DiceCreator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tanner : PlayerBase
{
    private readonly int might = 1;
    private int cunning = 3;
    private int wisdom = 4;

    public override void getPlayerDieValue(string rollValue, EnemyBase enemy)
    {
        switch (rollValue)
        {
            case "0":
                enemy.reduceEnemyCunning(1);
                break;

            case "1":
                enemy.reduceEnemyWisdom(1);
                break;

            case "2":
                enemy.reduceEnemyWisdom(1);
                break;

            case "3":
                enemy.reduceEnemyWisdom(2);
                setShieldActiveState(true);
                break;

            case "4":
                enemy.reduceEnemyMight(1);
                break;

            case "5":
                enemy.reduceEnemyCunning(2);
                setShieldActiveState(true);
                break;

            default:
                break;
        }
    }

    public override int getPlayerMight()
    {
        return might;
    }

    public override int getPlayerCunning()
    {
        return cunning;
    }

    public override int getPlayerWisdom()
    {
        return wisdom;
    }

}
