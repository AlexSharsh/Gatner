using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet1 : MonoBehaviour
{
    [SerializeField] private float _damage = 9;
    //private Transform _target;
    private float _speed;
    bool isHit = false;
    private Vector3 _direction;

    public void Init(/*Transform target,*/ float lifeTime, float speed)
    {
        //_target = target;
        _speed = speed;
        Destroy(gameObject, lifeTime);

        _direction.x = Input.GetAxis("Horizontal");
        _direction.z = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        //transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed);
        transform.position += transform.forward * _speed * Time.fixedDeltaTime;
        var fixedDirection = transform.TransformDirection(_direction.normalized);
        transform.position += fixedDirection * _speed * Time.fixedDeltaTime;

        if (transform.position.y > 0)
        {
            _direction.y += 1;
        }
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

        if (other.CompareTag("SlugBoss"))
        {
            Destroy(gameObject);
        }
    }

    public float GetBulletDamage()
    {
        return _damage;
    }
}
