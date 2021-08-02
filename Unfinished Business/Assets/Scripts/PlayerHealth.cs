using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : HealthController
{
    // The HP of the player. If they are damaged at $0, they die.
    public int Money;

    // The amount of time the player is invincible after being hit.
    [SerializeField]
    float invicibilityTime;

    // Time since this character has taken a hit
    float hitTimer;

    // Whether or not the player took damage at $0
    public bool hitAtZero = false;

    // The global cash pool
    CollectablePool cashPool;

    // The distance to drop the money from the player
    [SerializeField]
    float moneyDropDistance;

    // The time it takes for the money to drop away from the player
    [SerializeField]
    float moneyDropTime;

    // Returns hitAtZero
    protected override bool isDead()
    {
        return hitAtZero;
    }

    // If enough time has passed since the player has taken damage, deals damage to the player.
    // If the player is hit at $0, sets hasMoney to false.
    // If the player has less money than the damage they would take, set their money to 0 and drop what they had.
    // If the player has more money than the damage they would take, deduct the damage from their money and drop money equal to the damage taken.
    protected override void onDamage(int damage)
    {
        if (hitTimer >= invicibilityTime) 
        {
            hitTimer = 0;
            int droppedMoney = 0;
            if (Money == 0)
                hitAtZero = true;
            else if (damage > Money)
            {
                droppedMoney = Money;
                Money = 0;
            }
            else if (damage <= Money) 
            {
                droppedMoney = damage;
                Money -= damage;
            }
            DropMoney(droppedMoney);
        }
    }

    // If the player dies, play death animation and 
    protected override void onDeath()
    {
        /// Todo::
        /// - Play death animation
        /// - Restart from the beginning of the level
        PlayerController pc = GetComponent<PlayerController>();
        pc.DisableAttack();
        pc.DisableMove();
    }

    private void DropMoney(int amount) 
    {
        cashPool.SpawnCollectables(amount, this.transform.position, moneyDropDistance, moneyDropTime);
    }

    private void Start()
    {
        cashPool = GameObject.FindWithTag("Cash Pool").GetComponent<CollectablePool>();
    }

    private void Update()
    {
        hitTimer += Time.deltaTime;
    }
}
