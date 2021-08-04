using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyHealth : HealthController
{
    [SerializeField]
    int HP;

    [SerializeField]
    float deathSpeed;

    [SerializeField]
    int moneyDropAmount;

    [SerializeField]
    float moneyDropDistance;

    [SerializeField]
    float moneyDropTime;

    [SerializeField]
    Animator anim;

    private Action<GameObject> deathCallback;

    public void SetDeathCallback(Action<GameObject> callBack)
    {
        deathCallback = callBack;
    }

    protected override bool isDead()
    {
        return HP <= 0;
    }

    protected override void onDamage(int damage)
    {
        HP -= damage;
        anim.SetTrigger("Fall");
    }

    protected override void onDeath() { }

    public void ManualCheckDead() 
    {
        if (isDead())
        {
            deathCallback(gameObject);
            GameObject.FindWithTag("Cash Pool").GetComponent<CollectablePool>().SpawnCollectables(moneyDropAmount, this.transform.position, moneyDropDistance, moneyDropTime);
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
}
