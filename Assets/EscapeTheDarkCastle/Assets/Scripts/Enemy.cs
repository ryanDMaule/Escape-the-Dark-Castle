using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    [SerializeField] public Text enemy_might_text;
    [SerializeField] public Text enemy_cunning_text;
    [SerializeField] public Text enemy_wisdom_text;

    private int enemy_might_int = 0;
    private int enemy_cunning_int = 0;
    private int enemy_wisdom_int = 0;


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
        } else
        {
            enemy_might_int -= value;
        }
        enemy_might_text.text = enemy_might_int.ToString();
        //Debug.Log("reduceEnemyMight: " + enemy_might_int.ToString());
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
        //Debug.Log("reduceEnemyCunning: " + enemy_cunning_int.ToString());
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
        //Debug.Log("reduceEnemyWisdom: " + enemy_wisdom_int.ToString());
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
