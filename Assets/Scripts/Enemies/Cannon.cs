using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private GameObject _bullet;
    private Queue<GameObject> _bulletQueue = new Queue<GameObject>();
    public void OnShoot()
    {
        GameObject newProj = Instantiate(_bullet);
        newProj.GetComponent<BasicProjectile>().Velocity = newProj.GetComponent<BasicProjectile>().Velocity * (this.GetComponent<SpriteRenderer>().flipX ? -1 : 1);
        newProj.GetComponent<BasicProjectile>().Owner = this.gameObject;
        if (this.GetComponent<SpriteRenderer>().flipX)
        {
            newProj.GetComponent<SpriteRenderer>().flipY = !newProj.GetComponent<SpriteRenderer>().flipY;
        }

        newProj.transform.position = this.transform.position + new Vector3((this.GetComponent<SpriteRenderer>().flipX ? 1f : -1f),0,0);
        _bulletQueue.Enqueue(newProj);
        if (_bulletQueue.Count > 8) //Max of 8 player bullets.
        {
            Destroy(_bulletQueue.Dequeue());
        }
    }
}
