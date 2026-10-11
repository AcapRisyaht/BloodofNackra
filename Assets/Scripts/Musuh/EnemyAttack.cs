using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public EnemyMovement movement; // Rujuk skrip pergerakan
    public Transform Player;
    public float attackCooldown = 1f;
    private bool canAttack = true;
    public float attackRange  = 5f;
    public int attackDamage = 10; // Tambah nilai damage
    private Animator animator;
    private bool hasHit;

    void Start()
    {
        animator = GetComponent<Animator>();

       if (movement == null)
        {
            movement = GetComponent<EnemyMovement>();
        }

        if (Player == null)
        {
           GameObject playerObj = GameObject.FindWithTag("Player");
           if (playerObj != null)
               Player = playerObj.transform;
        }
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

                Vector2 dirToPlayer = (Player.position - transform.position).normalized;

                movement.LastMoveDir = dirToPlayer;

                animator.SetFloat("AttackX", dirToPlayer.x);

                animator.SetFloat("AttackY", dirToPlayer.y);

                animator.SetTrigger("Attack");

                canAttack = false;
                hasHit = false; // Reset hasHit sebelum serangan baru
                StartCoroutine(AttackCooldown());
            }
            else
            {
                Debug.Log("Pemain tidak berada di depan!");
            }
        }
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    IEnumerator ResetPlayerColor(SpriteRenderer sr)
    {
        yield return new WaitForSeconds(0.2f);
        sr.color = Color.white;
    }

    public void DoDamage()
    {
        if (Player == null || hasHit) return;
        {
            PlayerHealth playerHealth = Player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
                hasHit = true; // damage sekali sahaja per serangan
            
                //Gegaran kamera bila musuh serang Player
                CameraShake cameraShake = Camera.main.GetComponent<CameraShake>();
                if (cameraShake != null)
                {
                    StartCoroutine(cameraShake.Shake(0.1f, 0.1f));
                }
            }
        }
    }
}
