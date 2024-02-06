using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class HiddenPassageChapterLogic : MonoBehaviour
{
    #region globalVariables

    [Header("Game objects")]
    [SerializeField] public GameObject combatOptions;

    #endregion

    void Start()
    {
        MainManager.Instance.updateGameState(GameState.CHAPTER);
        MainManager.Instance.drawCards = MainManager.Instance.Players.Count;
        print("MainManager.Instance.drawCards: " + MainManager.Instance.drawCards);

        print("TOTAL PLAYERS: " + MainManager.Instance.Players.Count);

    }

    #region HUD_methods

    public void setCombatOptionsHUD()
    {
        combatOptions.gameObject.SetActive(true);
    }

    #endregion

}
