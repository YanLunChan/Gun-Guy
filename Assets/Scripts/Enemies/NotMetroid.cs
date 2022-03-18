using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotMetroid : LivingEntity
{

    private float _metroidLoop = 0.0f;
    private bool _isActive = false;
    private SpriteRenderer _renderer;
    private float _hurtCooldown = 0;

    [SerializeField] private GameObject _explosionEffect;
    public override void TakeDamage(int damage)
    {
        this.health -= damage;
        if (this.health < 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        GameObject effect = Instantiate(_explosionEffect);
        effect.transform.position = this.transform.position;
        Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        _anim = this.GetComponent<Animator>();
        _body = this.GetComponent<Rigidbody2D>();
        _renderer = this.GetComponent<SpriteRenderer>();
        this.health = 2;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_isActive)
        {
            if (_renderer.isVisible)
                _isActive = true;
            else
                return;
        }
        _metroidLoop += Time.fixedDeltaTime;
        if (_metroidLoop > 8)
        {
            _metroidLoop -= 8;
        }
        Vector2 _targetDir = (GameManager.instance.Player.transform.position - this.transform.position).normalized;
        _body.velocity = new Vector2((Mathf.Sin(_metroidLoop * Mathf.PI) / 4)*6*_speed, 0) + (_targetDir*_speed);
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
            _hurtCooldown = 1f;
            _anim.SetTrigger("attack");
            collision.gameObject.GetComponent<PlayerBehaviour>()?.TakeDamage(1);
        }
    }
}
