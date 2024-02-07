using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireDemon : EnemyBase
{
    [Header("Option panels")]
    [SerializeField] GameObject option1_object;
    [SerializeField] GameObject option2_object;

    private void Start()
    {
        setPlayerRollTotal();
    }

    //for the combat option cards, update the value that shows how many players need to roll for enemy health with the total players stored in Main manager
    private void setPlayerRollTotal()
    {
        var textFields = option2_object.GetComponentsInChildren<Text>();
        foreach (var textField in textFields)
        {
            if (textField.tag == "healthCounter")
            {
                textField.text = MainManager.Instance.Players.Count.ToString();
                break;
            }
        }
    }

    public override void showOptionsHUD()
    {
        base.showOptionsHUD();

        option1_object.gameObject.SetActive(true);
        option2_object.gameObject.SetActive(true);
    }

    public override void hideOptionsHUD()
    {
        base.hideOptionsHUD();

        option1_object.gameObject.SetActive(false);
        option2_object.gameObject.SetActive(false);
    }

    //the button on click logic for option 1
    public void option1(FireDemonChapterLogic cl)
    {
        cl.optionSelected = 1;

        //set won item cards
        MainManager.Instance.drawCards = 1;

        //hide description and show Win hud
        cl.setWinHUD();

        //apply damage to all players
        foreach(var player in MainManager.Instance.Players)
        {
            player.RedcuceHealth(2);
        }
    }

    //the button on click logic for option 2
    public void option2(FireDemonChapterLogic cl)
    {
        cl.optionSelected = 2;

        //set won item cards
        MainManager.Instance.drawCards = 1;

        //set enemy health and damage
        setEnemyCunning(1);
        setEnemyWisdom(1);

        setDamage(2);
        enemy_damage_image.sprite = damage2;

        //enter the next game phase
        cl.setEnemyHealthPhase();
    }
}
