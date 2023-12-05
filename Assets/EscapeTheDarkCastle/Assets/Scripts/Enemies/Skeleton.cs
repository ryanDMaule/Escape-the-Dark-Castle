public class Skeleton : EnemyBase
{
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
