using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenes : MonoBehaviour
{

    //https://stackoverflow.com/questions/71680080/can-i-create-array-of-scenes-unity

    private string itemScreenExperiment = "Items experiment";
    private string preGameExperiment = "Pre game Experiment";
    private string skeletonExperiment = "Skeletons experiment";

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

    public void loadExperimentChapter()
    {
        Loader.Load(skeletonExperiment);
    }

    public void loadItemsExperimentChapter()
    {
        Loader.Load(itemScreenExperiment);
    }

    public void loadPreGameExperimentChapter()
    {
        Loader.Load(preGameExperiment);
    }
}
