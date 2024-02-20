using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkeletalBeastFormatChapter : MonoBehaviour
{
    [Header("Other")]
    [SerializeField] public EnemyBase enemy;
    [SerializeField] public SkeletalBeastChapterLogic clBase;

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

        var images = controlBlock.GetComponentsInChildren<Image>();
        foreach (var image in images)
        {
            if (image.tag == "combatState")
            {
                player.combatState = image;
                image.gameObject.SetActive(false);
                break;
            }
        }

        var textFields = controlBlock.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var item in textFields)
        {
            if (item.tag == "HUD-name")
            {
                item.text = player.name;
                break;
            }
        }

        var buttons = controlBlock.GetComponentsInChildren<Button>();
        SoundFXPlayer soundFX = FindFirstObjectByType<SoundFXPlayer>();
        foreach (var button in buttons)
        {
            //ROLL
            if (button.tag == "ChapterDie")
            {
                player.rollChapterDieButton = button;

                button.onClick.AddListener(() => player.rollEnemyHealthNew(enemy, button));
                button.onClick.AddListener(() => soundFX.PlayButtonClick());

                continue;
            }

            //FIGHT
            if (button.tag == "fightButton")
            {
                player.fightButton = button;

                button.onClick.AddListener(() => player.fightState());
                button.onClick.AddListener(() => soundFX.PlayAttack());

                button.gameObject.SetActive(false);
                continue;
            }

            //REST
            if (button.tag == "restButton")
            {
                player.restButton = button;

                button.onClick.AddListener(() => player.restNew());
                button.onClick.AddListener(() => soundFX.PlayRestSound());

                button.gameObject.SetActive(false);
                continue;
            }
        }
    }
}
