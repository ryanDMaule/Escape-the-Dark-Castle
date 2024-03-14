using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelsChapterLogic : ChapterLogicBase
{
    #region globalVariables

    [Header("Game objects")]
    [SerializeField] public GameObject descriptionSection;
    [SerializeField] public GameObject combatOptions;
    [SerializeField] public GameObject winSection;


    #endregion

    void Start()
    {
        MainManager.Instance.updateGameState(GameState.CHAPTER);
        MainManager.Instance.clBase = this;
    }

    #region HUD_methods

    public override void setCombatOptionsHUD()
    {
        combatOptions.gameObject.SetActive(true);
    }

    public void option1()
    {
        MainManager.Instance.drawCards = 2;

        combatOptions.gameObject.SetActive(false);
        descriptionSection.gameObject.SetActive(false);
        winSection.gameObject.SetActive(true);

        PlayerBase player = MainManager.Instance.getYou();
        player.RedcuceHealth(2);

    }

    #endregion
}
