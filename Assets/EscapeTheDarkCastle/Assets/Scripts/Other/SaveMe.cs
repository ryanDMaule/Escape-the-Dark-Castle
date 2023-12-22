using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMe : MonoBehaviour
{
    //public static SaveMe Instance;
    void Start()
    {
        Debug.Log("SaveMe!");
        DontDestroyOnLoad(gameObject);

        Debug.Log("gameObject:" + gameObject.name);

    }
    
    /*
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    */

}
