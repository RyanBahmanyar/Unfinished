using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A wrapper for the Unity Character Controller class that allows objects to move on screen.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Moveable : MonoBehaviour
{
    /// <summary>
    /// The base speed of this moveable object.
    /// </summary>
    [SerializeField]
    private float speed;

    /// <summary>
    /// The "acceleration" of the movement of the player.
    /// </summary>
    [SerializeField]
    private float maxVelocityChange;

    /// <summary>
    /// How much objects move vertically vs horizontally.
    /// (In beat'em-ups, characters move more horizontally than they do vertically due to the perspective of the camera.)
    /// </summary>
    private static Vector2 movementRatio = new Vector2(1,0.75f);

    /// <summary>
    /// The Unity Character Controller.
    /// </summary>
    private Rigidbody2D controller;

    /// <summary>
    /// Get the Unity Character Controller when this object is active.
    /// </summary>
    protected virtual void Awake()
    {
        controller = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Moves a character in the given direction.
    /// </summary>
    /// <param name="direction">The direction to move the object in.</param>
    protected void Move(Vector2 direction)
    {
        Vector2 corrected = direction;

        if (direction.magnitude > 1)
        {
            corrected = direction.normalized;
        }
        
        Vector2 goalVelocity = new Vector2(corrected.x * speed * movementRatio.x, corrected.y * speed * movementRatio.y);

        Debug.Log(goalVelocity);

        Vector2 currentVelocity = controller.velocity;

        Vector2 difference = goalVelocity - currentVelocity;

        if (difference.magnitude > maxVelocityChange)
        {
            difference = difference.normalized * maxVelocityChange;
        }


        controller.velocity += difference;
    }
}
