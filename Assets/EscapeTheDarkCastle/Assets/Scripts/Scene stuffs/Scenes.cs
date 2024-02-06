using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenes : MonoBehaviour
{

    //https://stackoverflow.com/questions/71680080/can-i-create-array-of-scenes-unity

    //SCENES
    private string mainMenu = "Main menu";
    private string charctersSelect = "Character select";
    private string preGame = "Pre game";
    private string items = "Items scene";

    //CHAPTERS
    [Header("Chapter")]
    private string skeletonsTest = "Skeletons abstract chapter";
    private string boneBeast = "Bone beast NEW";

    //START ROOMS
    private string hiddenPassage = "Hidden passage";

    //LISTS
    private List<string> chapterList = new List<string>
    {
        "Skeletons abstract chapter",
        "Bone beast NEW"
    };

    public List<string> startRoomList = new();
    public List<string> bossList = new();


    public void loadHiddenPassage()
    {
        Loader.Load(hiddenPassage);
    }

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
