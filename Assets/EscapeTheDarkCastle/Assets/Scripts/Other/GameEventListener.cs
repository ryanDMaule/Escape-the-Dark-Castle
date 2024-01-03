using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//TUTORIAL : https://www.youtube.com/watch?v=7_dyDmF0Ktw

public class GameEventListener : MonoBehaviour
{
    public GameEvent gameEvent;

    public UnityEvent response;

    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        response.Invoke();
    }

}
