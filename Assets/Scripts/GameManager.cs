using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public GameObject Player;
    public GameObject Camera;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        //Use the game manager to mimic SNES frame rate
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
    }

    public void loadMainMenuDelayed()
    {
        StartCoroutine(WaitBeforeMenu());
    }

    IEnumerator WaitBeforeMenu()
    {
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(1);
    }
}