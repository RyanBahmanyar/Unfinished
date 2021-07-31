using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : HealthController
{
    [SerializeField]
    int HP;

    [SerializeField]
    int moneyDropAmount;

    [SerializeField]
    float moneyDropDistance;

    [SerializeField]
    float moneyDropTime;

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
        GameObject.FindWithTag("Cash Pool").GetComponent<CollectablePool>().SpawnCollectables(moneyDropAmount, this.transform.position, moneyDropDistance, moneyDropTime);
        // Play death animation, destroy with animation event
        this.gameObject.SetActive(false);
    }
}
