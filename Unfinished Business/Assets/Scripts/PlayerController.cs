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
    [SerializeField]
    private bool canMove = true;
    
    /// <summary>
    /// Whether the player can attack.
    /// This is controlled with events in the player animations.
    /// </summary>
    [SerializeField]
    private bool canAttack = true;

    /// <summary>
    /// Whether the player can dash.
    /// This is controlled with events in the player animations.
    /// </summary>
    private bool canDash = true;

    /// <summary>
    /// Time until next dash.
    /// </summary>
    private float dashTimer = 0;

    /// <summary>
    /// Time between dashes.
    /// </summary>
    [SerializeField]
    private float dashPause = 1f;

    /// <summary>
    /// Whether the player is facing right or not.
    /// </summary>
    [SerializeField]
    private bool facingRight;

    /// <summary>
    /// The collider that detects oncoming attacks.
    /// </summary>
    private Collider2D damageBox;

    /// <summary>
    /// The object that detects collisions between the player and obstacles.
    /// (Moved to a different layer during dash).
    /// </summary>
    [SerializeField]
    private GameObject foot;

    /// <summary>
    /// The name of the bool that the animator uses to play the walking animation.
    /// </summary>
    private const string walkingAnimatorKey = "Is Walking";

    /// <summary>
    /// The name of the trigger that the animator uses to play the light attack animation.
    /// </summary>
    private const string lightAttackAnimatorKey = "Light Attack";

    /// <summary>
    /// The name of the trigger that the animator uses to play the projectile attack animation.
    /// </summary>
    private const string projectileAttackAnimatorKey = "Projectile Attack";

    /// <summary>
    /// The name of the trigger that the animator uses to play the dash animation.
    /// </summary>
    private const string dashAnimatorKey = "Dash";

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
        damageBox = GetComponent<Collider2D>();
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

    public void DisableDash()
    {
        canDash = false;
    }

    public void EnableDash()
    {
        canDash = true;
    }

    public void DashEffectOn()
    {
        foot.layer = 11;
        damageBox.enabled = false;
    }

    public void DashEffectOff()
    {
        foot.layer = 8;
        damageBox.enabled = true;
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

            //Dash processing
            dashTimer += Time.deltaTime;
            dashTimer = Mathf.Min(dashTimer, dashPause);

            if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftShift)) && canDash && dashTimer >= dashPause)
            {
                Dash(direction);
                dashTimer = 0;
                anim.SetTrigger(dashAnimatorKey);
            }
            
        }


        //When player can attack and wants to, attack.
        if (canAttack)
        {
            if (Input.GetMouseButton(0))
            {
                anim.SetTrigger(lightAttackAnimatorKey);
            }
            else if(Input.GetMouseButton(1))
            {
                anim.SetTrigger(projectileAttackAnimatorKey);
            }
        }

    }
}
