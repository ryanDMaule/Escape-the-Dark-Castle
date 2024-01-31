using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenes : MonoBehaviour
{

    //https://stackoverflow.com/questions/71680080/can-i-create-array-of-scenes-unity

    [Header("Scenes")]
    private string charctersSelect = "Character select";
    private string preGame = "Pre game";
    private string skeletons = "Skeletons HUD CHANGES";
    private string items = "Items scene";
    private string mainMenu = "Main menu";

    public List<string> chapterList = new List<string>
    {
        "Skeletons",
        "Bone beast"
    };

    public List<string> startRoomList = new();
    public List<string> bossList = new();

    public void loadRandomChapter()
    {
        Loader.Load(chapterList[Random.Range(0, chapterList.Count)]);
    }

    public void loadCharacterSelect()
    {
        Loader.Load(charctersSelect);
    }

    public void loadPreGameChapter()
    {
        Loader.Load(preGame);
    }

    public void loadSkeletonsChapter()
    {
        Loader.Load(skeletons);
    }

    public void loadItemsChapter()
    {
        Loader.Load(items);
    }

    public void loadMainMenu()
    {
        Loader.Load(mainMenu);
    }

}
