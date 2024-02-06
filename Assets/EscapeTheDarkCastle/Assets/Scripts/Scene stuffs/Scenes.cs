using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenes : MonoBehaviour
{

    //https://stackoverflow.com/questions/71680080/can-i-create-array-of-scenes-unity

    [Header("Scenes")]
    private string mainMenu = "Main menu";
    private string charctersSelect = "Character select";
    private string preGame = "Pre game";
    private string items = "Items scene";

    [Header("Chapter")]
    private string skeletonsTest = "Skeletons abstract chapter";
    private string boneBeast = "Bone beast NEW";

    [Header("Lists")]
    public List<string> chapterList = new List<string>
    {
      
    };

    public List<string> startRoomList = new();
    public List<string> bossList = new();

    public void loadTesctChapter()
    {
        //Loader.Load(skeletonsTest);
        Loader.Load(boneBeast);
    }

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

    public void loadItemsChapter()
    {
        Loader.Load(items);
    }

    public void loadMainMenu()
    {
        Loader.Load(mainMenu);
    }

}
