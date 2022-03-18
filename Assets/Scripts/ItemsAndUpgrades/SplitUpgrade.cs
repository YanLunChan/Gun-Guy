using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitUpgrade : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerBehaviour player;
        if (player = collision.GetComponent<PlayerBehaviour>())
        {
            player._extraShots = 2;
            Destroy(this.gameObject);
        }
    }
}
