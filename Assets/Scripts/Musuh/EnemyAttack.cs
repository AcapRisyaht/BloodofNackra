using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyAttack : MonoBehaviour
{
    public EnemyMovement movement; // Rujuk skrip pergerakan
    public Transform Player;
    public float attackCooldown = 1f;
    private bool canAttack = true;
    public float attackRange  = 5f;
    public int attackDamage = 10; // Tambah nilai damage

    void Start()
    {
        if (movement == null)
            Debug.LogError("movement belum disambung!");
    }

    void Update()
    {
        if (movement == null || Player == null)
        {
            Debug.LogWarning("movement atau player belum di sambungkan!");
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, Player.position);

        if (distanceToPlayer <= attackRange && canAttack)
        {
            Debug.DrawLine(transform.position, Player.position, Color.green, 0.1f);

            Vector2 DistanceToPlayer = (Player.position - transform.position).normalized;
            float dot = Vector2.Dot(movement.LastMoveDir.normalized, DistanceToPlayer);

            if (dot > 0.7f) //Player berada di depan
            {
                PlayerAttack playerAttack = Player.GetComponent<PlayerAttack>();
                if (playerAttack != null && playerAttack.isParrying)
                {
                    Debug.Log("Parry aktif!");
                    // Boleh letak kesan stun di sini 
                    return;
                }

                PlayerHealth playerHealth = Player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(attackDamage);
                    Debug.Log("Pemain kena serangan! -" + attackDamage );
                }

                SpriteRenderer sr = Player.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.color = Color.red;
                    StartCoroutine(ResetPlayerColor(sr));
                    Debug.Log("Pemain kena serangan!");
                }

                canAttack = false;
                Invoke(nameof(ResetAttack), attackCooldown);
            }
            else
            {
                Debug.Log("Pemain tidak berada di depan!");
            }
        }
    }

    void ResetAttack()
    {
        canAttack = true;
    }

    IEnumerator ResetPlayerColor(SpriteRenderer sr)
    {
        yield return new WaitForSeconds(0.2f);
        sr.color = Color.white;
    }
}
