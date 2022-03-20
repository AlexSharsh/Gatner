using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _damage = 3;
    //private Transform _target;
    private float _speed;
    bool isHit = false;

    public void Init(/*Transform target,*/ float lifeTime, float speed)
    {
        //_target = target;
        _speed = speed;
        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        //transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed);
        transform.position += transform.forward * _speed * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out ITakeDamage takeDamage))
        {
            Debug.Log("Hit!");
            takeDamage.Hit(_damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Slug"))
        {
            Destroy(gameObject);
        }
    }

    public float GetBulletDamage()
    {
        return _damage;
    }
}
