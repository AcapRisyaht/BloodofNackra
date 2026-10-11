using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 2f;
    public Transform Player;
    
    public float minDistance = 1f;
    public float stopRadius = 0.5f; // Tambah ini untuk radius berhenti
    public float chaseRadius = 5f; // Tambah ini untuk radius pengejaran
    public Vector2 LastMoveDir = Vector2.right; // Arah hadap terakhir, default ke kanan
    public float patroSpeed = 1.5f;
    public float patrolChangeTime = 3f;
    public float patrolRadius = 5f;
    public bool isKnockbacked = false;
    public float KnockbackTimer = 0f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 spawnPoint;
    private Vector2 patrolDir;
    private float patrolTimer;
    

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        spawnPoint = transform.position;
        ChooseRandomDirection();
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (animator == null)
            Debug.LogWarning($"{name} tiada Animator");

        else
           Debug.Log($"{name} berjaya jumpa Animator: {animator.name}");
    }

    private void UpdateAnimator(Vector2 direction, bool isMoving)
    {
        animator.SetBool("IsMoving", isMoving);
        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);

        if (isMoving)
        {
           animator.SetFloat("LastMoveX", direction.x);
           animator.SetFloat("LastMoveY", direction.y);
        }
        
    }

    private void FixedUpdate()
    {
        if (isKnockbacked)
        {
            KnockbackTimer -= Time.fixedDeltaTime;
            if (KnockbackTimer <= 0f)
            {
                isKnockbacked = false;
                rb.velocity = Vector2.zero; // Hentikan selepas knockback
            }
            return; // Jangan override velocity semasa knockback
        }

        float distance = Vector2.Distance(transform.position, Player.position);

        if (distance < chaseRadius && distance > stopRadius)
        {
            Vector2 direction = (Player.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
            LastMoveDir = direction; // Update arah hadap terakhir

            UpdateAnimator(direction, true);
        }
        else if (distance <= stopRadius)
        {
            rb.velocity = Vector2.zero;
            UpdateAnimator(LastMoveDir, false);
        }
        else
        {
            patrolTimer -= Time.deltaTime;
            if (patrolTimer <= 0)
            {
                ChooseRandomDirection();
            }

            Vector2 nextPos = (Vector2)transform.position + patrolDir * patroSpeed * Time.fixedDeltaTime;
            if (Vector2.Distance(nextPos, spawnPoint) > patrolRadius)
            {
                patrolDir = (spawnPoint - (Vector2)transform.position).normalized;
            }
            
            rb.velocity = patrolDir * patroSpeed;
            LastMoveDir = patrolDir; // Update arah hadap terakhir  
            UpdateAnimator(patrolDir, true);
        }
    }

    void ChooseRandomDirection()
    {
       int rand = Random.Range(0, 4);
       switch (rand)
        {
            case 0: patrolDir = Vector2.up; break;
            case 1: patrolDir = Vector2.down; break;
            case 2: patrolDir = Vector2.left; break;
            case 3: patrolDir = Vector2.right; break;
        }
        patrolTimer = patrolChangeTime;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stopRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(spawnPoint, patrolRadius);
    }

}