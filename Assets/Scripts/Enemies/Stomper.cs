using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomper : LivingEntity
{
    [SerializeField] [Tooltip("Distance Stomper walks before turning back.")] private float _distanceOfTravel = 5;
    private Vector2 _startPos;
    private SpriteRenderer _sprite;
    [SerializeField] private GameObject _explosionEffect;

    private float _hurtCooldown = 0;
    private void recalculate()
    {
        _startPos = this.transform.position;
    }
    void Start()
    {
        recalculate();
        _sprite = this.GetComponent<SpriteRenderer>();
        this.health = 3;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position = this.transform.position + new Vector3((_sprite.flipX ? 0.0625f : -0.0625f), 0, 0);
        if (Vector2.Distance(this.transform.position, _startPos) > _distanceOfTravel)
        {
            this.transform.position = _startPos + new Vector2(_distanceOfTravel*(_sprite.flipX?1:-1),0);
            _sprite.flipX = !_sprite.flipX;
            recalculate();
        }
        if (_hurtCooldown > 0)
        {
            _hurtCooldown -= Time.fixedDeltaTime;
            if (_hurtCooldown < 0) _hurtCooldown = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == GameManager.instance.Player && _hurtCooldown <= 0)
        {
            _hurtCooldown = 1.0f;
            collision.gameObject.GetComponent<PlayerBehaviour>()?.TakeDamage(1);
            Debug.Log("GET STOMPED >:D");
        }
    }

    public override void Die()
    {
        GameObject effect = Instantiate(_explosionEffect);
        effect.transform.position = this.transform.position;
        Destroy(this.gameObject);
    }

    public override void TakeDamage(int damage)
    {
        this.health-= damage;
        Debug.Log("No hurt stomper D:");
        if (this.health <= 0)
        {
            this.Die();
        }
    }
}
