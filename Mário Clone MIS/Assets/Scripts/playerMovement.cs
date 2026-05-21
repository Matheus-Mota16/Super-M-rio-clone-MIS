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

    private Vector2 direction;

    void Update()
    {
        
        direction = new Vector2(Input.GetAxisRaw("Horizontal") * speed, rig.linearVelocity.y);

        if(Input.GetAxisRaw("Horizontal") < 0)
        {
            sprite.flipX = true;
        }

        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            sprite.flipX = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate()
    {
            rig.linearVelocity = direction;

        if(direction.sqrMagnitude > 0)
        {
            anim.Play("playerRun");
        }
        else
        {
            anim.Play("playerIdle");
        }
    }

   
}
