using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigStomp : LivingEntity
{
    [SerializeField] private GameObject _explosionEffect;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Gate gate;

    private float _hurtCooldown = 0;
    private SpriteRenderer _sprite;
    private bool _isActive = false;
    private bool _shouldShoot = false;
    private float _jumpCooldown = 0.0f;
    void Start()
    {
        _sprite = this.GetComponent<SpriteRenderer>();
        _body = this.GetComponent<Rigidbody2D>();
        _anim = this.GetComponent<Animator>();
        this.health = 9;
    }

    private void randomJump()
    {
        float angle = Mathf.Deg2Rad*Random.Range(45.0f, 135.0f);
        float x = Mathf.Cos(angle);
        float y = Mathf.Sin(angle);
        _body.velocity = new Vector2(x, y)  * 10;
        _shouldShoot = true;
        _anim.SetBool("jump",true);
    }

    private void Update()
    {
        _anim.SetFloat("velocity", _body.velocity.y);
        this.transform.localScale = new Vector3((GameManager.instance.Player.transform.position.x > this.transform.position.x ? -1 : 1), 1, 1);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_isActive)
        {
            if (_sprite.isVisible)
                _isActive = true;
            else
                return;
        }
        if (_hurtCooldown > 0)
        {
            _hurtCooldown -= Time.fixedDeltaTime;
            if (_hurtCooldown < 0) _hurtCooldown = 0;
        }
        if (_body.velocity.y == 0) _anim.SetBool("jump", false);
        if (_jumpCooldown > 0)
        {
            _jumpCooldown -= Time.fixedDeltaTime;
        }
        else
        {
            if (_shouldShoot)
            {
                _anim.SetTrigger("shoot");
                _shouldShoot = false;
            }
            else
            {
                randomJump();
            }
            _jumpCooldown = Random.Range(2, 3);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == GameManager.instance.Player && _hurtCooldown <= 0)
        {
            _hurtCooldown = 2.0f;
            collision.gameObject.GetComponent<PlayerBehaviour>()?.TakeDamage(2);
        }
    }

    public void Shoot()
    {
        foreach (Transform point in this.GetComponentsInChildren<Transform>())
        {
            if (point != this.transform)
            {
                GameObject temp = Instantiate(_bullet);
                temp.transform.position = point.position;
                temp.GetComponent<BasicProjectile>().Owner = this.gameObject;
                temp.GetComponent<BasicProjectile>().Velocity = temp.GetComponent<BasicProjectile>().Velocity * this.transform.localScale;
                temp.GetComponent<SpriteRenderer>().flipY = (this.transform.localScale.x > 0);
            }
        }
    }
    public override void Die()
    {
        GameObject effect = Instantiate(_explosionEffect);
        effect.transform.position = this.transform.position;
        gate.Usable = true;
        Destroy(this.gameObject);
    }

    public override void TakeDamage(int damage)
    {
        if (damage < 3) return;
        this.health -= damage;
        if (this.health < 0)
        {
            Die();
        }
    }
}
