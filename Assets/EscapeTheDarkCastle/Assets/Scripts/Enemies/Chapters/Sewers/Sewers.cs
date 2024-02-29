using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sewers : EnemyBase
{
    [Header("Option panels")]
    [SerializeField] GameObject option1_object;

    [SerializeField] GameObject panel_general;
    [SerializeField] GameObject panel_nominate;

    public override void showOptionsHUD()
    {
        background.gameObject.SetActive(true);
        option1_object.gameObject.SetActive(true);
    }

    public override void hideOptionsHUD()
    {
        background.gameObject.SetActive(false);
        option1_object.gameObject.SetActive(false);
    }

    public void nominateHUD()
    {
        panel_general.gameObject.SetActive(false);
        panel_nominate.gameObject.SetActive(true);
    }

    //the button on click logic for option 1
    public void option1(SewersChapterLogic cl)
    {
        MainManager.Instance.drawCards = 1;

        setEnemyCunning(3);

        setDamage(1);
        enemy_damage_image.sprite = damage1;
    }
}
