using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeUpgrade : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerBehaviour player;
        if (player = collision.GetComponent<PlayerBehaviour>())
        {
            player.ChargeUpgrade();
            Destroy(this.gameObject); 
        }
    }
}
