using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelBossCol : LivingEntity
{
    public bool attacking = false;

    // Start is called before the first frame update
    void Start()
    {
        health = 30;
    }

    public override void Die()
    {
        gameObject.SetActive(false);
    }

    public override void TakeDamage(int damage)
    {
        if (--health <= 0) 
            Die();
    }

}
