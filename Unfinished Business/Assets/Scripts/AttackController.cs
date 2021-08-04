using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    // The reference point for determining depth
    [SerializeField]
    GameObject bodyRef;

    // The tag specifying which objects can be damaged by this attack
    [SerializeField]
    string hitTag;

    // The damage of the attack
    [SerializeField]
    int damage;

    // The depth range of the attack
    [SerializeField]
    float depthTolerance;

    // The colliders that have already been hit during this attack
    [SerializeField]
    List<GameObject> hitObjects;

    // Initializes the previously hit colliders list
    private void Awake()
    {
        hitObjects = new List<GameObject>();
    }

    // Clears the previously hit colliders list before each attack
    private void OnDisable()
    {
        hitObjects.Clear();
    }

    // Detects collisions with colliders on objects with the hit tag
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject obj = other.gameObject;
        if (HasHitTag(obj) && !IsMultiHit(obj) && IsInRange(obj))
        {
            HealthController objHP = obj.GetComponent<HealthController>();
            objHP.Damage(damage);
            hitObjects.Add(obj);
        }
    }

    // Determines if the object hit has the correct tag
    private bool HasHitTag(GameObject obj) 
    {
        return obj.CompareTag(hitTag);
    }

    // Determines if the object hit has been hit before by the same attack
    private bool IsMultiHit(GameObject obj) 
    {
        return hitObjects.Contains(obj);
    }

    // Determines if the object hit is within the depth range for the attack
    private bool IsInRange(GameObject obj) 
    {
        float depth = Mathf.Abs(obj.transform.position.z - bodyRef.transform.position.z);
        return depth <= depthTolerance;
    }
}
