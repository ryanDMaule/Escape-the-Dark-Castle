using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneBeast : EnemyBase
{
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
 
}
