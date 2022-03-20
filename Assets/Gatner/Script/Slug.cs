using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slug : MonoBehaviour
{
    [SerializeField] private Bullet _bullet;
    [SerializeField] private Bullet1 _bullet1;
    [SerializeField] private float _damageLevel = 9;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            if(_damageLevel > 0)
            {
                _damageLevel -= _bullet.GetBulletDamage();
            }

            if (_damageLevel <= 0)
            {
                Destroy(gameObject);
            }
        }

        if (other.CompareTag("Bullet1"))
        {
            if (_damageLevel > 0)
            {
                _damageLevel -= _bullet1.GetBulletDamage();
            }

            if (_damageLevel <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
