using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A controller that moves the player around.
/// </summary>
public class PlayerController : Moveable
{
    /// <summary>
    /// The animator of the player.
    /// </summary>
    private Animator anim;

    /// <summary>
    /// Whether the player can move (through inputs).
    /// This is controlled with events in the player animations.
    /// </summary>
    private bool canMove = true;
    
    /// <summary>
    /// Whether the player can attack (through inputs).
    /// This is controlled with events in the player animations.
    /// </summary>
    private bool canAttack = true;

    /// <summary>
    /// Whether the player is facing right or not.
    /// </summary>
    [SerializeField]
    private bool facingRight;

    /// <summary>
    /// The name of the bool that the animator uses to play the walking animation.
    /// </summary>
    private const string walkingAnimatorKey = "Is Walking";

    /// <summary>
    /// The name of the trigger that the animator uses to play the light attack animation.
    /// </summary>
    private const string lightAttackAnimatorKey = "Light Attack";

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }

    public void DisableMove()
    {
        canMove = false;
    }

    public void EnableMove()
    {
        canMove = true;
    }

    public void DisableAttack()
    {
        canAttack = false;
    }

    public void EnableAttack()
    {
        canAttack = true;
    }

    private void FixedUpdate()
    {

        //Process the player movement for this frame...
        {
            Vector2 direction = Vector2.zero;

            if (canMove)
            {
                direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            }

            //Face the player in the right direction...
            if ((direction.x > 0 && !facingRight) || (direction.x < 0 && facingRight))
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                facingRight = !facingRight;
            }

            //Play the walking animation (or don't)...
            if (direction.magnitude > 0)
            {
                anim.SetBool(walkingAnimatorKey, true);
            }
            else
            {
                anim.SetBool(walkingAnimatorKey, false);
            }

            //Actually move the player.
            Move(direction);
        }


        //When player can attack and wants to, attack.
        if (Input.GetMouseButton(0) && canAttack)
        {
            anim.SetTrigger(lightAttackAnimatorKey);
        }

    }
}
