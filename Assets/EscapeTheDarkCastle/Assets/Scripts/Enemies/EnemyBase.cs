using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Events")]
    public GameEvent EnemyDead;

    [Header("Damage total sprites")]
    [SerializeField] public Sprite damage1;
    [SerializeField] public Sprite damage2;
    [SerializeField] public Sprite damage3;

    [Header("Description phase assets")]
    [SerializeField] public ScrollRect description;
    [SerializeField] public Button description_button;
    [SerializeField] public Button audioButton;
    [SerializeField] public Image background;
    [SerializeField] Button backButton;
    [SerializeField] Button forwardButton;

    [Header("Enemy assets")]
    [SerializeField] public Image enemyImage;
    [SerializeField] public Image enemy_might;
    [SerializeField] public Image enemy_cunning;
    [SerializeField] public Image enemy_wisdom;
    [SerializeField] public Text enemy_might_text;
    [SerializeField] public Text enemy_cunning_text;
    [SerializeField] public Text enemy_wisdom_text;
    [SerializeField] public Image enemy_damage_image;

    [Header("Enemy values")]
    private int enemy_might_int = 0;
    private int enemy_cunning_int = 0;
    private int enemy_wisdom_int = 0;
    private int damage = 1;

    #region combat_options_stuffs

    public virtual void showOptionsHUD()
    {
        background.gameObject.SetActive(true);
        try
        {
            backButton.gameObject.SetActive(false);
            forwardButton.gameObject.SetActive(true);
        }
        catch (NullReferenceException ex)
        {
            //Debug.LogException(ex, this);
        }
    }

    public virtual void hideOptionsHUD()
    {
        background.gameObject.SetActive(false);
        try
        {
            backButton.gameObject.SetActive(false);
            forwardButton.gameObject.SetActive(false);
        }
        catch (NullReferenceException ex)
        {
            //Debug.LogException(ex, this);
        }
    }

    #endregion

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

    public void resetEnemyWisdom(int value) {
        enemy_wisdom_int = value;
        enemy_wisdom_text.text = enemy_wisdom_int.ToString();
    }

    public int getEnemyMight()
    {
        return enemy_might_int;
    }

    public int getEnemyCunning()
    {
        return enemy_cunning_int;
    }

    public int getEnemyWisdom()
    {
        return enemy_wisdom_int;
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

            EnemyDead.Raise();
            return true;
        }
        Debug.Log("enemyDead: FALSE");
        return false;
    }


    #region HUD_methods
    public void SET_ENEMY_IMAGE_ONLY()
    {
        enemyImage.gameObject.SetActive(true);
        enemy_might.gameObject.SetActive(false);
        enemy_cunning.gameObject.SetActive(false);
        enemy_wisdom.gameObject.SetActive(false);
        enemy_damage_image.gameObject.SetActive(false);
        enemy_might_text.gameObject.SetActive(false);
        enemy_cunning_text.gameObject.SetActive(false);
        enemy_wisdom_text.gameObject.SetActive(false);
    }
    public void SET_ENEMY_ASSETS_VISIBLE()
    {
        enemyImage.gameObject.SetActive(true);
        enemy_might.gameObject.SetActive(true);
        enemy_cunning.gameObject.SetActive(true);
        enemy_wisdom.gameObject.SetActive(true);
        enemy_damage_image.gameObject.SetActive(true);
        enemy_might_text.gameObject.SetActive(true);
        enemy_cunning_text.gameObject.SetActive(true);
        enemy_wisdom_text.gameObject.SetActive(true);
    }

    public void SHOW_DESCRIPTION()
    {
        description.gameObject.SetActive(true);
        description_button.gameObject.SetActive(true);
        audioButton.gameObject.SetActive(true);
    }

    public void HIDE_DESCRIPTION()
    {
        description.gameObject.SetActive(false);
        description_button.gameObject.SetActive(false);
        audioButton.gameObject.SetActive(false);
    }

    #endregion


}
