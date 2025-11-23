using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 2f;
    public Transform Player;
    private Rigidbody2D rb;
    public float attackRange = 5f;
    public float minDistance = 1f;
    public float stopRadius = 0.5f; // Tambah ini untuk radius berhenti
    public float chaseRadius = 5f; // Tambah ini untuk radius pengejaran
    public Vector2 LastMoveDir = Vector2.right; // Arah hadap terakhir, default ke kanan

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float distance = Vector2.Distance(transform.position, Player.position);

        if (distance < chaseRadius && distance > stopRadius)
        {
            Vector2 direction = (Player.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
            LastMoveDir = direction; // Update arah hadap terakhir
        }
        else if (distance <= stopRadius)
        {
            rb.velocity = Vector2.zero;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stopRadius);
    }

}