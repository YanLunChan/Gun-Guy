using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : LivingEntity
{
    GameManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void TakeDamage(int damage)
    {
        throw new System.NotImplementedException();
    }

    public override void Die()
    {
        throw new System.NotImplementedException();
    }
}
