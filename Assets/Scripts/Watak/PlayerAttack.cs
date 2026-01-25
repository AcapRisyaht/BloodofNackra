using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 0.5f;
    public float parryDuration = 0.3f;

    public bool isParrying = false;
    public bool isMoving = false; // Tambah ini untuk cek apakah player sedang bergerak

    public Animator animator;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public GameObject attackHitBox; // Untuk aktivkan hitbox
    public Vector2 LastMoveDir = Vector2.right; // Arah hadap terakhir, default ke kanan
    public PlayerInventory PlayerInventory;
    public ItemData pedangBiasa;
    public ItemData pedangDarahKirmizi;

    public int attackDamage = 20;
    public int maxHealth = 100;
    public int currentHealth = 100;

    private PlayerControls playControls;

    void Start()
    {
        if (PlayerInventory != null && pedangBiasa != null)
        {
            PlayerInventory.AddItem(pedangBiasa, 1);
        }
    }

    public void UnlockPedangDarahKirmizi()
    {
        if (PlayerInventory != null && pedangDarahKirmizi != null)
        {
            PlayerInventory.AddItem(pedangDarahKirmizi, 1);
            Debug.Log("Pedang Darah Kirmizi telah dibuka kuncinya dan ditambah ke inventori.");
        }
    }

    void Awake()
    {
        playControls = new PlayerControls();
    }

    void OnEnable()
    {
        playControls.Enable();

        playControls.Combat.Attack.performed += OnAttackFizikal;

        playControls.Combat.OpenWheel.performed += OnOpenWheel;

        playControls.Combat.UseGeliga.performed +=  OnUseGeliga;


        playControls.Movement.Move.performed += OnMovePerformed;
        playControls.Movement.Move.canceled += OnMoveCanceled;

    }

    void OnDisable()
    {
        playControls.Combat.Attack.performed -= OnAttackFizikal;
 
        playControls.Combat.OpenWheel.performed -= OnOpenWheel;

        playControls.Combat.UseGeliga.performed -= OnUseGeliga;


        playControls.Movement.Move.performed -= OnMovePerformed;
        playControls.Movement.Move.canceled -= OnMoveCanceled;

       
        playControls.Disable();
    }

    // Serangan biasa (tetikus kiri)
    private void OnAttackFizikal(InputAction.CallbackContext ctx)
    {
        if (isMoving) return;

        KuasaPemain kuasa = GetComponent<KuasaPemain>();
        if (kuasa == null) return;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        if (hitEnemies == null || hitEnemies.Length == 0) return;

        // Kalau tiada elemen aktif, fallback serangan biasa
        if (kuasa.elemenAktif == JenisGeliga.None) // atau enum default awak
        {
            foreach (Collider2D enemy in hitEnemies)
                SeranganBiasa(enemy);
            return;
        }

        // Serangan elemen
        foreach (Collider2D enemy in hitEnemies)
        {
            Target target = enemy.GetComponentInParent<Target>();
            if (target != null)
            {
                int geligaDamage = kuasa.KiraDamage(attackDamage);
                target.TerimaSerangan(kuasa.elemenAktif, geligaDamage);
                Debug.Log("Serangan elemen: " + kuasa.elemenAktif);
            }
            else
            {
                SeranganBiasa(enemy);
            }
        }
    }

    private void SeranganBiasa(Collider2D enemy = null)
    {
        if (enemy == null) return;

        int attackDamage = PlayerInventory.CountItem(pedangDarahKirmizi) > 0 ?
                           pedangDarahKirmizi.damageSenjata :
                           pedangBiasa.damageSenjata;

        EnemyHealth enemyHealth = enemy.GetComponentInParent<EnemyHealth>();
        if (enemyHealth != null) enemyHealth.TakeDamage(attackDamage);

        BossHealth bossHealth = enemy.GetComponentInParent<BossHealth>();
        if (bossHealth != null) bossHealth.TakeDamage(attackDamage);
    }

    private void SenjataBiasa()
    {
        if (PlayerInventory != null && pedangBiasa != null)
        {
            PlayerInventory.pedangBiasa(pedangBiasa);
            Debug.Log("Pedang Biasa ditambah ke inventori.");
        }
        else
        {
            Debug.LogWarning("Inventori pemain atau data pedang biasa tiada.");
        }
        
    }

    private void SenjataDarahKirmizi()
    {
        if (PlayerInventory != null && pedangDarahKirmizi != null)
        {
            PlayerInventory.pedangDarahKirmizi(pedangDarahKirmizi);
            Debug.Log("Pedang Darah Kirmizi ditambah ke inventori.");
        }
        else
        {
            Debug.LogWarning("Inventori pemain atau data pedang darah kirmizi tiada.");
        }
    }

    // Guna Geliga (E)
    private void OnUseGeliga(InputAction.CallbackContext ctx)
    {
        KuasaPemain kuasa = GetComponent<KuasaPemain>();
        if (kuasa == null || kuasa.radialMenu == null || kuasa.inventory == null || kuasa.inventory.slots == null)
        {
            Debug.LogWarning("Komponen KuasaPemain / radialMenu / inventory / slots tiada.");
            return;
        }

        int index = kuasa.radialMenu.slotAktif;
        if (index < 0 || index >= kuasa.inventory.slots.Length)
        {
            Debug.Log("Index geliga tidak sah. Abaikan penggunaan geliga.");
            return;
        }

        GeligaData dipilih = kuasa.inventory.slots[index];
        if (dipilih == null)
        {
            Debug.Log("Slot geliga kosong. Abaikan penggunaan geliga.");
            return;
        }
        
        kuasa.AktifkanGeliga(dipilih);
    }

    // Buka/tutup roda (Q)
    private void OnOpenWheel(InputAction.CallbackContext ctx)
    {
        KuasaPemain kp = GetComponent<KuasaPemain>();
        if (kp == null || kp.radialMenu == null)
            return;
        
        RadialMenu menu = kp.radialMenu;
        bool newState = !menu.gameObject.activeSelf;
        menu.gameObject.SetActive(newState);
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();
        isMoving = input != Vector2.zero;
        if (isMoving) LastMoveDir = input;
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx) => isMoving = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(ParryWindow());
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
