using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideousCreature : EnemyBase
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
        var textFields = option1_object.GetComponentsInChildren<Text>();
        foreach (var textField in textFields)
        {
            if (textField.tag == "healthCounter")
            {
                textField.text = MainManager.Instance.Players.Count.ToString();
                break;
            }
        }

        textFields = option2_object.GetComponentsInChildren<Text>();
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
    public void option1(HideousCreatureChapterHandler cl)
    {
        MainManager.Instance.drawCards = 1;

        setEnemyMight(1);
        setDamage(3);
        enemy_damage_image.sprite = damage3;

        cl.setEnemyHealthPhase();
    }

    //the button on click logic for option 2
    public void option2(HideousCreatureChapterHandler cl)
    {
        MainManager.Instance.drawCards = 1;

        setEnemyMight(1);
        setEnemyWisdom(2);
        setDamage(2);
        enemy_damage_image.sprite = damage2;

        cl.setEnemyHealthPhase();
    }
}
