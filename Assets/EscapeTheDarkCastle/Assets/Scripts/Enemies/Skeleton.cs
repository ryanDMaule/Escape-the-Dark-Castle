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

    public void option1()
    {
        setDamage(2);
        enemyDamage.sprite = damage2;

        cl.setEnemyHealthPhase();
    }

    public void option2()
    {
        setEnemyMight(2);
        setDamage(1);
        enemyDamage.sprite = damage1;

        cl.setEnemyHealthPhase();
    }

}
