using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private float _Move,_Scale;
    private bool _CanMove = true;
    private bool _CanAttack = false;
    private float _LastAttack = 0;
    private float _FireBallDirection;
    [SerializeField]
    private GameObject FireBallRight;
    [SerializeField]
    private GameObject FireBallLeft;
    [SerializeField]
    private GameObject AttackPoint;

    //Detect player
    [SerializeField]
    private Transform PlayerPosition;
    [SerializeField]
    private CircleCollider2D PlayerCheckCollider;
    [SerializeField]
    private LayerMask PlayerLayer;

    private Animator _Animator;
    void Start()
    {
        _Scale = transform.localScale.x;
        _Animator = GetComponent<Animator>();
    }

    void Update()
    {
        PlayerDetect();
        Movement();
        Attack();
    }

    private void PlayerDetect()
    {
        RaycastHit2D CircleCenter = Physics2D.CircleCast(PlayerCheckCollider.bounds.center, PlayerCheckCollider.radius, Vector2.down, 1f, PlayerLayer);
        if (CircleCenter.collider != null)
        {
            _CanAttack = true;
        }
        else
        {
            _CanAttack = false;
        }

        if (_CanAttack)
        {
            if (PlayerPosition.position.x < transform.position.x)
            {
                _Move = -1;
            }
            else
            {
                _Move = 1;
            }
        }
        else
        {
            _Move = 0;
        }
    }
    private void Movement()
    {
        if (_CanMove)
        {
            if (_Move > 0) 
            {
                transform.localScale = new Vector3(_Scale, transform.localScale.y, transform.localScale.z);
            }
            else if (_Move < 0)
            {
                transform.localScale = new Vector3(-_Scale, transform.localScale.y, transform.localScale.z);
            }
        }
    }
    private void Attack()
    {
 
        if (_CanAttack && _LastAttack + 3f < + Time.time && PlayerPosition.position.y < 0.6)
        {
            _CanMove = false;
            _Animator.SetBool("AttackA", true);
            _LastAttack = Time.time;
            _FireBallDirection = transform.position.x - PlayerPosition.position.x;
        }

        if (_Animator.GetCurrentAnimatorStateInfo(0).IsName("DeadwoodAttackA"))
        {
            _Animator.SetBool("AttackA", false);
        }
    }


   

    //Animator
    private void FireBall()
    {
        if (_FireBallDirection >= 0)
        {
            Instantiate(FireBallLeft, AttackPoint.transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(FireBallRight, AttackPoint.transform.position, Quaternion.identity);
        }
    }
    private void Attacked()
    {
        _CanMove = true;
        _CanAttack = false;
    }


}
