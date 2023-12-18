using InnerDriveStudios.DiceCreator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smith : PlayerBase
{
    private readonly int might = 4;
    private int cunning = 3;
    private int wisdom = 1;

    private void setDice(string rollValue, EnemyBase enemy)
    {
        //implement
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
            setDice(dieValue, enemy);
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
