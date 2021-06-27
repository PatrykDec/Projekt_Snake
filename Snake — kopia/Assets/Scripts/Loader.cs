using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    
    public enum Scene
    {
        GameScene,
        MainMenu
    }

    private static Action loaderCallbackAction;

    public static void Load(Scene scene)
    {
        loaderCallbackAction = () => {
            SceneManager.LoadScene("GameScene1");
        };
    }
    public static void LoaderCallback()
    {
        if (loaderCallbackAction != null)
        {
            loaderCallbackAction();
            loaderCallbackAction = null;
        }
    }

}
