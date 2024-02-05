using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventClearer : MonoBehaviour
{
    public List<GameEvent> Events = new();
    void Start()
    {
        print("Clear event listeners");
        foreach (var Event in Events)
        {
            Event.ClearListeners();
        }
    }

    //Add a chapterEvent clearer to prevent masses of duplicate listeners being created
}
