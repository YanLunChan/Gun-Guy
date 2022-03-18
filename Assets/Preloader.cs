using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Preloader : MonoBehaviour
{
    private AsyncOperation async;
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        async = SceneManager.LoadSceneAsync(currentScene.buildIndex + 1);   //Load the next scene
        async.allowSceneActivation = false;                                 //Wait to switch to next scene
    }

    private void Update()
    {
        if (async != null && async.progress >= 0.9f)    //If the scene to load is specified and 90% loaded
            async.allowSceneActivation = true;          //Switch scene
    }
}
