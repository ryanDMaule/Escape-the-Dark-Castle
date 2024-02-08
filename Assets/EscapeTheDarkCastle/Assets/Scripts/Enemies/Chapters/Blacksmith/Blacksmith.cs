using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blacksmith : EnemyBase
{
    [Header("Option panels")]
    [SerializeField] GameObject option1_object;
    [SerializeField] GameObject option2_object;

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
    public void option1(BlacksmithChapterLogic cl)
    {
        //set won item cards
        MainManager.Instance.drawCards = 1;

        //hide description and show Win hud
        cl.setWinHUD();
    }

    //the button on click logic for option 2
    public void option2(BlacksmithChapterLogic cl)
    {
        //set won item cards
        MainManager.Instance.drawCards = 2;

        //set enemy health and damage
        setEnemyCunning(1);

        setDamage(2);
        enemy_damage_image.sprite = damage2;

        cl.option2SuccessFormatting();

        //enter the next game phase
        cl.setEnemyHealthPhase();
    }
}
