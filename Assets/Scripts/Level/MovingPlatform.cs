using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    private int currentWaypoint = 0;
    [SerializeField] private float speed = 2f;
    private bool playerOnPlatform = false;
    private float timer = 6f;
    private bool started = false;

    // Update is called once per frame
    void Update()
    {
        if(!playerOnPlatform && Vector2.Distance(transform.position, waypoints[0].position) < 0.1f)
            started = false;

        if(started)
        {
            timer = 6f; //Reset the timer
            //If we've reached the current waypoint
            if (Vector2.Distance(waypoints[currentWaypoint].position, transform.position) < 0.1f)
            {
                currentWaypoint++;  //Switch to the next waypoint
                if (currentWaypoint >= waypoints.Length)    //If we've run out of waypoints, reset to first
                    currentWaypoint = 0;
            }
            transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypoint].transform.position, Time.deltaTime * speed);
        }
    }

    //Attach player to platform
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerBehaviour>())
        {
            started = true;
            playerOnPlatform = true;
            collision.gameObject.transform.SetParent(transform);
        }
    }

    //Detach player from platform
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerBehaviour>())
        {
            playerOnPlatform = false;
            collision.gameObject.transform.SetParent(null);
            StartCoroutine(PlatformTimer());
        }
    }

    //Coroutine checks how long player has been off platform
    private IEnumerator PlatformTimer()
    {
        float delay = 3f;
        //If the player has been off the platform for 3 seconds
        while (delay > 0 & !playerOnPlatform) 
        {
            delay -= Time.deltaTime;
            yield return null;
        }
        if(!playerOnPlatform)
            currentWaypoint = 0;
    }
}
