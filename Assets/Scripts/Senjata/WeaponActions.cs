using UnityEngine;

public class WeaponActions : MonoBehaviour
{
    public PedangHitBox pedangHitBox;        // Komponen hitbox pedang
    public PlayerInventory playerInventory;  // Referensi ke PlayerInventory
    public ItemData pedangBiasa;             // Data pedang biasa
    public ItemData pedangDarahKirmizi;      // Data pedang darah kirmizi

    private int currentDamage = 20;          // Default damage


    // Tentukan damage ikut inventory
    private void UpdateWeaponDamage()
    {
        if (playerInventory != null)
        {
            if (playerInventory.CountItem(pedangDarahKirmizi) > 0)
                currentDamage = pedangDarahKirmizi.damageSenjata;
            else
                currentDamage = pedangBiasa.damageSenjata;
        }

        if (pedangHitBox != null)
            pedangHitBox.weaponDamage = currentDamage;
    }

    // Dipanggil oleh Animation Event dalam clip pedang
    public void EnableHitBox()
    {
        if (pedangHitBox != null)
            pedangHitBox.EnableHitBox();
    }

    public void DisableHitBox()
    {
        if (pedangHitBox != null)
            pedangHitBox.DisableHitBox();
    }

    // Fungsi untuk serangan normal (direct damage tanpa animasi)
    public void DoNormalAttack(Collider2D enemy)
    {
        if (enemy == null) return;

        IDamageable damageable = enemy.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(currentDamage);
            Debug.Log($"Dealt {currentDamage} damage to {enemy.name}");
        }
    }
    
    public void DoElementalAttack(Transform attackPoint, float attackRange, LayerMask enemyLayers, JenisGeliga elemenAktif, int baseDamage)
    {
        KuasaPemain kuasa = GetComponent<KuasaPemain>();
        if (kuasa == null) return;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        if (hitEnemies == null || hitEnemies.Length == 0) return;

        if (kuasa.elemenAktif == JenisGeliga.None)
        {
            foreach (Collider2D enemy in hitEnemies)
            {
                DoNormalAttack(enemy);
            }
            return;
        }

        foreach (Collider2D enemy in hitEnemies)
        {
            Target target = enemy.GetComponentInParent<Target>();
            if (target != null)
            {
                int geligaDamage = kuasa.KiraDamage(baseDamage);
                target.TerimaSerangan(kuasa.elemenAktif, geligaDamage);
                Debug.Log("Serangan elemen: " + kuasa.elemenAktif);
            }
        }
    }
}
