using UnityEngine;

public abstract class LivingEntity : MonoBehaviour, IDamagable 
{
    protected int health;
    [SerializeField] protected private float _speed = 5.0f;
    [SerializeField] protected private float _gravity = 0.0f;
    [SerializeField] protected private LayerMask _levelMask;

    protected private Animator _anim;
    protected private Rigidbody2D _body;
    
    public abstract void Die();
    public abstract void TakeDamage(int damage);
}
