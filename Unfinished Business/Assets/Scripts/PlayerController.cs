using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Moveable
{
    private Animator anim;

    private bool canMove = true;

    [SerializeField]
    private bool facingRight;

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Vector2 direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if ((direction.x > 0 && !facingRight)|| (direction.x < 0 && facingRight))
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                facingRight = !facingRight;
            }

            Move(direction);
        }
    }
}
