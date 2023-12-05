using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] public Image enemyDamage;

    [SerializeField] public Sprite damage1;
    [SerializeField] public Sprite damage2;

    [SerializeField] public Text enemy_might_text;
    [SerializeField] public Text enemy_cunning_text;
    [SerializeField] public Text enemy_wisdom_text;

    private int enemy_might_int = 0;
    private int enemy_cunning_int = 0;
    private int enemy_wisdom_int = 0;

    private int damage = 1;

    [SerializeField] public ChapterLogic cl;

    public void setDamage(int value)
    {
        damage = value;
    }

    public int getDamage()
    {
        return damage;
    }

    public void setEnemyMight(int value)
    {
        enemy_might_int += value;
        enemy_might_text.text = enemy_might_int.ToString();
    }

    public void setEnemyCunning(int value)
    {
        enemy_cunning_int += value;
        enemy_cunning_text.text = enemy_cunning_int.ToString();
    }

    public void setEnemyWisdom(int value)
    {
        enemy_wisdom_int += value;
        enemy_wisdom_text.text = enemy_wisdom_int.ToString();
    }

    public void reduceEnemyMight(int value)
    {
        if (enemy_might_int - value <= 0)
        {
            enemy_might_int = 0;
        }
        else
        {
            enemy_might_int -= value;
        }
        enemy_might_text.text = enemy_might_int.ToString();
    }

    public void reduceEnemyCunning(int value)
    {
        if (enemy_cunning_int - value <= 0)
        {
            enemy_cunning_int = 0;
        }
        else
        {
            enemy_cunning_int -= value;
        }
        enemy_cunning_text.text = enemy_cunning_int.ToString();
    }

    public void reduceEnemyWisdom(int value)
    {
        if (enemy_wisdom_int - value <= 0)
        {
            enemy_wisdom_int = 0;
        }
        else
        {
            enemy_wisdom_int -= value;
        }
        enemy_wisdom_text.text = enemy_wisdom_int.ToString();
    }

    public bool enemyDead()
    {
        if (enemy_might_int == 0 && enemy_cunning_int == 0 && enemy_wisdom_int == 0)
        {
            Debug.Log("enemyDead: TRUE");
            return true;
        }
        Debug.Log("enemyDead: FALSE");
        return false;
    }

}
