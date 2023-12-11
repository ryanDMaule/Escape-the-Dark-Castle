using InnerDriveStudios.DiceCreator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miller : PlayerBase
{
    public void printInventory()
    {
        Debug.Log("SLOT 1 : " + InventoryArray[0].name);
        Debug.Log("SLOT 2 : " + InventoryArray[1].name);
    }
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

}
