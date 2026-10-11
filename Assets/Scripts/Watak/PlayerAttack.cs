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
    public Animator pedangAnimator; // Animator untuk pedang
    public GameObject pedangPrefab;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public PlayerInventory PlayerInventory;
    public ItemData pedangBiasa;
    public ItemData pedangDarahKirmizi;
    public Vector2 LastMoveDir = Vector2.zero; // Default arah terakhir adalah diam (0,0)
    public CameraShake cameraShake; // Referensi ke skrip CameraShake 

    public int attackDamage = 20;
    public int maxHealth = 100;
    public int currentHealth = 100;

    private PlayerControls playControls;
    private WeaponActions weapon;

    void Start()
    {
        {
            EquipPedang(pedangPrefab, transform.Find("HandSlotDown"));

            if (PlayerInventory != null && pedangBiasa != null)
            {
                 PlayerInventory.AddItem(pedangBiasa, 1);
            }

             weapon = GetComponentInChildren<WeaponActions>();

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
    // Dalam OnAttackFizikal
    private void OnAttackFizikal(InputAction.CallbackContext ctx)
    {
      // fallback arah default kalau player diam
      if (LastMoveDir == Vector2.zero)
        LastMoveDir = new Vector2(0, -1); // Default arah ke bawah

      // Trigger Player Animator
      animator.SetFloat("AttackX", LastMoveDir.x);
      animator.SetFloat("AttackY", LastMoveDir.y);
      animator.SetTrigger("Attack");

      // Trigger Pedang Animator (serupa dengan toolAnimator sebelum ni)
      if (pedangAnimator != null)
        {
        int x = Mathf.RoundToInt(LastMoveDir.x);
        int y = Mathf.RoundToInt(LastMoveDir.y);

        if (x == 0 && y == -1) pedangAnimator.SetTrigger("doActionDown");
        else if (x == 0 && y == 1) pedangAnimator.SetTrigger("doActionUp");
        else if (x == -1 && y == 0) pedangAnimator.SetTrigger("doActionLeft");
        else if (x == 1 && y == 0) pedangAnimator.SetTrigger("doActionRight");
        else pedangAnimator.ResetTrigger("doActionDown");
        }

        // Damage logic kekal sama
        KuasaPemain kuasa = GetComponent<KuasaPemain>();
        if (kuasa == null) return;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        if (hitEnemies == null || hitEnemies.Length == 0) return;

        if (kuasa.elemenAktif == JenisGeliga.None)
         {
          foreach (Collider2D enemy in hitEnemies)
         {
            Dummy dummy = enemy.GetComponentInParent<Dummy>();
            if (dummy != null)
            {
                dummy.TakeHit(transform);
                continue; // Teruskan ke musuh seterusnya jika ini adalah Dummy
            }

            weapon.DoNormalAttack(enemy);

            // Tambah gegaran untuk serangan biasa juga
            if (cameraShake != null)
            {
                StartCoroutine(cameraShake.Shake(0.1f, 0.1f)); // Contoh gegaran untuk serangan biasa
            }
         }
        return;
        } 

        foreach (Collider2D enemy in hitEnemies)
        {
        Target target = enemy.GetComponentInParent<Target>();
        if (target != null)
          {
            int geligaDamage = kuasa.KiraDamage(attackDamage);
            target.TerimaSerangan(kuasa.elemenAktif, geligaDamage);
            Debug.Log("Serangan elemen: " + kuasa.elemenAktif);
          }
       }
    }

    public void EquipPedang(GameObject pedangPrefab, Transform handSlot)
    {
        foreach (Transform child in handSlot)
        {
            Destroy(child.gameObject);
        }

        GameObject pedangBaru = Instantiate(pedangPrefab, handSlot);
        pedangBaru.transform.localPosition = Vector3.zero;
        pedangBaru.transform.localRotation = Quaternion.identity;

        pedangAnimator = pedangBaru.GetComponent<Animator>();

        if (pedangAnimator != null)
            Debug.Log("PedangAnimator dijumpai: " + pedangAnimator.runtimeAnimatorController.name);
        
        else
            Debug.LogWarning("PedangAnimator belum dijumpai! Pasti prefab ada komponen Animator.");
    
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
        
        if (input != Vector2.zero)
        {
            // Tentukan arah terakhir berdasarkan input
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
                LastMoveDir = new Vector2(Mathf.Sign(input.x), 0); // Horizontal
            
            else
                LastMoveDir = new Vector2(0, Mathf.Sign(input.y)); // Vertical
        }
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        isMoving = false;
    } 
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

    public void EnableHitBox()
    {
        if (weapon != null)
            weapon.EnableHitBox();
    }

    public void DisableHitBox()
    {
        if (weapon != null)
            weapon.DisableHitBox();
    }
}