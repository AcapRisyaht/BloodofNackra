using UnityEngine;

public class EnemyHitBox : MonoBehaviour
{
    public int damage = 10; // Damage tangan kosong
    public Collider2D weaponCollider;
    private bool hasHit;

    void Awake()
    {
      if (weaponCollider != null) weaponCollider.enabled = false; // Pastikan hitbox dimatikan pada awalnya
    }

    public void EnableHitBox()
    { 
       weaponCollider.enabled = true;
       hasHit = false;
    }

    public void DisableHitBox()
    {
        weaponCollider.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
       if (!hasHit && other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                hasHit = true; // damage sekali sahaja per serangan
                Debug.Log("Player terkena damage: " + damage);
            }
        }
    }
}