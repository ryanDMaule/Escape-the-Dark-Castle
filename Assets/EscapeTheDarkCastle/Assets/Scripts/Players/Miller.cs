using InnerDriveStudios.DiceCreator;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class Miller : PlayerBase
{
    private readonly int might = 3;
    private int cunning = 4;
    private int wisdom = 1;

    private void SetMillerDice(string rollValue, EnemyBase enemy)
    {
        switch (rollValue)
        {
            case "0":
                enemy.reduceEnemyCunning(1);
                break;

            case "1":
                enemy.reduceEnemyMight(1);
                break;

            case "2":
                enemy.reduceEnemyCunning(1);
                break;

            case "3":
                enemy.reduceEnemyCunning(2);
                setShieldActiveState(true);
                break;

            case "4":
                enemy.reduceEnemyWisdom(1);
                break;

            case "5":
                enemy.reduceEnemyMight(2);
                setShieldActiveState(true);
                break;

            default:
                break;
        }
    }

    public void rollLogic(EnemyBase enemy)
    {
        StartCoroutine(rollDelay(enemy));
    }

    IEnumerator rollDelay(EnemyBase enemy)
    {
        Die characterDie = getCharacterDie();
        if (!characterDie.isRolling)
        {
            Debug.Log("ROLL");


            //show the dice and spawn it rolling in the air above the camera
            //characterDie.transform.position = new Vector3(0, 15, 0);
            characterDie.gameObject.SetActive(true);
            characterDie.Roll();

            while (characterDie.isRolling)
            {
                yield return null;
            }

            //when rolling is false hide the die itself
            characterDie.gameObject.SetActive(false);
            getCharacterDieButton().gameObject.SetActive(false);

            string dieValue = characterDie.dieSides.GetDieSideMatchInfo().closestMatch.ValuesAsString();
            SetMillerDice(dieValue, enemy);
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
