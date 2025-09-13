using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public EnemyState state;
    [SerializeField] private Animator animator;
    
    [Space]

    [Header("Movement")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed = 3f;
    public Transform player;
    private int facingDirection = -1;

    [Space]

    [Header("Attack")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldownTime = 2f;
    private float attackCooldownTimer;

    [Space]

    [Header("Detection")]
    [SerializeField] private Transform detectionPoint;
    [SerializeField] private float playerDetectRange = 5f;
    [SerializeField] private LayerMask playerLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        if(player == null)
        {
            Transform Player = GameObject.FindWithTag("Player").transform;
            if(Player != null)
            {
                player = Player;
            }
            else
            {
                Debug.Log("Can't find player");
            }
        }

        ChangeState(EnemyState.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        CheckForPlayer();
        if(attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }

        if (state == EnemyState.Chasing)
        {
            Chase();
        }
        else if(state == EnemyState.Attacking)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void Chase()
    {
        if (player.position.x > transform.position.x && facingDirection == -1
                || player.position.x < transform.position.x && facingDirection == 1)
        {
            Flip();
        }
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
        Debug.Log("Enemy is chasing");
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(facingDirection, transform.localScale.y, transform.localScale.z);
    }

    public void Attack()
    {
        Debug.Log("Enemy attack player");
    }

    private void CheckForPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, playerDetectRange, playerLayer);

        if(hits.Length > 0)
        {
            if (Vector2.Distance(transform.position, player.position) <= attackRange && attackCooldownTimer <= 0)
            {
                attackCooldownTimer = attackCooldownTime;
                ChangeState(EnemyState.Attacking);
            }
            else if (Vector2.Distance(transform.position, player.position) > attackRange)
            {
                ChangeState(EnemyState.Chasing);
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            ChangeState(EnemyState.Idle);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            ChangeState(EnemyState.Idle);
        }
    }

    private void ChangeState(EnemyState nextState)
    {
        if (state == EnemyState.Idle)
            animator.SetBool("isIdle", false);
        else if (state == EnemyState.Chasing)
            animator.SetBool("isChasing", false);
        else if (state == EnemyState.Attacking)
            animator.SetBool("isAttacking", false);

        state = nextState;

        if (state == EnemyState.Idle)
            animator.SetBool("isIdle", true);
        else if (state == EnemyState.Chasing)
            animator.SetBool("isChasing", true);
        else if (state == EnemyState.Attacking)
            animator.SetBool("isAttacking", true);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectionPoint.position, playerDetectRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

public enum EnemyState { Idle, Chasing, Attacking, Hit, Dead }
