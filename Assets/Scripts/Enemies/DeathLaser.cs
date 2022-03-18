using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathLaser : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private bool activate = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(activate)
            transform.position += (Vector3) (Vector2.up * speed * Time.deltaTime);
    }

    public void DelayStart(float timer)
    {
        if (activate)
            ToggleBool();
        else
            Invoke("ToggleBool", timer);
    }
    private void ToggleBool() 
    {
        if (activate)
            activate = false;
        else
            activate = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        collision.gameObject.GetComponent<LivingEntity>()?.Die();
    }

}
