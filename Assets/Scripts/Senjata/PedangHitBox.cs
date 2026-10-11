using UnityEngine;

public class PedangHitBox : MonoBehaviour
{
    public GameObject player; 
    public WeaponActions weapon;
    private Collider2D swordCollider;
    public int weaponDamage = 20; // Default damage value

    void Awake()
    {
        swordCollider = GetComponent<Collider2D>();
        if (swordCollider != null)
            swordCollider.enabled = false; // OFF masa mula

        weapon = GetComponentInParent<WeaponActions>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(weaponDamage);               
        }

        Target target = other.GetComponentInParent<Target>();
        if (target == null) return;

        KuasaPemain kuasa = player.GetComponent<KuasaPemain>();
        if (kuasa != null && kuasa.elemenAktif != JenisGeliga.None)
        {
            int geligaDamage = kuasa.KiraDamage(weaponDamage);
            target.TerimaSerangan(kuasa.elemenAktif, geligaDamage);
        }
        else
        {
            weapon.DoNormalAttack(other);
        }            
    }

    public void EnableHitBox()
    {
        if (swordCollider != null)
            swordCollider.enabled = true;
    }

    public void DisableHitBox()
    {
        if (swordCollider != null)
            swordCollider.enabled = false;
    }
}
