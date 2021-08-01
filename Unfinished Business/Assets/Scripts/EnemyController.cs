using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Seeker))]
public class EnemyController : Moveable
{
    /// <summary>
    /// The object the Enemy tries to follow (the player's transform).
    /// </summary>
    [SerializeField]
    private Transform targetObj;

    /// <summary>
    /// The actual point in space the Enemy will move to.
    /// </summary>
    private Vector3 trueTarget;
    /// <summary>
    /// The distance from a waypoint the Enemy feels is confortable to move to the next waypoint.
    /// </summary>
    [SerializeField]
    private float nextWaypointDistance = 3f;

    /// <summary>
    /// The distance the Enemy wants to be from its target.
    /// </summary>
    [SerializeField]
    private float goalPlayerDistance = 1f;

    /// <summary>
    /// How far off the Enemy is ok with being from the target distance.
    /// </summary>
    [SerializeField]
    private float goalMargin = 0.5f;

    /// <summary>
    /// How much higher or lower the Enemy is comfortable with being relative to the player.
    /// (An Enemy directly below the player will not hit the player)
    /// </summary>
    [SerializeField]
    private float maxHeightDifference = 0.5f;
    /// <summary>
    /// How far off the Enemy is ok with being from the target heigh range.
    /// </summary>
    [SerializeField]
    private float heightMargin = 0.25f;

    /// <summary>
    /// The path that the Enemy is following.
    /// A path is a series of waypoints.
    /// </summary>
    private Path path;
    /// <summary>
    /// The index of current waypoint of the path the Enemy is moving towards.
    /// </summary>
    private int currentWaypoint = 0;

    /// <summary>
    /// The script that develops a path to the target.
    /// </summary>
    Seeker seeker;

    /// <summary>
    /// The animator of the Enemy.
    /// </summary>
    private Animator anim;

    /// <summary>
    /// The name of the bool that the animator uses to play the walking animation.
    /// </summary>
    private const string walkingAnimatorKey = "Is Walking";

    /// <summary>
    /// The name of the trigger that the animator uses to play the attack animation.
    /// </summary>
    private const string attackAnimatorKey = "Light Attack";

    /// <summary>
    /// Whether the Enemy is facing right.
    /// </summary>
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

    /// <summary>
    /// Get Components and start looking for paths.
    /// </summary>
    private void Start()
    {
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, 0.1f);

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

    /// <summary>
    /// Looks for a new path for the Enemy to follow.
    /// </summary>
    private void UpdatePath()
    {
        if (seeker.IsDone())
        {
            Vector2 radialPoint = transform.position;

            //Keep the target position within the height restrictions.
            if (transform.position.y - targetObj.position.y > maxHeightDifference)
            {
                radialPoint.y = targetObj.position.y + maxHeightDifference;
            }
            else if (targetObj.position.y - transform.position.y > maxHeightDifference)
            {
                radialPoint.y = targetObj.position.y - maxHeightDifference;
            }

            //Keep the target position within distance restrictions.
            trueTarget = ClosestPointOnEllipse(goalPlayerDistance, targetObj.position, radialPoint);

            seeker.StartPath(transform.position, trueTarget, OnPathComplete);
        }
    }

    /// <summary>
    /// Receives an updated path when generated.
    /// </summary>
    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    /// <summary>
    /// Finds the point on an ellipse (forshortened circle) closest to a point not on the ellipse.
    /// </summary>
    /// <param name="circleRadius">The radius of the circle that when forshortened makes the ellipse.</param>
    /// <param name="circleCenter">The center of the ellipse / circle.</param>
    /// <param name="position">The point not on the ellipse.</param>
    /// <returns>The closest point on the ellipse.</returns>
    private static Vector2 ClosestPointOnEllipse(float circleRadius, Vector2 circleCenter, Vector3 position)
    {
        Vector3 direction = (PerspectiveUtilities.UnForeshortenVector(position) - PerspectiveUtilities.UnForeshortenVector(circleCenter)).normalized;
        direction *= circleRadius;

        return PerspectiveUtilities.ForeshortenVector(direction) + circleCenter;
    }

    private void FixedUpdate()
    {
        //If a path can't be generated to the player, the AI is stuck.
        if (path == null)
        {
            anim.SetBool(walkingAnimatorKey, false);
            return;
        }
        
        
        //Get the direction to the next waypoint.
        Vector2 direction = Vector2.zero;
        if (currentWaypoint < path.vectorPath.Count)
            direction = path.vectorPath[currentWaypoint] - transform.position;

        direction = PerspectiveUtilities.UnForeshortenVector(direction);

        //Check if the Enemy is in a comfortable spot / can move at all.
        float distanceFromTarget = PerspectiveUtilities.GetForeshortenedDistance(transform.position, targetObj.position);
        bool acceptableDistance = distanceFromTarget < goalPlayerDistance + goalMargin && distanceFromTarget > goalPlayerDistance - goalMargin;
        bool acceptableHeight = Mathf.Abs(transform.position.y - targetObj.position.y) < maxHeightDifference + heightMargin;

        if (acceptableDistance && acceptableHeight || !canMove)
        {
            direction = Vector2.zero;

            anim.SetBool(walkingAnimatorKey, false);

            //Attack if the Enemy is in a desired position and is able to.
            if (canAttack)
            {
                anim.SetTrigger(attackAnimatorKey);
            }
        }
        else
        {
            anim.SetBool(walkingAnimatorKey, true);
        }

        Move(direction);

        float waypointDistance = float.PositiveInfinity;

        //Check if the Enemy needs to move to another waypoint.
        if (currentWaypoint < path.vectorPath.Count)
            waypointDistance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);

        if (waypointDistance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
        

        //Face the Enemy towards the target...
        if (canMove && (transform.position.x < targetObj.position.x && !facingRight) || (transform.position.x > targetObj.position.x && facingRight))
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            facingRight = !facingRight;
        }
    }


}
