using UnityEngine;
using UnityEngine.UI;

public class FormatHud : MonoBehaviour
{
    [SerializeField] public GameObject placeholder1;
    [SerializeField] public GameObject placeholder2;
    [SerializeField] public GameObject placeholderMid;
    [SerializeField] public GameObject placeholder3;
    [SerializeField] public GameObject placeholder4;

    [SerializeField] public GameObject player1;
    [SerializeField] public GameObject player2;
    [SerializeField] public GameObject player3;
    [SerializeField] public GameObject player4;   

    [SerializeField] public InventoryHandler ih;   
    [SerializeField] public GameObject dieCollection;  
    
    public void Start()
    {
        formatHud();
        clearUnusedObjects();
    }

    private void clearUnusedObjects()
    {
        int playerCount = MainManager.Instance.Players.Count;

        switch (playerCount)
        {
            case 2:
                //players section
                Destroy(player3);
                Destroy(player4);

                //inventory HUD section
                Destroy(placeholder2);
                Destroy(placeholderMid);
                Destroy(placeholder3);

                break;

            case 3:
                Destroy(player4);

                Destroy(placeholder2);
                Destroy(placeholder3);

                break;

            case 4:
                Destroy(placeholderMid);

                break;

            default:
                Debug.Log("Error!");
                break;
        }

    }

    //dependant on the amount of player in MainManager.players (players selected on previous screen)
    //will determine how the HUD is spaced out, each of these players then call setHudDetails()
    public void formatHud()
    {
        int playersCount = MainManager.Instance.Players.Count;

        switch (playersCount)
        {
            case 2:
                placeholder1.SetActive(true);
                setHudDetails(MainManager.Instance.Players[0], player1, placeholder1);

                placeholder4.SetActive(true);
                setHudDetails(MainManager.Instance.Players[1], player2, placeholder4);
                break;

            case 3:
                placeholder1.SetActive(true);
                setHudDetails(MainManager.Instance.Players[0], player1, placeholder1);

                placeholderMid.SetActive(true);
                setHudDetails(MainManager.Instance.Players[1], player2, placeholderMid);

                placeholder4.SetActive(true);
                setHudDetails(MainManager.Instance.Players[2], player3, placeholder4);
                break;

            case 4:
                placeholder1.SetActive(true);
                setHudDetails(MainManager.Instance.Players[0], player1, placeholder1);

                placeholder2.SetActive(true);
                setHudDetails(MainManager.Instance.Players[1], player2, placeholder2);

                placeholder3.SetActive(true);
                setHudDetails(MainManager.Instance.Players[2], player3, placeholder3);

                placeholder4.SetActive(true);
                setHudDetails(MainManager.Instance.Players[3], player4, placeholder4);
                break;

            default:
                break;
        }
    }

    //Instantiates the passed players characterDie and a chapter die to the die container
    //Instantiates the passed players specific HUD prefab to the placeholder position provided (making a child gameObject beneath placeholder)
    //We then call assignPlayer()
    public void setHudDetails(PlayerBase player, GameObject playerInventoryContainer, GameObject hud)
    {
        var chapterDie = Instantiate(player.chapterDiePrefab, dieCollection.transform);
        player.GetComponent<PlayerBase>().chapterDie = chapterDie;
        chapterDie.gameObject.SetActive(false);

        var characterDie = Instantiate(player.characterDiePrefab, dieCollection.transform);
        player.GetComponent<PlayerBase>().characterDie = characterDie;
        characterDie.gameObject.SetActive(false);

        Instantiate(player.hud, hud.transform);
        formatInventory(player, playerInventoryContainer, hud);
        setInventoryAnimator(player, hud);
    }

    private void formatInventory(PlayerBase player, GameObject playerInventoryContainer, GameObject hud)
    {
        player.GetComponent<PlayerBase>().ih = ih;

        var playerObjectRectObjects = playerInventoryContainer.GetComponentsInChildren<RectTransform>();
        foreach (var item in playerObjectRectObjects)
        {
            if (item.tag == "Inventory")
            {
                player.GetComponent<PlayerBase>().panel = item.gameObject;
                break;
            }
        }

        var hudComponenets = hud.GetComponentsInChildren<Text>();
        foreach (var text in hudComponenets)
        {
            if (text.tag == "HUD-health")
            {
                player.GetComponent<PlayerBase>().healthText = text;
                text.text = player.currentHealth.ToString();

                break;
            }
        }

        var textFields = playerInventoryContainer.GetComponentsInChildren<Text>();
        foreach (var text in textFields)
        {
            if (text.tag == "might")
            {
                text.text = player.getPlayerMight().ToString();
                continue;
            }

            if (text.tag == "cunning")
            {
                text.text = player.getPlayerCunning().ToString();
                continue;
            }

            if (text.tag == "wisdom")
            {
                text.text = player.getPlayerWisdom().ToString();
                continue;
            }
        }

        var hudImageobjects = playerInventoryContainer.GetComponentsInChildren<Image>();
        foreach (var slot in hudImageobjects)
        {
            if (slot.tag == "InventorySlot1")
            {
                player.GetComponent<PlayerBase>().InventorySlot1 = slot;
                continue;
            }
            if (slot.tag == "InventorySlot2")
            {
                player.GetComponent<PlayerBase>().InventorySlot2 = slot;
                continue;
            }
            if (slot.tag == "InventorySlotMid")
            {
                player.GetComponent<PlayerBase>().TwoHandedSlot = slot;
                break;
            }
        }

        var buttons = playerInventoryContainer.GetComponentsInChildren<Button>();
        foreach (var button in buttons)
        {
            Debug.Log("button: " + button);

            if (button.tag == "InventorySlot1")
            {
                Debug.Log("POP");
                button.onClick.AddListener(() => player.slot1Pressed());
                continue;
            }
            if (button.tag == "InventorySlot2")
            {
                Debug.Log("POW");
                button.onClick.AddListener(() => player.slot2Pressed());
                continue;
            }
            if (button.tag == "InventorySlotMid")
            {
                Debug.Log("ZOOM");
                button.onClick.AddListener(() => player.TwoHandedSlotPressed());
                continue;
            }
        }

        player.GetComponent<PlayerBase>().InventorySlot1.gameObject.SetActive(false);
        player.GetComponent<PlayerBase>().InventorySlot2.gameObject.SetActive(false);
        player.GetComponent<PlayerBase>().TwoHandedSlot.gameObject.SetActive(false);
    }

    //Assigns the on click function when pressing the playerHud (has to be done after all previous variables are set)
    private void setInventoryAnimator(PlayerBase player, GameObject playerHud)
    {
        var button = playerHud.GetComponentInChildren<Button>();
        button.onClick.AddListener(() => player.openInventory(MainManager.Instance.Players));
    }

}
