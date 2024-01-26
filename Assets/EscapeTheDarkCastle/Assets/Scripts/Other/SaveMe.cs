using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMe : MonoBehaviour
{
    void Start()
    {
        Debug.Log("SaveMe!");
        DontDestroyOnLoad(gameObject);
    }

    /*
    public static SaveMe Instance;

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
