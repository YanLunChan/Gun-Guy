using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Gate : MonoBehaviour
{

    [SerializeField] [Tooltip("Which side can the player access reach from first?")] private bool _startOnSide = false;
    [SerializeField] [Tooltip("Only works once?")] private bool _oneWay = false;
    [SerializeField] [Tooltip("Spawner on side 0")] private Transform _spawn0;
    [SerializeField] [Tooltip("Spawner on side 1")] private Transform _spawn1;
    [SerializeField] [Tooltip("Rail on side 0")] private GameObject _rail0;
    [SerializeField] [Tooltip("Rail on side 1")] private GameObject _rail1;
    [SerializeField] [Tooltip("Blinds on camera")] private SpriteRenderer _shade;
    [SerializeField] [Tooltip("Level end gate")] private bool _loadNextLevel;
    [SerializeField] private DeathLaser _loveLaser;
    [SerializeField] private float _headstart;

    [SerializeField] [Tooltip("Background color after passing through gate (MUST HAVE SHADE)")] private Color _desiredBGColor = new Color(0, 0, 0, 0);
    
    public bool Usable = true;

    private AsyncOperation async;

    private void Start()
    {
        if (_loadNextLevel)
        {
            async = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
            async.allowSceneActivation = false;
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!Usable) return;
        if (collision.gameObject == GameManager.instance.Player && GameManager.instance.Player != null)
        {
            if (_shade != null)
            {
                StartCoroutine(FadeInAndOut());
                _loveLaser?.DelayStart(_headstart);
                    
            }
            else
            {
                if (async != null)
                {
                    async.allowSceneActivation = true;
                }
                else
                {
                    GameManager.instance.Camera?.GetComponent<CameraDolly>().SetRail(_startOnSide ? _rail0 : _rail1);
                    GameManager.instance.Player.transform.position = (_startOnSide ? _spawn0.position : _spawn1.position);
                    if (_oneWay) Usable = false;
                    _startOnSide = !_startOnSide;
                }
            }

        }
    }

    IEnumerator FadeInAndOut()
    {
        Time.timeScale = 0;
        for (float i = 0f; i <= 1; i += 0.25f)
        {
            _shade.color = new Color(0, 0, 0, i);
            yield return new WaitForSecondsRealtime(0.25f);
        }
        Time.timeScale = 1;
        if (async!= null)
        {
            async.allowSceneActivation = true;
        }
        else
        {
            GameManager.instance.Camera?.GetComponent<CameraDolly>().SetRail(_startOnSide ? _rail0 : _rail1);
            GameManager.instance.Player.transform.position = (_startOnSide ? _spawn0.position : _spawn1.position);
            if (_oneWay) Usable = false;
            if (_desiredBGColor.a > 0)
                GameManager.instance.Camera.GetComponent<Camera>().backgroundColor = _desiredBGColor;
            for (float i = 1f; i >= 0; i -= 0.25f)
            {
                _shade.color = new Color(0, 0, 0, i);
                yield return new WaitForSecondsRealtime(0.25f);
            }
            _startOnSide = !_startOnSide;
        }
    }

}
