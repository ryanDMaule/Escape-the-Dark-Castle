using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

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


    public void Start()
    {
        formatHud();
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

    private void setHudDetailsOld(GameObject player, int arrayPos)
    {
        var textFields = player.GetComponentsInChildren<Text>();

        foreach (var text in textFields)
        {
            if(text.tag == "HUD-name")
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
    }
    
  


}
