using UnityEngine;
using UnityEngine.Events;
public class Destructible : MonoBehaviour, IDamagable
{
    [Header("Destructible Options")]
    [SerializeField][Tooltip("How many hits before breaking")] private int _hits = 1;
    [SerializeField][Tooltip("What to drop once broken")] private GameObject _drop = null;
    [SerializeField][Tooltip("What to do once broken")] private UnityEvent _onDrop = null;

    private Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    //Called once hit
    public void TakeDamage(int damage)
    {
        _hits--;
        if (_hits <= 0)
        {
            _anim?.SetTrigger("break"); //Should call Despawn on final frame of anim.
        }
        else
        {
            _anim?.SetTrigger("hit");
        }
    }

    //Called from animator
    public void Despawn()
    {
        if (_drop)
        {
            GameObject drop = Instantiate(_drop);
            drop.transform.position = this.transform.position;
        }
        _onDrop?.Invoke();
        Destroy(this);
    }

}
