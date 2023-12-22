using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenes : MonoBehaviour
{

    //https://stackoverflow.com/questions/71680080/can-i-create-array-of-scenes-unity

    public string characterSelect = "Character select";
    public string itemScreen = "Items";
    public string preGame = "Pre game";
    public string preGameExperiment = "Pre game Experiment";

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

}
