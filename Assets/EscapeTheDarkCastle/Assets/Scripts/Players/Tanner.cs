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
                determineEnemyDamage(ChapterDieOptions.CUNNING, enemy, this);
                break;

            case "1" or "2":
                determineEnemyDamage(ChapterDieOptions.WISDOM, enemy, this);
                break;

            case "3":
                enemy.reduceEnemyWisdom(2);
                setShieldActiveState(true);
                break;

            case "4":
                determineEnemyDamage(ChapterDieOptions.MIGHT, enemy, this);
                break;

            case "5":
                enemy.reduceEnemyCunning(2);
                setShieldActiveState(true);
                break;

            default:
                break;
        }
    }

    public override ChapterDieOptions getCharacterRollResult(string rollValue)
    {
        return rollValue switch
        {
            "0" or "5" => ChapterDieOptions.CUNNING,
            "4" => ChapterDieOptions.MIGHT,
            "1" or "2" or "3" => ChapterDieOptions.WISDOM,
            _ => ChapterDieOptions.FAIL,
        };
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
