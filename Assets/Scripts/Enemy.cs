using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class Enemy : Character
{
    [SerializeField] private float attackRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject attackArea;
    
    private IState currentState;

    private bool isRight = true;

    private Character target;
    public Character Target => target;

    private void Update()
    {
        if (currentState != null && !isDead)
        {
            currentState.OnExecute(this);
        }
    }
    // Start is called before the first frame update
    public override void OnInit()
    {
        base.OnInit();

        ChangeState(new IdleState());
        DeActiveAttack();
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        Destroy(healthBar.gameObject);
        Destroy(gameObject);
    }

    protected override void Die()
    {
        ChangeState(null);
        base.Die();
    }

    public void Moving()
    {
        ChangeAnimation("run");
        //rb.velocity = transform.right * moveSpeed * Time.deltaTime;
        rb.velocity = isRight ? new Vector2(moveSpeed * Time.deltaTime, rb.velocity.y) : new Vector2(-moveSpeed * Time.deltaTime, rb.velocity.y);
    }

    public void StopMoving()
    {
        ChangeAnimation("idle");
        rb.velocity = new Vector2(0, rb.velocity.y);

    }

    internal void SetTarget(Character character)
    {
        this.target = character;
        if (TargetInRange())
        {
            ChangeState(new AttackState());
        }
        else if (Target != null)
        {
            ChangeState(new PatrolState());
        }
        else
        {
            ChangeState(new IdleState());
        }
    }

    public void Attack()
    {
        ChangeAnimation("attack");
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.5f);
    }

    public bool TargetInRange()
    {
        if (target != null && Vector2.Distance(target.transform.position, transform.position) <= attackRange)
        {
            return true;
        }
        return false;
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }
        
        Debug.LogError(currentState + "___" + newState);

        currentState = newState;

        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyWall")
        {
            ChangeDirection(!isRight);
        }
    }

    public void ChangeDirection(bool isRight)
    {
        this.isRight = isRight;
        transform.rotation = isRight ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(Vector3.up * 180);
    }
    
    private void ActiveAttack()
    {
        attackArea.SetActive(true);
    }

    private void DeActiveAttack()
    {
        attackArea.SetActive(false);
    }
}
