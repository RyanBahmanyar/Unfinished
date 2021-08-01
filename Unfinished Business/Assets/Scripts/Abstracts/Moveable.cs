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
    /// The Unity Rigidbody 2D.
    /// </summary>
    private Rigidbody2D controller;

    /// <summary>
    /// The speed at which this character can dash.
    /// </summary>
    [SerializeField]
    private float dashSpeed;

    /// <summary>
    /// Get the Unity Character Controller when this object is active.
    /// </summary>
    protected virtual void Awake()
    {
        controller = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Moves a character in the given direction.
    /// Call this once per frame.
    /// </summary>
    /// <param name="direction">The direction to move the object in.</param>
    protected void Move(Vector2 direction)
    {
        Vector2 corrected = direction;

        if (direction.magnitude > 1)
        {
            corrected = direction.normalized;
        }
        
        Vector2 goalVelocity = new Vector2(corrected.x * speed * PerspectiveUtilities.perspectiveRatio.x, corrected.y * speed * PerspectiveUtilities.perspectiveRatio.y) * Time.deltaTime;

        Vector2 currentVelocity = controller.velocity;

        Vector2 difference = goalVelocity - currentVelocity;

        if (difference.magnitude > maxVelocityChange)
        {
            difference = difference.normalized * maxVelocityChange;
        }

        controller.velocity += difference;
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }


    /// <summary>
    /// Gives a short burst of velocity in a given direction.
    /// </summary>
    /// <param name="direction">The direction to move in.</param>
    protected void Dash(Vector2 direction)
    {
        Vector2 velocityChange = PerspectiveUtilities.ForeshortenVector(direction.normalized * dashSpeed);
        controller.velocity += velocityChange;
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }
}
