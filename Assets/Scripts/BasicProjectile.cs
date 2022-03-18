using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    [HideInInspector] public GameObject Owner = null;
    [Tooltip("The damage this is going to deal")] public int Damage = 1;
    [Tooltip("The projectiles velocity")] public Vector2 Velocity = new Vector2(1, 0);
    [Tooltip("How much of the velocity will be carried over onto the rigidbody being hit")] public float Knockback = 0.0f;

    private Renderer _render;

    private void Update()
    {
        this.transform.position = this.transform.position + (Vector3)(Velocity * Time.deltaTime);
        this.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, Velocity));
    }

    private void Start()
    {
        _render = GetComponent<Renderer>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamagable hit = collision.gameObject.GetComponent<IDamagable>();

        //If we hit part of the level
        if(collision.gameObject.layer == LayerMask.NameToLayer("Level") && collision.gameObject != Owner)
            Destroy(this.gameObject);

        //If we hit something damagable
        else if (collision.gameObject != Owner)
        {
            hit?.TakeDamage(this.Damage);
            Destroy(this.gameObject);
        }
    }
}
