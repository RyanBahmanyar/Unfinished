using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains methods for handling received damage and character death
/// </summary>
public abstract class HealthController : MonoBehaviour
{
    /// <summary>
    /// The method to call from an opposing hitbox.
    /// </summary>
    /// <param name="damage">The damage of the incoming attack</param>
    public void Damage(int damage) 
    {
        onDamage(damage);
        if (isDead())
            onDeath();
    }

    /// <summary>
    /// Handles the incoming damage from an opposing hitbox
    /// </summary>
    /// <param name="damage">The damage of the incoming attack</param>
    protected abstract void onDamage(int damage);

    /// <summary>
    /// Check for the death conditions of the attached character
    /// </summary>
    /// <returns></returns>
    protected abstract bool isDead();

    /// <summary>
    /// Handles the death event of the attached character
    /// </summary>
    protected abstract void onDeath();
}
