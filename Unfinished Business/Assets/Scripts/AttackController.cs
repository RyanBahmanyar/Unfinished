using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    // The colliders that have already been hit during this attack
    [SerializeField]
    List<Collider2D> hitColliders;

    // The tag specifying which objects can be damaged by this attack
    [SerializeField]
    string hitTag;

    // The damage of the attack
    [SerializeField]
    int damage;

    // Initializes the previously hit colliders list
    private void Awake()
    {
        hitColliders = new List<Collider2D>();
    }

    // Clears the previously hit colliders list before each attack
    private void OnDisable()
    {
        hitColliders.Clear();
    }

    // Detects collisions with colliders on objects with the hit tag
    private void OnTriggerEnter2D(Collider2D other)
    {
        System.Console.WriteLine(other.tag);
        if (other.gameObject.CompareTag(hitTag) && !hitColliders.Contains(other)) 
        {
            HealthController otherHP = other.GetComponent<HealthController>();
            otherHP.Damage(damage);
            hitColliders.Add(other);
        }
    }
}
