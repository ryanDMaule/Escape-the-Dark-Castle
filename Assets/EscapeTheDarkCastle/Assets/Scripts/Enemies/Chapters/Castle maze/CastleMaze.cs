using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleMaze : EnemyBase
{
    [Header("Option panels")]
    [SerializeField] GameObject option1_object;

    private void Start()
    {
        setPlayerRollTotal();
    }

    //for the combat option cards, update the value that shows how many players need to roll for enemy health with the total players stored in Main manager
    private void setPlayerRollTotal()
    {
        var textFields = option1_object.GetComponentsInChildren<Text>();
        foreach (var textField in textFields)
        {
            if (textField.tag == "wisdom")
            {
                textField.text = MainManager.Instance.Players.Count.ToString();
                break;
            }
        }
    }

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

    //the button on click logic for option 1
    public void option1(CastleMazeChapterLogic cl)
    {
        MainManager.Instance.drawCards = 1;

        setEnemyWisdom(MainManager.Instance.Players.Count);

        setDamage(1);
        enemy_damage_image.sprite = damage1;

        cl.setEnemyHealthPhase();
    }
}
