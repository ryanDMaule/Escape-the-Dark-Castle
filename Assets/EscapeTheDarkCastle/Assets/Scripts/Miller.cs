using InnerDriveStudios.DiceCreator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miller : PlayerBase
{
    private void SetMillerDice(string rollValue, Enemy enemy)
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

    public int mightDamage = 0;
    public int cunningDamage = 0;
    public int wisdomDamage = 0;
    private void SetMillerDice(string rollValue)
    {
        switch (rollValue)
        {
            case "0":
                cunningDamage += 1;
                break;

            case "1":
                mightDamage += 1;
                break;

            case "2":
                cunningDamage += 1;
                break;

            case "3":
                cunningDamage += 2;
                setShieldActiveState(true);
                break;

            case "4":
                wisdomDamage += 1;
                break;

            case "5":
                mightDamage += 2;
                setShieldActiveState(true);
                break;

            default:
                break;
        }
    }

    public int getMightDamage()
    {
        return mightDamage;
    }
    public int getCunningDamage()
    {
        return cunningDamage;
    }
    public int getWisdomDamage()
    {
        return wisdomDamage;
    }

    public void rollLogic()
    {
        StartCoroutine(rollDelay());
    }

    IEnumerator rollDelay()
    {
        Die characterDie = getCharacterDie();
        if (!characterDie.isRolling)
        {
            Debug.Log("ROLL");


            //show the dice and spawn it rolling in the air above the camera
            characterDie.transform.position = new Vector3(0, 15, 0);
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
            SetMillerDice(dieValue);
        }

    }

}
