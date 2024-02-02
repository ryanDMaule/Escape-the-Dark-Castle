using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMe : MonoBehaviour
{
    public void destroySavedObjects()
    {
        print("Clean up game...");

        var playerObjectRectObjects = GameObject.FindGameObjectsWithTag("Destroy");
        foreach (var item in playerObjectRectObjects)
        {
            Destroy(item);
        }

        Destroy(MainManager.Instance);
    }

}
