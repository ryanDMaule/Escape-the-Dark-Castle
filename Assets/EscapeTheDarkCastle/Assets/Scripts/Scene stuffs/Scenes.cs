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
    private string barrels = "Barrels";
    private string putridCaptain = "Putrid captain";
    private string fireDemon = "Fire demon";
    private string blacksmith = "Blacksmith";
    private string strongman = "Strongman";
    private string arenaChampion = "Arena champion";
    private string skeletalBeast = "Skeletal beast";
    private string castleMaze = "Castle maze";
    private string sewers = "Sewers";
    private string hideousCreature = "Hideous creature";
    private string bloodthirster = "Bloodthirster";
    private string theArrow = "The arrow";
    private string theCurse = "The curse";
    private string theCaveIn = "The cave in";

    //BOSSES
    private string thePlayerOfGames = "The player of games";

    //START ROOMS
    private string hiddenPassage = "Hidden passage";

    //VICTORY SCREEN
    private string victory = "Victory screen";

    //LISTS
    private List<string> chapterList = new List<string>()
    {
        "Skeletons abstract chapter",
        "Bone beast NEW",
        "Barrels",
        "Putrid captain",
        "Fire demon",
        "Blacksmith",
        "Strongman",
        "Arena champion",
        "Skeletal beast",
        "Castle maze",
        "Sewers",
        "Hideous creature",
        "Bloodthirster",
        "The arrow",
        "The curse",
        "The cave in"
    };

    private List<string> startRoomList = new List<string>()
    {
        "Hidden passage"
    };

    private List<string> bossList = new List<string>()
    {
        "The player of games"
    };

    public string getStartRoom()
    {
        if(startRoomList.Count > 0)
        {
            int pos = Random.Range(0, startRoomList.Count);
            string chapter = startRoomList[pos];

            startRoomList.RemoveAt(pos);

            return chapter;
        } else
        {
            //HANDLE PROPERLY
            return "NO MORE START ROOMS";
        }
    }

    public string getChapter()
    {
        if (chapterList.Count > 0)
        {
            int pos = Random.Range(0, chapterList.Count);
            string chapter = chapterList[pos];

            chapterList.RemoveAt(pos);

            return chapter;
        } else
        {
            //HANDLE PROPERLY
            return "NO MORE CHAPTERS";
        }
    }

    public string getBoss()
    {
        if (bossList.Count > 0)
        {
            int pos = Random.Range(0, bossList.Count);
            string chapter = bossList[pos];

            bossList.RemoveAt(pos);

            return chapter;
        } else
        {
            //HANDLE PROPERLY
            return "NO MORE BOSSES";
        }
    }

    public string getVictory()
    {
        return "Victory screen";
    }

    public void loadTest()
    {
        Loader.Load(thePlayerOfGames);
    }

    public void loadVictory()
    {
        Loader.Load(victory);
    }

    public void loadHiddenPassage()
    {
        Loader.Load(hiddenPassage);
    }

    public void loadTesctChapter()
    {
        Loader.Load(thePlayerOfGames);
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
