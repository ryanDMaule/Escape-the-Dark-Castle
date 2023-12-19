using System.Buffers;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class FormatHud : MonoBehaviour
{
    public List<PlayerBase> Players = new();

    [SerializeField] public GameObject placeholder1;
    [SerializeField] public GameObject placeholder2;
    [SerializeField] public GameObject placeholderMid;
    [SerializeField] public GameObject placeholder3;
    [SerializeField] public GameObject placeholder4;

    [SerializeField] public GameObject AbbotHud;
    [SerializeField] public GameObject MillerHud;
    [SerializeField] public GameObject SmithHud;
    [SerializeField] public GameObject CookHud;
    [SerializeField] public GameObject TannerHud;
    [SerializeField] public GameObject TailorrHud;

    [SerializeField] public GameObject player1;
    [SerializeField] public GameObject player2;
    [SerializeField] public GameObject player3;
    [SerializeField] public GameObject player4;

    [SerializeField] public GameObject inventory;
    //[SerializeField] public GameObject inventoryPlaceholder;

    [SerializeField] public GameObject cardContainer;
    [SerializeField] public DeckLogic dl;

    public void Start()
    {
        formatHud();
        clearUnusedObjects();
    }

    public void formatHud()
    {
        int playersCount = MainManager.Instance.Players.Count;

        switch (playersCount)
        {
            case 2:
                placeholder1.SetActive(true);
                setHudDetails(placeholder1, 0);

                placeholder4.SetActive(true);
                setHudDetails(placeholder4, 1);
                break;

            case 3:
                placeholder1.SetActive(true);
                setHudDetails(placeholder1, 0);

                placeholderMid.SetActive(true);
                setHudDetails(placeholderMid, 1);

                placeholder4.SetActive(true);
                setHudDetails(placeholder4, 2);

                break;

            case 4:
                placeholder1.SetActive(true);
                setHudDetails(placeholder1, 0);

                placeholder2.SetActive(true);
                setHudDetails(placeholder2, 1);

                placeholder3.SetActive(true);
                setHudDetails(placeholder3, 2);

                placeholder4.SetActive(true);
                setHudDetails(placeholder4, 3);

                break;

            default:
                break;
        }
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

    private void setHudDetailsOld(GameObject player, int arrayPos)
    {
        var textFields = player.GetComponentsInChildren<Text>();

        foreach (var text in textFields)
        {
            if (text.tag == "HUD-name")
            {
                text.text = MainManager.Instance.getPlayerName(arrayPos);
            }

            if (text.tag == "HUD-health")
            {
                text.text = MainManager.Instance.getPlayerHealth(arrayPos).ToString();
            }
            Debug.Log("text field : " + text.text);
        }
    }

    private GameObject getPlayerObject(int position)
    {
        switch (position)
        {
            case 0:
                return player1;

            case 1:
                return player2;

            case 2:
                return player3;

            case 3:
                return player4;

            default:
                Debug.Log("Error!");
                return player1;
        }
    }

    public void assignPlayer(int arrayPos, string playerName, GameObject hud)
    {
        GameObject playerObject = getPlayerObject(arrayPos);
        //playerObject.name = playerName;

        switch (playerName)
        {
            case "Abbot":
                PlayerBase abbot = playerObject.AddComponent<Abbot>();
                abbot.name = "Abbot";

                formatInventory(arrayPos, abbot, hud);
                setInventoryAnimator(hud, abbot);
                applyPlayerItems(arrayPos, abbot);

                Players.Add(abbot);
                break;

            case "Miller":
                PlayerBase miller = playerObject.AddComponent<Miller>();
                miller.name = "Miller";

                formatInventory(arrayPos, miller, hud);
                setInventoryAnimator(hud, miller);
                applyPlayerItems(arrayPos, miller);

                Players.Add(miller);
                break;

            case "Smith":
                PlayerBase smith = playerObject.AddComponent<Smith>();
                smith.name = "Smith";

                formatInventory(arrayPos, smith, hud);
                setInventoryAnimator(hud, smith);
                applyPlayerItems(arrayPos, smith);

                Players.Add(smith);
                break;

            case "Cook":
                PlayerBase cook = playerObject.AddComponent<Cook>();
                cook.name = "Cook";

                formatInventory(arrayPos, cook, hud);
                setInventoryAnimator(hud, cook);
                applyPlayerItems(arrayPos, cook);

                Players.Add(cook);
                break;

            case "Tanner":
                PlayerBase tanner = playerObject.AddComponent<Tanner>();
                tanner.name = "Tammer";

                formatInventory(arrayPos, tanner, hud);
                setInventoryAnimator(hud, tanner);
                applyPlayerItems(arrayPos, tanner);

                Players.Add(tanner);
                break;

            case "Tailor":
                PlayerBase tailor = playerObject.AddComponent<Tailor>();
                tailor.name = "Tailor";

                formatInventory(arrayPos, tailor, hud);
                setInventoryAnimator(hud, tailor);
                applyPlayerItems(arrayPos, tailor);

                Players.Add(tailor);
                break;

            default:
                Debug.Log("Error!");
                break;
        }

}

    public void setHudDetails(GameObject placeholder, int arrayPos)
    {
        string name = MainManager.Instance.getPlayerName(arrayPos);
        switch (name)
        {
            case "Abbot":
                Instantiate(AbbotHud, placeholder.transform);
                break;

            case "Miller":
                Instantiate(MillerHud, placeholder.transform);
                break;

            case "Smith":
                Instantiate(SmithHud, placeholder.transform);
                break;

            case "Cook":
                Instantiate(CookHud, placeholder.transform);
                break;

            case "Tanner":
                Instantiate(TannerHud, placeholder.transform);
                break;

            case "Tailor":
                Instantiate(TailorrHud, placeholder.transform);
                break;

            default:
                Debug.Log("Error!");
                break;
        }
        assignPlayer(arrayPos, name, placeholder);
    }

    private void formatInventory(int arrayPos, PlayerBase player, GameObject hud)
    {
        var playerObject = getPlayerObject(arrayPos);
        var test = playerObject.GetComponentsInChildren<RectTransform>();

        foreach (var item in test)
        {
            if (item.tag == "Inventory")
            {
                playerObject.GetComponent<PlayerBase>().panel = item.gameObject;

                var hudComponenets = hud.GetComponentsInChildren<Text>();
                foreach (var text in hudComponenets)
                {
                    if (text.tag == "HUD-health")
                    {
                        playerObject.GetComponent<PlayerBase>().healthText = text;
                        break;
                    }
                }
            }
        }

        var textFields = playerObject.GetComponentsInChildren<Text>();
        foreach (var text in textFields)
        {
            if (text.tag == "might")
            {
                text.text = player.getPlayerMight().ToString();
            }

            if (text.tag == "cunning")
            {
                text.text = player.getPlayerCunning().ToString();
            }

            if (text.tag == "wisdom")
            {
                text.text = player.getPlayerWisdom().ToString();
            }
        }

        var image = playerObject.GetComponentsInChildren<Image>();
        foreach (var slot in image)
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
        player.GetComponent<PlayerBase>().InventorySlot1.gameObject.SetActive(false);
        player.GetComponent<PlayerBase>().InventorySlot2.gameObject.SetActive(false);
        player.GetComponent<PlayerBase>().TwoHandedSlot.gameObject.SetActive(false);


    }

    private void setInventoryAnimator(GameObject playerHud, PlayerBase player)
    {
        var button = playerHud.GetComponentInChildren<Button>();
        button.onClick.AddListener(() => player.openInventory(Players));
    }

    /*
    public void instantiateInventory(GameObject playerObject, PlayerBase player)
    {
        GameObject inventoryObject = Instantiate(inventory, playerObject.transform);
        inventoryObject.transform.position = inventoryPlaceholder.transform.position;

        var textFields = inventoryObject.GetComponentsInChildren<Text>();
        foreach (var text in textFields)
        {
            if (text.tag == "might")
            {
                text.text = player.getPlayerMight().ToString();
            }

            if (text.tag == "cunning")
            {
                text.text = player.getPlayerCunning().ToString();
            }

            if (text.tag == "wisdom")
            {
                text.text = player.getPlayerWisdom().ToString();
            }
        }
    }
    */

    private void applyPlayerItems(int arrayPos, PlayerBase player)
    {
        switch (arrayPos)
        {
            case 0:
                player.SetCurrentHealth(MainManager.Instance.player1_health);
                //apply item cards
                instantiateCard(MainManager.Instance.player1_inventory0, player);
                instantiateCard(MainManager.Instance.player1_inventory1, player);
                break;

            case 1:
                player.SetCurrentHealth(MainManager.Instance.player2_health);
                //apply item cards
                instantiateCard(MainManager.Instance.player2_inventory0, player);
                instantiateCard(MainManager.Instance.player2_inventory1, player);
                break;

            case 2:
                player.SetCurrentHealth(MainManager.Instance.player3_health);
                //apply item cards
                instantiateCard(MainManager.Instance.player3_inventory0, player);
                instantiateCard(MainManager.Instance.player3_inventory1, player);
                break;

            case 3:
                player.SetCurrentHealth(MainManager.Instance.player4_health);
                //apply item cards
                instantiateCard(MainManager.Instance.player4_inventory0, player);
                instantiateCard(MainManager.Instance.player4_inventory1, player);
                break;

            default:
                Debug.Log("Error!");
                break;
        }
    }

    private void instantiateCard(string cardName, PlayerBase player)
    {
        Debug.Log("Item card:" + cardName);
        switch (cardName)
        {
            case "decayed blade_0":
                player.assignInventoryCard(Instantiate(dl.decayedBlade, cardContainer.transform));
                break;

            case "stale loaf of bread_0":
                player.assignInventoryCard(Instantiate(dl.staleLoafOfBread, cardContainer.transform));
                break;

            case "rotten shield_0":
                player.assignInventoryCard(Instantiate(dl.rottenShield, cardContainer.transform));
                break;

            case "Cracked axe":
                player.assignInventoryCard(Instantiate(dl.crackedAxe, cardContainer.transform));
                break;

            case "the replication stones_0":
                player.assignInventoryCard(Instantiate(dl.theReplicationStones, cardContainer.transform));
                break;

            default:
                Debug.Log("Card not found");
                break;
        }
    }

}