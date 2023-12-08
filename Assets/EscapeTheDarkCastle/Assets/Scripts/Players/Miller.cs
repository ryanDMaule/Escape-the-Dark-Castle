using InnerDriveStudios.DiceCreator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miller : PlayerBase
{
    public void printInventory()
    {
        if(Inventory.Count == 0)
        {
            //Debug.Log("MILLER INVENTORY - EMPTY");
        } else if(Inventory.Count == 1)
        {
            //Debug.Log("MILLER INVENTORY - SIZE 1");
            //Debug.Log("CARD 1 : " + Inventory[0].name);
        } else if (Inventory.Count == 2)
        {
            //Debug.Log("MILLER INVENTORY - SIZE 2");
            //Debug.Log("CARD 1 : " + Inventory[0].name);
            //Debug.Log("CARD 2 : " + Inventory[1].name);
        }
        else
        {
            //Debug.Log("MILLER - ERROR");
        }
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
