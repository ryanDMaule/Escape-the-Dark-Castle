using UnityEngine;

public class Skeleton : EnemyBase
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

    //perhaps remove it directly influencing chapter logic, have it just set anything relevant to enemy and have chapter logic listen for this finishing.
    public void option1(ChapterLogic cl)
    {
        setDamage(2);
        enemy_damage_image.sprite = damage2;

        cl.setEnemyHealthPhase();
    }

    public void option1New(ChapterLogicNew cl)
    {
        setDamage(2);
        enemy_damage_image.sprite = damage2;

        cl.setEnemyHealthPhase();
    }

    public void option2(ChapterLogic cl)
    {
        setEnemyMight(2);
        setDamage(1);
        enemy_damage_image.sprite = damage1;

        cl.setEnemyHealthPhase();
    }

    public void option2New(ChapterLogicNew cl)
    {
        setEnemyMight(2);
        setDamage(1);
        enemy_damage_image.sprite = damage1;

        cl.setEnemyHealthPhase();
    }

}
