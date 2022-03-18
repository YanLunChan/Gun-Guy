using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : LivingEntity
{
    private Vector2 _direction;
    //private Queue<GameObject> _bulletQueue = new Queue<GameObject>(); //The snes had a sprite limit and there for shooters usually had a bullet limit
    private bool _diagonal;
    private bool _sprint;
    private bool _dead = false;
    private int _maxHealth = 5;
    private bool _grounded;
    private CapsuleCollider2D _hitbox;
    private BoxCollider2D _groundCheck;
    private List<SpriteRenderer> _renderers = new List<SpriteRenderer>();
    private bool _pause = false;
    [SerializeField] private float _jumpStrength = 10.0f;
    [SerializeField] private float _sprintSpeed = 7.0f;
    public int _extraShots;
    private GameObject bulletToFire;
    [SerializeField] GameObject regularBullet;
    [SerializeField] GameObject chargedBullet;
    [SerializeField] private GameObject _largeExplosionEffect;
    [SerializeField] private GameObject _smallExplosionEffect;
    [SerializeField] private bool chargeShotAvailable = false;

    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        health = _maxHealth;
        _hitbox = GetComponent<CapsuleCollider2D>();
        _groundCheck = GetComponent<BoxCollider2D>();
        foreach ( SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>())
        {
            _renderers.Add(renderer);
        }
    }

    private void FixedUpdate()
    {
        _body.velocity = new Vector2(_direction.x * (_sprint?_sprintSpeed:_speed), _body.velocity.y);
        if (!_grounded)
            _body.velocity += _gravity * Vector2.down;
        if (_body.velocity.x != 0)
            _anim.SetBool("Walking", true);
        else
            _anim.SetBool("Walking", false);
    }

    private void Update()
    {
        if (_pause) return;
        _anim.SetFloat("Height", _direction.y * (_diagonal ? 0.5f : 1.0f));
        _anim.SetFloat("Speed", Mathf.Abs(_body.velocity.x));
        _anim.SetFloat("VerticalSpeed", _body.velocity.y);
        if (_grounded && Mathf.Round(_body.velocity.x) == 0 && _direction.y < 0)
        {
            if (!_anim.GetBool("Ducking"))
            {
                //Trying something but you can move it by -0.5f * (new Vector3(0, 0.9f, 0)) instead.
                _hitbox.size = new Vector2(0.625f, 1f);
                this.transform.position = this.transform.position + -0.5f * (new Vector3(0, 0.9f, 0));
                _anim.SetBool("Ducking", true);
                _groundCheck.offset = new Vector2(0f, -0.875f);
            }
        }
        else
        {
            if (_anim.GetBool("Ducking"))
            {
                _hitbox.size = new Vector2(0.625f, 1.9f);
                this.transform.position = this.transform.position + 0.5f * (new Vector3(0, 0.9f, 0));
                _anim.SetBool("Ducking", false);
                _groundCheck.offset = new Vector2(0f, -1.3125f);
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (_pause) return;
        _direction = context.ReadValue<Vector2>();
        if (_direction.x != 0)
            foreach(SpriteRenderer renderer in _renderers)
            {
                renderer.flipX = (_direction.x < 0);
            }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (_pause) return;
        if (context.performed && _grounded && !_anim.GetBool("Ducking"))
        {
            _body.velocity = new Vector2(_body.velocity.x, _jumpStrength);
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (_pause) return;
        _sprint = context.performed && _grounded;
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (_pause) return;
        //Regular shot
        if ((!chargeShotAvailable && context.performed) || ( context.performed && context.duration < 1f))
        {
            bulletToFire = regularBullet;
            Shoot();
        }
        //Charged shot
        else if (context.performed && context.duration >= 1f)
        {
            bulletToFire = chargedBullet;
            Shoot();
        }
    }
    private void Shoot()
    {
        if (_dead) return;
        _anim.SetTrigger("Shoot");

        //add bullets here
        GameObject newProj = Instantiate(bulletToFire);
        newProj.GetComponent<BasicProjectile>().Owner = this.gameObject;

        //This entirely relies on the way the character appears to be aiming in the animator.
        Vector2 vel = new Vector2(0, 0);
        float height = _anim.GetFloat("Height");
        if (Mathf.Abs(height) == 1)
        {
            vel = new Vector2(0, height);
        }
        else
        {
            vel = new Vector2((_renderers[0].flipX ? -1 : 1), height * 2);
        }
        newProj.GetComponent<BasicProjectile>().Velocity = vel.normalized * 20;
        newProj.transform.position = this.transform.position + (Vector3)(vel.normalized * 0.5f);
        //_bulletQueue.Enqueue(newProj);
        BasicProjectile angle = newProj.GetComponent<BasicProjectile>();


        for (int i = 0; i < _extraShots; i++)
        {
            float rotateAngle = Mathf.Pow(-1, i) * 45f * Mathf.PI / 180f;
            Vector2 newVel = new Vector2(angle.Velocity.x * Mathf.Cos(rotateAngle) - angle.Velocity.y * Mathf.Sin(rotateAngle),
                angle.Velocity.x * Mathf.Sin(rotateAngle) + angle.Velocity.y * Mathf.Cos(rotateAngle));
            GameObject placeholder = Instantiate(newProj);
            placeholder.GetComponent<BasicProjectile>().Velocity = newVel;
        }
        //if (_bulletQueue.Count > 8) //Max of 8 player bullets.
        //{
        //    Destroy(_bulletQueue.Dequeue());
        //}
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        _pause = !_pause;
        Time.timeScale = (_pause ? 0.0f : 1.0f);
    }

    public void OnDiagonal(InputAction.CallbackContext context)
    {
        if (_pause) return;
        _diagonal = context.performed;
    }

    public override void TakeDamage(int damage)
    {
        if (--health <= 0)
            Die();
        else
        {
            GameObject effect = Instantiate(_smallExplosionEffect);
            effect.transform.position = this.transform.position;
        }

    }
    public override void Die()
    {
        if (transform.parent != null)
            gameObject.transform.parent = null;
        GameObject effect = Instantiate(_largeExplosionEffect);
        effect.transform.position = this.transform.position;
        GameManager.instance.loadMainMenuDelayed();
        _dead = true;
        gameObject.SetActive(false);
        
    }

    public bool RestoreHealth()
    {
        if(health < _maxHealth)
        {
            health++;
            return true;
        }
        else return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Mathf.Log(_levelMask.value, 2f)) 
        {
            _grounded = true;
            _anim.SetBool("Jump", false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Mathf.Log(_levelMask.value, 2f)) 
        {
            _grounded = false;
            _anim.SetBool("Jump", true);
        }
    }

    public void ChargeUpgrade()
    {
        chargeShotAvailable = true;
    }

}

