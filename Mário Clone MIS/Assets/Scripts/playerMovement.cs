using UnityEngine;
using System.Collections;   
using System.Collections.Generic;

public class playerMovement : MonoBehaviour
{
    public Rigidbody2D rig;
    public Animator anim;
    public float speed;
    public float jumpForce;

    private Vector2 direction;

    // Update is called once per frame
    void Update()
    {
        // read input in Update     
        float vy;
        if (rig != null)
            vy = rig.linearVelocity.y;
        else
            vy = 0f;
        direction = new Vector2(Input.GetAxisRaw("Horizontal") * speed, vy);
    }

    private void FixedUpdate()
    {
        // apply velocity in FixedUpdate
        if (rig != null)
            rig.linearVelocity = direction;
    }

    private void Awake()
    {
        if (rig == null)
            rig = GetComponent<Rigidbody2D>();
    }
}
