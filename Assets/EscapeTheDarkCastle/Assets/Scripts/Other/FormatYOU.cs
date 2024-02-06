using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FormatYOU : MonoBehaviour
{
    [Header("Player objects")]
    [SerializeField] public GameObject player1;
    [SerializeField] public GameObject player2;
    [SerializeField] public GameObject player3;
    [SerializeField] public GameObject player4;

    [Header("Other")]
    [SerializeField] public RuntimeAnimatorController controller;
    [SerializeField] public Button continueButton;

    public void Start()
    {
        clearUnusedObjects();
        formatYouOptions();
    }

    //Deletes the placeholder locations that arent used
    private void clearUnusedObjects()
    {
        int playerCount = MainManager.Instance.Players.Count;

        switch (playerCount)
        {
            case 2:
                //players section
                Destroy(player3);
                Destroy(player4);

                break;

            case 3:
                Destroy(player4);

                break;

            default:
                Debug.Log("Error!");
                break;
        }

    }


    //dependant on the amount of player in MainManager.players (players selected on previous screen)
    //will determine how the HUD is spaced out, each of these players then call setHudDetails()
    public void formatYouOptions()
    {
        int playersCount = MainManager.Instance.Players.Count;

        switch (playersCount)
        {
            case 2:
                player1.SetActive(true);
                formatIndividualOption(Instantiate(MainManager.Instance.Players[0].hud, player1.transform), MainManager.Instance.Players[0]);

                player2.SetActive(true);
                formatIndividualOption(Instantiate(MainManager.Instance.Players[1].hud, player2.transform), MainManager.Instance.Players[1]);

                break;

            case 3:
                player1.SetActive(true);
                formatIndividualOption(Instantiate(MainManager.Instance.Players[0].hud, player1.transform), MainManager.Instance.Players[0]);

                player2.SetActive(true);
                formatIndividualOption(Instantiate(MainManager.Instance.Players[1].hud, player2.transform), MainManager.Instance.Players[1]);

                player3.SetActive(true);
                formatIndividualOption(Instantiate(MainManager.Instance.Players[2].hud, player3.transform), MainManager.Instance.Players[2]);

                break;

            case 4:
                player1.SetActive(true);
                formatIndividualOption(Instantiate(MainManager.Instance.Players[0].hud, player1.transform), MainManager.Instance.Players[0]);

                player2.SetActive(true);
                formatIndividualOption(Instantiate(MainManager.Instance.Players[1].hud, player2.transform), MainManager.Instance.Players[1]);

                player3.SetActive(true);
                formatIndividualOption(Instantiate(MainManager.Instance.Players[2].hud, player3.transform), MainManager.Instance.Players[2]);

                player4.SetActive(true);
                formatIndividualOption(Instantiate(MainManager.Instance.Players[3].hud, player4.transform), MainManager.Instance.Players[3]);

                break;

            default:
                break;
        }
    }

    public void formatIndividualOption(GameObject hudItem, PlayerBase player)
    {
        //resize box itself       
        hudItem.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);

        //update health text and font size
        var hudComponenets = hudItem.GetComponentsInChildren<Text>();
        foreach (var text in hudComponenets)
        {
            if (text.tag == "HUD-health")
            {
                text.text = player.currentHealth.ToString();
                text.fontSize = 44;

                break;
            }
        }
        //Add a controller for animations
        hudItem.AddComponent<Animator>().runtimeAnimatorController = controller;

        //Set on click listener to actually set player to YOU
        hudItem.GetComponent<Button>().onClick.AddListener(() => player.setYouTrue(continueButton));

        var hudImageComponenets = hudItem.GetComponentsInChildren<Image>();
        foreach (var image in hudImageComponenets)
        {
            if (image.tag == "YOU")
            {
                image.gameObject.SetActive(false);
                break;
            }
        }

        player.YOUSelectorItem = hudItem;
    }



}
