using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerProjectile : AttackController
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float lifeTime;

    private float age;

    private void FixedUpdate()
    {
        age += Time.deltaTime;

        if (age > lifeTime)
        {
            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector3 _direction)
    {
        GetComponent<Rigidbody2D>().velocity = _direction.normalized * speed;
    }
}
