using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private BoxCollider2D coll;
    private float speed = 7f;
    private float jumpForce = 14f;
    [SerializeField] private LayerMask jumpGround;

    private enum MovingState { idle = 0, running = 1, jumping = 2, falling = 3 }
    private float dirX;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * speed, rb.velocity.y);
        if(Input.GetButton("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        UpdateAnimationState();
    }

    void UpdateAnimationState()
    {
        MovingState state;
        if(dirX > 0f)
        {
            state = MovingState.running;
            sprite.flipX = false;
        }
        else if(dirX < 0f)
        {
            state = MovingState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovingState.idle;
        }

        if(rb.velocity.y  > .1f)
        {
            state = MovingState.jumping;
        }
        else if(rb.velocity.y < -.1f)
        {
            state = MovingState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f,
            Vector2.down, .1f, jumpGround);
    }
}
