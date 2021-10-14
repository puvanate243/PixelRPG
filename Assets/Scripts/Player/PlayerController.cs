using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator am;

    [SerializeField]
    private float RunSpeed = 1f, JumpSpeed = 4f;
    private float ScaleX, _move;
    private float LastAttacked;
    private float AttackDelay = 2.5f;
    private float Stamina = 100f;

    private int AttackCount;

    private bool Grounded = true;
    private bool CanMove = true;
    private bool Landing = false;


    [SerializeField]
    private LayerMask GroundLayer;
    [SerializeField]
    private LayerMask PlatformLayer;
    [SerializeField]
    private LayerMask EnemyLayer;

    [SerializeField]
    private BoxCollider2D BoxGroundCheck;
    [SerializeField]
    private BoxCollider2D WallCheck1;
    [SerializeField]
    private BoxCollider2D WallCheck2;
    [SerializeField]
    private BoxCollider2D AttackCheck;

    //Stamina
    [SerializeField]
    private Image ImageStaminaBar;
    private bool CanRestamina = true;

    //Health
    [SerializeField]
    private HealthController HealthController;
    [SerializeField]
    private Image ImageHealthBar;
    private bool CanRehealth = true;


    void Start()
    {
        Setup();
    }
    private void Setup()
    {
        rb = GetComponent<Rigidbody2D>();
        am = GetComponent<Animator>();
        ScaleX = transform.localScale.x;
    }

    void Update()
    {
        Movement();
        Attack();
        Climbing();
        HealthControl();
        StaminaControl();
    }
    private void GroundCheck()
    {
        RaycastHit2D GroundBox = Physics2D.BoxCast(BoxGroundCheck.bounds.center, BoxGroundCheck.bounds.size, 0f, Vector2.zero, 1f, GroundLayer);
        RaycastHit2D PlatformBox = Physics2D.BoxCast(BoxGroundCheck.bounds.center, BoxGroundCheck.bounds.size, 0f, Vector2.zero, 1f, PlatformLayer);
        if (GroundBox.collider != null || PlatformBox.collider != null)
        {
            Grounded = true;
            if (Landing)
            {
                am.SetBool("Jump", false);
                Landing = false;
            }
        }
        else
        {
            Grounded = false;
            Landing = true;
        }

    }
    private void Movement()
    {
        if (CanMove)
        {
            _move = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(_move * RunSpeed, rb.velocity.y);
            rb.gravityScale = 0.8f;
            am.SetFloat("Speed", SpeedCalculate(_move));
            if (_move > 0)  //scale
            {
                transform.localScale = new Vector3(ScaleX, transform.localScale.y, transform.localScale.z);
            }
            if (_move < 0)
            {
                transform.localScale = new Vector3(-ScaleX, transform.localScale.y, transform.localScale.z);
            }

            Jump();
        }
        else
        {
            am.SetFloat("Speed", SpeedCalculate(0));
            rb.velocity = new Vector2(0, 0);
            rb.gravityScale = 0;
        }

        GroundCheck();
    }
    private void Jump()
    {
        if (Grounded)
        {
            if (Input.GetKeyDown("space"))
            {
                rb.velocity = new Vector2(rb.velocity.x, JumpSpeed);
                am.SetBool("Jump", true);
            }
        }
    }
    private float SpeedCalculate(float x)
    {
        return x >= 0f ? x : -x;
    }

    private void Attack()
    {
        if (Time.time - LastAttacked > AttackDelay)
        {
            AttackCount = 0;
            am.SetBool("AttackA", false);
            am.SetBool("AttackB", false);
        }

        if (Input.GetKeyDown("k") && Grounded)
        {
            AttackCount++;
            LastAttacked = Time.time;
            if (AttackCount == 1 && Stamina >= 30)
            {
                am.SetBool("AttackA", true);
                AttackCount = Mathf.Clamp(AttackCount, 0, 3);
                CanMove = false;
                ReduceStamina(30);
            }
        }
    }
    private void ReturnAttackA()
    {
        am.SetBool("AttackA", false);

        if (AttackCount >= 2 && Stamina >= 30)
        {
            am.SetBool("AttackB", true);
            CanMove = false;
            ReduceStamina(30);
        }
        else
        {
            AttackCount = 0;
            CanMove = true;
        }
    }

    private void ReturnAttackB()
    {
        //if (AttackCount > 2 && Stamina >= 30)
        //{
        //    am.SetBool("AttackC", true);
        //    //ReduceStamina(30);
        //}
        //else
        //{
        am.SetBool("AttackB", false);
        AttackCount = 0;
        CanMove = true;
        //}
    }

    private void Climbing()
    {
        RaycastHit2D RayWall1 = Physics2D.BoxCast(WallCheck1.bounds.center, WallCheck1.bounds.size, 0f, Vector2.zero, 1f, GroundLayer);
        RaycastHit2D RayWall2 = Physics2D.BoxCast(WallCheck2.bounds.center, WallCheck2.bounds.size, 0f, Vector2.zero, 1f, GroundLayer);
        if (RayWall1.collider == null && RayWall2.collider != null)
        {
            CanMove = false;
            am.SetBool("Climb", true);
        }
    }
    private void Climbed()
    {
        float x = 0.2f;    
        float y = 0.5f;
        float distance = WallCheck1.transform.position.x - transform.position.x;
        if (distance >= 0)
        {
            transform.position = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.y);
        }
        else
        {
            transform.position = new Vector3(transform.position.x - x, transform.position.y + y, transform.position.y);
        }

        CanMove = true;
        am.SetBool("Climb", false);
    }


    //Stamina Functions
    private void StaminaControl()
    {
        ImageStaminaBar.fillAmount = Stamina / 100;
        if (Stamina < 100 && CanRestamina)
        {
            Stamina += 0.15f;
        }

        if (Stamina < 0)
        {
            Stamina = 0;
        }
    }
    public void ReduceStamina(float cost)
    {
        Stamina -= cost;
        CanRestamina = false;
        Invoke("IncreaseStamina", 0.5f);
    }
    private void IncreaseStamina()
    {
        CanRestamina = true;
    }


    //Health
    private void HealthControl()
    {

        ImageHealthBar.fillAmount = HealthController.Health / 100;

        if (HealthController.Health < 80 && HealthController.Health > 0)
        {
            HealthController.Health += 0.01f;
        }


        if (HealthController.Health <= 0)
        {
            am.SetBool("Dead", true);
            CanMove = false;
            rb.gravityScale = 0;
        }

    }
}
