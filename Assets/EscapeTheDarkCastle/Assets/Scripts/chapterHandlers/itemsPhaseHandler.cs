using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class itemsPhaseHandler : MonoBehaviour
{

    [SerializeField] public GameObject drawCardObject;
    [SerializeField] public Image drawnCardImage;
    [SerializeField] public GameObject cardOptionsOverlay;

    [SerializeField] public Button discardButton;
    [SerializeField] public Button continueButton;

    [SerializeField] public GameObject itemsTotalText;
    [SerializeField] public GameObject noItemsText;

    [SerializeField] public GameObject playerList;

    [SerializeField] public Button assignPlayer1;
    [SerializeField] public Button assignPlayer2;
    [SerializeField] public Button assignPlayer3;
    [SerializeField] public Button assignPlayer4;

    [SerializeField] public GameObject YouSelection;
    [SerializeField] public Button YouContinueButton;

    [SerializeField] public Scenes scenes;

    private int drawTotal;

    void Start()
    {
        drawTotal = MainManager.Instance.drawCards;

        MainManager.Instance.updateGameState(GameState.ITEMS_PHASE);
        Debug.Log("SET dl");

        drawCardObject.GetComponent<BackOfCard>().dl = MainManager.Instance.dl;

        MainManager.Instance.dl.setDrawnCardImage(drawnCardImage);
        MainManager.Instance.dl.setCardOptionHud(cardOptionsOverlay);

        discardButton.onClick.AddListener(() => MainManager.Instance.dl.discardCard());

        hideUnusedPlayers();

        cardOptionsOverlay.SetActive(false);

        allowContinue();

        //Revoke YOU from any players
        foreach(var player in MainManager.Instance.Players)
        {
            player.setYouFalse();
        }

        loadNextChapter();
    }

    //checks if the continue button on the items phase is interactable and formats the options accordingly
    private void allowContinue()
    {
        Debug.Log("drawTotal: " + drawTotal);

        if (drawTotal == 0)
        {
            //set the draw card to false
            drawCardObject.gameObject.SetActive(false);

            //hide the position total text
            itemsTotalText.gameObject.SetActive(false);

            //show some text saying "no itenms remaining"
            noItemsText.gameObject.SetActive(true);

            //make continue button interactable
            continueButton.interactable = true;
        } else
        {
            //set items total text
            if (drawTotal > 1)
            {
                itemsTotalText.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "You have " + drawTotal + " items to deal with.";
            } else
            {
                itemsTotalText.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "You have " + drawTotal + " item to deal with.";
            }
        }
    }

    public void reduceDrawTotal()
    {
        drawTotal--;
        allowContinue();
    }

    private void hideUnusedPlayers()
    {
        int playerCount = MainManager.Instance.Players.Count;
        int index = 0;

        if(playerCount >= 2)
        {
            var playerButtons = playerList.GetComponentsInChildren<Button>();

            foreach (var button in playerButtons)
            {
                if (playerCount == 2)
                {
                    if (button.tag == "player3")
                    {
                        Destroy(button.gameObject);
                    }
                    if (button.tag == "player4")
                    {
                        Destroy(button.gameObject);
                    }
                }
                if (playerCount == 3)
                {
                    if (button.tag == "player4")
                    {
                        Destroy(button.gameObject);
                    }
                }

                formatAssignButton(button, index);
                index++;
            }
            playerList.SetActive(false);
        }
    }

    private void formatAssignButton(Button button, int index)
    {
        try
        {
            //set button text to player name
            button.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = MainManager.Instance.Players[index].name;

            //handle button onClick
            button.onClick.AddListener(() => MainManager.Instance.Players[index].addInventoryItem(MainManager.Instance.dl));
            button.onClick.AddListener(() => closePlayerList());
            button.onClick.AddListener(() => reduceDrawTotal());
        }
        catch (Exception ex)
        {
            Debug.Log("EXCEPTION:" + ex);
        }
    }

    public void togglePlayerListVisibility()
    {
        if (playerList.gameObject.activeInHierarchy)
        {
            playerList.gameObject.SetActive(false);
        } else
        {
            playerList.gameObject.SetActive(true);
        }
    }

    //call this on card flipped or broadcat listeners for players using, trading or discarding cards in their inventory.
    //make a new version of this where we can pass the player whose inventory has chaanged and do the check only on that player.
    public void playerInventoryUpdate()
    {
        Debug.Log("playerInventoryUpdate");

        //get the shown items size
        int drawnCardSize = MainManager.Instance.dl.drawnCard.size;

        for (var i = 0; i < MainManager.Instance.Players.Count; i++)
        {
            switch (i)
            {
                case 0:
                    enableCardCheck(assignPlayer1, i, drawnCardSize);
                    break;

                case 1:
                    enableCardCheck(assignPlayer2, i, drawnCardSize);
                    break;

                case 2:
                    enableCardCheck(assignPlayer3, i, drawnCardSize);
                    break;

                case 3:
                    enableCardCheck(assignPlayer4, i, drawnCardSize);
                    break;


                default:
                    Debug.Log("Error!");
                    break;
            }
        }
    }
    private void enableCardCheck(Button button, int index, int drawnCardSize)
    {
        //check if the players have the amount of space for that item
        if (drawnCardSize <= MainManager.Instance.Players[index].InventorySlotsFree())
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }

    public void openPlayerList()
    {   
         playerList.gameObject.SetActive(true);
    }

    public void closePlayerList()
    {
        playerList.gameObject.SetActive(false);
    }

    public void showYouSelectionPanel()
    {
        YouSelection.gameObject.SetActive(true);
    }

    private void loadNextChapter()
    {
        print("loadNextChapter");

        //STANDARD LOGIC
        YouContinueButton.onClick.AddListener(() => MainManager.Instance.LoadNextChapter());

        //LOAD WHATEVER CHAPTER
        //YouContinueButton.onClick.AddListener(() => scenes.loadVictory());
        //YouContinueButton.onClick.AddListener(() => scenes.loadTest());
    }

}
