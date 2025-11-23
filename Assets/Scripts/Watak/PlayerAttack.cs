using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public bool isParrying = false;
    public float parryDuration = 0.3f;
    private PlayerControls playControls;
    public GameObject attackHitBox; // Untuk aktivkan hitbox
    public bool isMoving = false; // Tambah ini untuk cek apakah player sedang bergerak
    public Vector2 LastMoveDir = Vector2.right; // Arah hadap terakhir, default ke kanan
    public int attackDamage = 20;
    public BossZoneTrigger bossZoneTrigger;
    public int maxHealth = 100;
    public int currentHealth = 100;

    void Awake()
    {
        playControls = new PlayerControls();
    }

    void OnEnable()
    {
        playControls.Enable();
        playControls.Combat.Attack.performed += OnAttack;

        playControls.Movement.Move.performed += ctx =>
        {
            Vector2 input = ctx.ReadValue<Vector2>();
            isMoving = input != Vector2.zero;
            if (isMoving)

                LastMoveDir = input;

        };

        playControls.Movement.Move.canceled += ctx => isMoving = false;
    }

    void OnDisable()
    {
        playControls.Combat.Attack.performed -= OnAttack;

        playControls.Movement.Move.performed -= ctx => isMoving = ctx.ReadValue<Vector2>() != Vector2.zero;
        playControls.Movement.Move.canceled -= ctx => isMoving = false;

        playControls.Disable();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(ParryWindow());
        }
    }

    private void OnAttack(InputAction.CallbackContext ctx)
    {
        if (isMoving)
        {
            Debug.Log("Can't attack while moving");
            return;
        }
        Debug.Log("Player Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Ambil arah hadap pemain
        Vector2 facingDir = transform.right; // Assuming right is the facing direction
        if (facingDir == Vector2.zero)
            facingDir = Vector2.right; // Default facing direction

        HashSet<GameObject> processedEnemies = new HashSet<GameObject>();

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Collider dijumpai: " + enemy.name);


            GameObject enemyObj = enemy.GetComponentInParent<EnemyHealth>()?.gameObject ?? enemy.gameObject;



            if (processedEnemies.Contains(enemyObj))
                continue;

            processedEnemies.Add(enemyObj);

            Vector2 enemyDir = ((Vector2)enemy.transform.position - (Vector2)transform.position).normalized;
            float dot = Vector2.Dot(LastMoveDir.normalized, enemyDir);

            if (dot > 0.7) // Musuh berada di depan pemain
            {
                EnemyHealth enemyHealth = enemyObj.GetComponent<EnemyHealth>();
                
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(attackDamage);
                }

                BossHealth bossHealth = enemy.GetComponentInParent<BossHealth>();
                if (bossHealth != null)
                {
                    Debug.Log("BossHealth dijumpai" + bossHealth.name);
                    bossHealth.TakeDamage(attackDamage);
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        ShowFloatingText(damage); 

        if (currentHealth <= 0)
            Die();
    }   

    private void Die()
    {
        // Implement player death logic here (e.g., play animation, disable controls, etc.)
        Debug.Log("Player has died.");
        // Example: Destroy(gameObject);
    }

    // Dummy implementation for ShowFloatingText to prevent compile error
    private void ShowFloatingText(int damage)
    {
        // You can implement floating text display here if needed
        Debug.Log($"Damage taken: {damage}");
    }

    void DisableHitBox()
    {
        attackHitBox.SetActive(false);
    }

    private IEnumerator ResetColor(SpriteRenderer enemySprite)
    {
        yield return new WaitForSeconds(0.2f);

        if (enemySprite != null && enemySprite.gameObject != null)
        {
            enemySprite.color = Color.white;
        }
        
    }

    IEnumerator ParryWindow()
    {
        isParrying = true;
        Debug.Log("Parry aktif");
        yield return new WaitForSeconds(parryDuration);
        isParrying = false;
        Debug.Log("Parry tamat");
    }
    
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
