using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private BoxCollider2D collide;
    private Animator anima;
    [SerializeField] private TrailRenderer tr;

    [SerializeField] private LayerMask jumpableGround;

    public float speed;
    private bool airControl;
    private float multiplierX;
    public float maxFallSpeed;
    public float normGravity;
    private float dirX;
    private float varX;
    private float facing;
    private float acceleration;
    [SerializeField] private float coyoteTime;
    private float jumpTimeCounter;
    //Dashing Var
    [SerializeField] private bool canDash;
    private bool isDashing = false;
    private float dashingTime = 0.15f;
    private float dashingCooldown = 1f;
    [SerializeField] private float dashingPower;
    //WallMovement
    private bool wallHug;
    private bool wallJump;
    private int wallJumpCounter = 0;
    // public float xWallJump;
    public float yWallJump;


    // private enum Status { idle, walking, running, jumping, falling}
    // private Status state = Status.idle;

    // Start is called before the first frame update
    private void Awake()
    {
        rb =  GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        collide = GetComponent<BoxCollider2D>();
        anima = GetComponent<Animator>();
        airControl = true;
    }

    // Update is called once per frame
    private void Update()
    {   
        if(isDashing)
        {
            return;
        }   
        //HORIZONTAL MOVEMENT
        acceleration = 0;
        multiplierX = 0;
        dirX = Input.GetAxis("Horizontal");
        varX = Input.GetAxisRaw("Horizontal");

        //Kalo di pencet kanan kiri speed max
        if(varX != 0)
        {
            facing = varX;
            multiplierX = varX;
        } else if (dirX !=0 && varX == 0) //Kalau horizontal move key baru dilepas bakal tetep ada akselerasi
        {
            acceleration = (dirX/4)*speed;
        }

        rb.velocity = new Vector2(multiplierX*speed+acceleration, rb.velocity.y); //rubah velocity
        
        //VERTICAL MOVEMENT
        if(IsGrounded())
        {
            jumpTimeCounter = coyoteTime;
        } else
        {
            jumpTimeCounter -= Time.deltaTime;
        }

        //Jump
        if (Input.GetKeyDown(KeyCode.X) && jumpTimeCounter > 0f)
        { 
            rb.velocity = new Vector2 (rb.velocity.x, 25f);
            jumpTimeCounter = 0;
        }

        if (Input.GetKeyUp(KeyCode.X) && rb.velocity.y > 0f)
        {
            jumpTimeCounter = 0;
        }

        //Maximum falling speed
        if (rb.velocity.y < maxFallSpeed)    
            rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);

        //Wall Hug
        if(OnTheWall() == true && IsGrounded() == false && Input.GetButton("Horizontal") && rb.velocity.y <= .5f)
        {
            wallHug = true; //Aktivasi wallhug
        } else
        {
            wallHug = false; //Wallhug mati
            if (rb.velocity.y < 0 && !wallHug)
                rb.gravityScale = 12;
            else
                gravityOn();
        }

        if(wallHug)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                rb.velocity = new Vector2(rb.velocity.x, 0); //Kalo nahan space ga turun
                gravityOff();
            }
            else
            {
                gravityOn();
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y/2);
            }
        } 

        //Wall Jump
        if(Input.GetKeyDown(KeyCode.X) && wallHug && wallJumpCounter < 2)
        {
            wallJump = true; //Aktivasi walljump
            Invoke("WallJumpOff", 0.05f); //Mematikan wall jump dalam .05 detik
        }

        if(wallJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, yWallJump); //mengubah velocity
            wallHug = false; //wallhug mati
            tr.emitting = true; //efek trail nyala
            Invoke("EmitOff", 0.25f); //mematikan efek trail dalam 0.25 detik
        }
        if(IsGrounded())
        {
            wallJumpCounter = 0;
        }

        //Dash
        if (Input.GetKeyDown(KeyCode.Z) && canDash && !wallHug) 
        {
            StartCoroutine(Dash());//Menyalakan dash
        }

        // Animate();
    }

    // private void Animate()
    // {
    //     // Status state;
        
    //     // if (dirX > 0 && wallHug == false)
    //     // {
    //     //     sprite.flipX = true;
    //     //     state = Status.walking;
    //     //     if (rb.velocity.x >= 20)
    //     //     {
    //     //         state = Status.running;
    //     //     }
    //     // } else if (dirX < 0 && wallHug == false)
    //     // {
    //     //     state = Status.walking;
    //     //     sprite.flipX = false;
    //     //     if (rb.velocity.x <= -20)
    //     //     {
    //     //         state = Status.running;
    //     //     }
    //     // } else
    //     // {
    //     //     state = Status.idle;
    //     // }

    //     // if (rb.velocity.y > 0.01)
    //     // {
    //     //     state = Status.jumping;
    //     // } else if (rb.velocity.y < -0.1 && IsGrounded() == false)
    //     // {
    //     //     state = Status.falling;
    //     // }

    //     // if(wallJump)
    //     // {
    //     //     sprite.flipX = true;
    //     // }

    //     // anima.SetInteger("state", (int)state);
    // }

    private bool IsGrounded()
    {

        return Physics2D.BoxCast(collide.bounds.center, collide.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }

    private bool OnTheWall()
    {
        return Physics2D.BoxCast(collide.bounds.center, new Vector2(collide.size.x+.2f, collide.size.y-.5f), 0f, new Vector2(0, 0), 0.1f, jumpableGround);
    }

    private void WallJumpOff()
    {
        wallJump = false;
        wallJumpCounter++;
    }
    
    private void gravityOff()
    {
        rb.gravityScale = 0;
    }

    private void gravityOn()
    {
        rb.gravityScale = normGravity;
    }
    private void EmitOff()
    {
        tr.emitting = false;
    }

    private void enableAirControl()
    {
        airControl = true;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0; //Gravitasi menjadi 0
        rb.velocity = new Vector2(facing*dashingPower, 0); //Mengubah velocity
        tr.emitting = true; //Trail nyala
        yield return new WaitForSeconds(dashingTime);
        dirX = .4f;
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

}
