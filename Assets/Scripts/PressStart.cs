using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class PressStart : MonoBehaviour
{
    private AsyncOperation async;
    // Start is called before the first frame update
    void Start()
    {
        
        async = SceneManager.LoadSceneAsync((SceneManager.GetActiveScene().buildIndex+ 1 == SceneManager.sceneCount?1: SceneManager.GetActiveScene().buildIndex + 1));
        async.allowSceneActivation = false;
    }


    public void onStartPressed(InputAction.CallbackContext context)
    {
        if (context.performed) async.allowSceneActivation = true;
    }
}
