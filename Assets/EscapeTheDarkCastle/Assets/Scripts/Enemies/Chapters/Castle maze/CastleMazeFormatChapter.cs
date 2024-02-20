using InnerDriveStudios.DiceCreator;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CastleMazeFormatChapter : MonoBehaviour
{
    [Header("Other")]
    [SerializeField] public EnemyBase enemy;
    [SerializeField] public CastleMazeChapterLogic clBase;

    //control blocks = the rest, fight and roll UI sections for the players
    [Header("Player control blocks")]
    [SerializeField] public GameObject player1ControlBlock;
    [SerializeField] public GameObject player2ControlBlock;
    [SerializeField] public GameObject player3ControlBlock;
    [SerializeField] public GameObject player4ControlBlock;

    public void Start()
    {
        clearUnusedObjects();
        assignCl();
    }

    private void assignCl()
    {
        MainManager.Instance.clBase = clBase;
    }

    private void clearUnusedObjects()
    {
        int playerCount = MainManager.Instance.Players.Count;

        switch (playerCount)
        {
            case 2:
                Destroy(player3ControlBlock);
                Destroy(player4ControlBlock);

                formatPlayerControlBlock(player1ControlBlock, MainManager.Instance.Players[0]);
                formatPlayerControlBlock(player2ControlBlock, MainManager.Instance.Players[1]);

                break;

            case 3:
                Destroy(player4ControlBlock);

                formatPlayerControlBlock(player1ControlBlock, MainManager.Instance.Players[0]);
                formatPlayerControlBlock(player2ControlBlock, MainManager.Instance.Players[1]);
                formatPlayerControlBlock(player3ControlBlock, MainManager.Instance.Players[2]);

                break;

            case 4:
                formatPlayerControlBlock(player1ControlBlock, MainManager.Instance.Players[0]);
                formatPlayerControlBlock(player2ControlBlock, MainManager.Instance.Players[1]);
                formatPlayerControlBlock(player3ControlBlock, MainManager.Instance.Players[2]);
                formatPlayerControlBlock(player4ControlBlock, MainManager.Instance.Players[3]);

                break;

            default:
                Debug.Log("Error!");
                break;
        }

    }

    private void formatPlayerControlBlock(GameObject controlBlock, PlayerBase player)
    {
        player.roundHud = controlBlock;

        var textFields = controlBlock.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var item in textFields)
        {
            if (item.tag == "HUD-name")
            {
                item.text = player.name;
                break;
            }
        }

        formatRoll(controlBlock, player);
    }

    public void formatBlock()
    {
        int playerCount = MainManager.Instance.Players.Count;

        switch (playerCount)
        {
            case 2:
                formatRoll(player1ControlBlock, MainManager.Instance.Players[0]);
                formatRoll(player2ControlBlock, MainManager.Instance.Players[1]);

                break;

            case 3:
                formatRoll(player1ControlBlock, MainManager.Instance.Players[0]);
                formatRoll(player2ControlBlock, MainManager.Instance.Players[1]);
                formatRoll(player3ControlBlock, MainManager.Instance.Players[2]);

                break;

            case 4:
                formatRoll(player1ControlBlock, MainManager.Instance.Players[0]);
                formatRoll(player2ControlBlock, MainManager.Instance.Players[1]);
                formatRoll(player3ControlBlock, MainManager.Instance.Players[2]);
                formatRoll(player4ControlBlock, MainManager.Instance.Players[3]);

                break;

            default:
                Debug.Log("Error!");
                break;
        }
    }

    private void formatRoll(GameObject controlBlock, PlayerBase player)
    {
        var buttons = controlBlock.GetComponentsInChildren<Button>();
        SoundFXPlayer soundFX = FindFirstObjectByType<SoundFXPlayer>();
        foreach (var button in buttons)
        {
            //ROLL
            if (button.tag == "ChapterDie")
            {
                player.rollChapterDieButton = button;

                button.interactable = true;

                // replace this with normal character die roll
                button.onClick.AddListener(() => standardTurn(player, enemy));
                button.onClick.AddListener(() => button.interactable = false);
                button.onClick.AddListener(() => soundFX.PlayButtonClick());

                break;
            }
        }
    }

    public void standardTurn(PlayerBase player, EnemyBase enemy)
    {
        StartCoroutine(standardTurnIE(player, enemy));
    }

    IEnumerator standardTurnIE(PlayerBase player, EnemyBase enemy)
    {
        Die characterDie = player.getCharacterDie();

        if (!characterDie.isRolling)
        {
            //show the dice and spawn it rolling in the air above the camera
            //characterDie.transform.position = new Vector3(0, 15, 0);
            characterDie.gameObject.SetActive(true);

            characterDie.Roll();
            player.RollStarted.Raise();

            while (characterDie.isRolling)
            {
                yield return null;
            }
            player.hasRolled = true;

            //when rolling is false hide the die itself
            characterDie.gameObject.SetActive(false);

            //deal damage
            string dieValue = characterDie.dieSides.GetDieSideMatchInfo().closestMatch.ValuesAsString();
            player.getPlayerDieValue(dieValue, enemy);

            MainManager.Instance.playersRolled();
        }
    }

}
