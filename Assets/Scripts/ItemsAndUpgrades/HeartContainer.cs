using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartContainer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerBehaviour player;
        if (player = collision.GetComponent<PlayerBehaviour>())
            if (player.RestoreHealth())
                Destroy(this.gameObject);
    }
}
