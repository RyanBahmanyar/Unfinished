using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : HealthController
{
    [SerializeField]
    int HP;
    
    protected override bool isDead()
    {
        return HP <= 0;
    }

    protected override void onDamage(int damage)
    {
        HP -= damage;
    }

    protected override void onDeath()
    {
        this.gameObject.SetActive(false);
    }
}
