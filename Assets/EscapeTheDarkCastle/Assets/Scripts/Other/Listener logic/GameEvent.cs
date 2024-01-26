using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TUTORIAL : https://www.youtube.com/watch?v=7_dyDmF0Ktw

[CreateAssetMenu(menuName = "GameEvent")]
public class GameEvent : ScriptableObject
{
    public List<GameEventListener> listeners = new List<GameEventListener>();

    //Raise event through different method signatures
    public void Raise()
    {
        Debug.Log("Raise");

        for (int i = 0; i < listeners.Count; i++)
        {
            listeners[i].OnEventRaised();
        }
    }

    //Manage listeners
    public void RegisterListener(GameEventListener listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }

    public void UnregisterListener(GameEventListener listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }
    }

    public void ClearListeners()
    {
        listeners.Clear();
    }
}
