using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A wrapper for the Unity Character Controller class that allows objects to move on screen.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public abstract class Moveable : MonoBehaviour
{
    /// <summary>
    /// The base speed of this moveable object.
    /// </summary>
    [SerializeField]
    private float speed;

    /// <summary>
    /// How much objects move vertically vs horizontally.
    /// (In beat'em-ups, characters move more horizontally than they do vertically due to the perspective of the camera.)
    /// </summary>
    private static Vector2 movementRatio = new Vector2(1,0.75f);

    /// <summary>
    /// The Unity Character Controller.
    /// </summary>
    private CharacterController controller;

    /// <summary>
    /// Get the Unity Character Controller when this object is active.
    /// </summary>
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    /// <summary>
    /// Moves a character in the given direction.
    /// </summary>
    /// <param name="direction">The direction to move the object in.</param>
    protected void Move(Vector2 direction)
    {
        Vector2 normalized = direction;

        if (direction.magnitude > 1)
        {
            normalized = direction.normalized;
        }

        controller.Move(new Vector3(normalized.x * speed * movementRatio.x, normalized.y * speed * movementRatio.y, 0));
    }
}
