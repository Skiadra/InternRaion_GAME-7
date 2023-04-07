using System;
using System.Collections;
using UnityEngine;
using static SkillTree;

public class Movement : MonoBehaviour
{
    public static Movement move;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private BoxCollider2D collide;
    private Animator anima;
    [SerializeField] private TrailRenderer tr;

    [SerializeField] private LayerMask jumpableGround;

    //SaveLoadSystem
    [SerializeField] private bool saveLoadSystem;

    public float speed;
    public bool inControl;
    private float multiplierX;
    public float maxFallSpeed;
    public float normGravity;
    private float dirX;
    private float varX;
    private float facing;
    private float acceleration;
    public float jumpForce;
    public bool doubleJump;
    private int doubleJumpCount;
    [SerializeField] private float coyoteTime;
    private float jumpTimeCounter;
    //Dashing Var
    [SerializeField] private bool canDash;
    private bool isDashing = false;
    private float dashingTime = 0.15f;
    private float dashingCooldown = 1f;
    private bool dashReset;
    public bool canDashReset = false;
    [SerializeField] private float dashingPower;
    //WallMovement
    private bool wallHug;
    public float wallHugTime;
    private float wallHugCounter;
    public bool absorb = false;
    private bool wallJump;
    private int wallJumpCounter = 0;
    public int maxWallJump;
    private float wallJumpTime;
    // public float xWallJump;
    public float yWallJump;


    private enum Status { idle, walking, running, jumping, falling}
    private Status state = Status.idle;
    private void Awake(){ move = this; }

    // Start is called before the first frame update
    private void Start()
    {
        if (GameManager.loadStat)
        {
            loadPos();
            GameManager.loadStat = false;
        }
        if (!GameManager.newStat)
        {
            loadData();
            Debug.Log("Loaded");
        }
        GameManager.newStat = false;
        skillTree.skillPointAdd();
        saveData();
        rb =  GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        collide = GetComponent<BoxCollider2D>();
        anima = GetComponent<Animator>();
        doubleJumpCount = 0;
        inControl = true;
    }

    // Update is called once per frame
    private void Update()
    {   
        if (!inControl)
        {
            rb.velocity = new Vector2(0,rb.velocity.y);//Kalo lagi buka menu ga bisa gerak
            return;
        }
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
            wallHugCounter = wallHugTime;
            if (canDashReset) dashReset = true;
        } else
        {
            jumpTimeCounter -= Time.deltaTime;
        }

        //Double Jump
        if (doubleJumpCount < 1 && Input.GetKeyDown(KeyCode.X) && !IsGrounded() && !wallHug && jumpTimeCounter <= .03f)
        {
            rb.velocity = new Vector2 (rb.velocity.x, jumpForce);
            if (dashReset) 
            {
                StopCoroutine(Dash());
                canDash = true;
                dashReset = false;
            }
            doubleJumpCount++;
        }
        if (doubleJump == true)
        {
            if (IsGrounded()) doubleJumpCount = 0;
        } else doubleJumpCount = 1;

        //Jump
        if (Input.GetKeyDown(KeyCode.X) && jumpTimeCounter > 0f)
        { 
            rb.velocity = new Vector2 (rb.velocity.x, jumpForce);
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
        if(OnTheWall() == true && IsGrounded() == false && Input.GetButton("Horizontal") && rb.velocity.y <= 5f && wallHugCounter > 0)
        {
            wallHug = true; //Aktivasi wallhug
            wallJumpTime = .08f;
            wallHugCounter -= Time.deltaTime;
        } else
        {
            wallHug = false;//Wallhug mati
            if (wallJumpTime > 0)
                wallJumpTime -= Time.deltaTime;
            if (rb.velocity.y < 0 && !wallHug)
            {
                rb.gravityScale = 8;
            }
            else
                gravityOn();
        }

        if(wallHug)
        {
            if (Input.GetKey(KeyCode.Space) && absorb)
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
        if(Input.GetKeyDown(KeyCode.X) && wallJumpTime > 0 && wallJumpCounter < maxWallJump)
        {
            wallJump = true; //Aktivasi walljump
            wallJumpTime = 0;
            Invoke("WallJumpOff", 0.05f); //Mematikan wall jump dalam .05 detik
        }

        if(wallJump)
        {
            float temp = jumpForce;
            if (jumpForce >  25) jumpForce -= 5;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); //mengubah velocity
            jumpForce = temp;
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

        Debug.Log(dashReset);

        Animate();
    }

    private void Animate()
    {
        Status state;
        
        if (dirX > 0 && wallHug == false)
        {
            sprite.flipX = true;
            state = Status.walking;
        } else if (dirX < 0 && wallHug == false)
        {
            state = Status.walking;
            sprite.flipX = false;
        } else
        {
            state = Status.idle;
        }

        if (rb.velocity.y > 0.01 && !wallHug)
        {
            state = Status.jumping;
        } else if (rb.velocity.y < -0.1 && IsGrounded() == false && !wallHug)
        {
            state = Status.falling;
        }

        if (isDashing)
        {
            state = Status.running;
        }

        if(wallJump)
        {
            sprite.flipX = true;
        }

        anima.SetInteger("state", (int)state);
    }

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

    //Save-Load method
    public void saveData()
    {
        SaveSystem.SavePlayer(this, skillTree);
    }

    public void loadData()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        doubleJump = data.canDoubleJump;
        canDashReset = data.canDashReset;
        jumpForce = data.jumpForce;
        absorb = data.canAbsorb;
        maxFallSpeed = data.maxFallSpeed;
        maxWallJump = data.maxWallJump;
        skillTree.addSkillPoints = data.skillPointsEachLevel;
        for (int i = 0; i < skillTree.unlocked.Length; i++)
        {
            skillTree.unlocked[i] = false;
        }
        for (int i = 0; i < skillTree.unlocked.Length; i++)
        {
            skillTree.unlocked[i] = data.unlockedSkill[i];
        }
        skillTree.skillPoint = data.points;
        skillTree.UpdateSkillUI();
    }

    public void loadPos()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        Vector2 pos;
        pos.x = data.position[0];
        pos.y = data.position[1];

        transform.position = pos;
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
