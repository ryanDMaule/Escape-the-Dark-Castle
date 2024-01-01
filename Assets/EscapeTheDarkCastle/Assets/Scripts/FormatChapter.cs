using UnityEngine;
using UnityEngine.UI;

public class FormatChapter : MonoBehaviour
{
    [SerializeField] public EnemyBase enemy;

    [SerializeField] public GameObject player1ControlBlock;
    [SerializeField] public GameObject player2ControlBlock;
    [SerializeField] public GameObject player3ControlBlock;
    [SerializeField] public GameObject player4ControlBlock;

    [SerializeField] public ChapterLogicNew cl;

    public void Start()
    {
        clearUnusedObjects();
        assignCl();
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

    private void assignCl()
    {
        MainManager.Instance.cl = cl;
    }

    private void formatPlayerControlBlock(GameObject controlBlock, PlayerBase player)
    {
        var textFields = controlBlock.GetComponentsInChildren<Text>();
        foreach (var item in textFields)
        {
            if (item.tag == "HUD-name")
            {
                item.text = player.name;
                break;
            }
        }

        var buttons = controlBlock.GetComponentsInChildren<Button>();
        foreach (var button in buttons)
        {
            //ROLL
            if (button.tag == "ChapterDie")
            {
                button.onClick.AddListener(() => player.rollEnemyHealthNew(enemy, button));
                continue;
            }

            //FIGHT
            if (button.tag == "fightButton")
            {
                button.onClick.AddListener(() => player.fightState());
                button.gameObject.SetActive(false);
                continue;
            }

            //REST
            if (button.tag == "restButton")
            {
                //button.onClick.AddListener(() => player.rest());
                button.gameObject.SetActive(false);
                continue;
            }
        }
    }


}
