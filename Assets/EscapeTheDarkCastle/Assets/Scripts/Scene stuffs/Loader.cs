using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{

    //https://www.youtube.com/watch?v=3I5d2rUJ0pE&t=51s

    public static void Load(string scene)
    {
        SceneManager.LoadScene(scene);
    }

}
