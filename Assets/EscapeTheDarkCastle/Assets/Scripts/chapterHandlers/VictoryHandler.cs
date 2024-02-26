using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryHandler : MonoBehaviour
{
    [SerializeField] public EndMe endGame;

    void Start()
    {
        endGame.destroySavedObjects();
    }

}
