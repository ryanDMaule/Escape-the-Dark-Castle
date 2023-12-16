using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class FormatHud : MonoBehaviour
{

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

    public void assignPlayer(int arrayPos, string playerName)
    {
        GameObject playerObject = getPlayerObject(arrayPos);
        //playerObject.name = playerName;

        switch (playerName)
        {
            case "Abbot":
                playerObject.AddComponent<Abbot>();
                break;

            case "Miller":
                playerObject.AddComponent<Miller>();
                break;

            case "Smith":
                playerObject.AddComponent<Smith>();
                break;

            case "Cook":
                playerObject.AddComponent<Cook>();
                break;

            case "Tanner":
                playerObject.AddComponent<Tanner>();
                break;

            case "Tailor":
                playerObject.AddComponent<Tailor>();
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
        assignPlayer(arrayPos, name);
    }




}
