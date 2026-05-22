using UnityEngine;
using System.Collections;   
using System.Collections.Generic;

public class playerMovement : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Rigidbody2D rig;
    public Animator anim;
    public float speed;
    public float jumpForce;
    [SerializeField] bool isJump;
    [SerializeField] bool inFloor = true;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.1f;

    private Vector2 direction;

    void Update()
    {
        
        // preserve current vertical velocity when setting horizontal
        float vy = rig != null ? rig.linearVelocity.y : 0f;
        direction = new Vector2(Input.GetAxisRaw("Horizontal") * speed, vy);

        if(Input.GetAxisRaw("Horizontal") < 0)
        {
            sprite.flipX = true;
        }

        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            sprite.flipX = false;
        }

        // update ground check
        if (groundCheck != null)
            inFloor = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) != null;

        // jump only if grounded
        if (Input.GetButtonDown("Jump") && inFloor)
        {
            if (rig != null)
                rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate()
    {
        if (rig != null)
            rig.linearVelocity = direction;

        if (direction.sqrMagnitude > 0)
        {
            anim.Play("playerRun");
        }
        else
        {
            anim.Play("playerIdle");
        }
    }

    private void Awake()
    {
        if (rig == null)
            rig = GetComponent<Rigidbody2D>();
    }

   
}
