using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { DESCRIPTION, COMBAT_OPTIONS, SET_ENEMY_HEALTH, PREPERATION, PLAYER_TURN, ENEMY_TURN, WON, LOST }

public abstract class ChapterLogicBase : MonoBehaviour
{

    [Header("Death stuffs")]
    [SerializeField] public string deathBio = "";
    [SerializeField] public AudioClip deathClip;

    [Header("Other")]
    public BattleState state;
    [SerializeField] public EnemyBase enemyBase;

    public BattleState getState()
    {
        return state;
    }

    public void setDescriptionPhase()
    {
        state = BattleState.DESCRIPTION;
        setDescriptionHUD();
    }

    public void setCombatOptionsPhase()
    {
        state = BattleState.COMBAT_OPTIONS;
        setCombatOptionsHUD();
    }

    public virtual void setDescriptionHUD()
    {
        Debug.Log("setDescriptionHUD");
        
        enemyBase.SHOW_DESCRIPTION();
        enemyBase.hideOptionsHUD();
        enemyBase.SET_ENEMY_IMAGE_ONLY();        
    }

    public virtual void setCombatOptionsHUD()
    {
        Debug.Log("setCombatOptionsHUD");
        
        enemyBase.SHOW_DESCRIPTION();
        enemyBase.showOptionsHUD();
        enemyBase.SET_ENEMY_IMAGE_ONLY();        
    }

    public virtual void setWinHUD()
    {
        Debug.Log("setWinHUD");

        enemyBase.HIDE_DESCRIPTION();
        enemyBase.hideOptionsHUD();
        enemyBase.SET_ENEMY_ASSETS_VISIBLE();
    }

}
