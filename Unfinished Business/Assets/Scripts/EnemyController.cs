using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : Moveable
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float nextWaypointDistance = 3f;

    private Path path;
    private int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;

    private Animator anim;

    /// <summary>
    /// The name of the bool that the animator uses to play the walking animation.
    /// </summary>
    private const string walkingAnimatorKey = "Is Walking";

    /// <summary>
    /// The name of the trigger that the animator uses to play the attack animation.
    /// </summary>
    private const string lightAttackAnimatorKey = "Attack";

    [SerializeField]
    private bool facingRight;

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

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, 0.5f);

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

    private void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(transform.position, target.position, OnPathComplete);
        }
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }


    private void FixedUpdate()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)(path.vectorPath[currentWaypoint] - transform.position)).normalized;

        direction = PerspectiveUtilities.UnForeshortenVector(direction).normalized;

        Move(direction);

        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);

        //Face the enemy in the right direction...
        if ((direction.x > 0 && !facingRight) || (direction.x < 0 && facingRight))
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            facingRight = !facingRight;
        }

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }


}
