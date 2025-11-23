using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GigitanRacun : MonoBehaviour, IBossAttack
{
    public Transform arahHadapan;
    public void SetArah(Transform arah)
    {
        arahHadapan = arah;
    }

    public int damageAwal = 10;
    public int damageRacun = 5;
    public int bilanganTrick = 3;
    public float selaAnraraTrick = 2.0f;
    public float radius = 3.0f;
    public LayerMask playerLayer;
    
    void Start()
    {
        Invoke("Serang", 0.4f);
    }

    void Serang()
    {
        if (arahHadapan == null)
        {
            Debug.LogError("arahHadapan tidak disambungkan ke GigitanRacun!");
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(arahHadapan.position, radius, playerLayer);
        Debug.Log("Jumlah Collider dikesan oleh GigitanRacun: " + hits.Length);

        foreach (Collider2D hit in hits)
        {
            PlayerHealth player = hit.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damageAwal);
                Debug.Log("Player kena gigitan racun! Damage awal: " + damageAwal);
                StartCoroutine(KesanRacun(player));
            }

            Rigidbody2D rb = hit.GetComponentInParent<Rigidbody2D>();
            if (rb != null && arahHadapan != null)
            {
                Vector2 arahTolakan = (rb.position - (Vector2)arahHadapan.position).normalized;
                rb.AddForce(arahTolakan * 0.5f, ForceMode2D.Impulse);
                Debug.Log("Player ditolak oleh gigitan racun!");
            }
        }

         Destroy(gameObject, 0.5f);
    }
     
    IEnumerator KesanRacun(PlayerHealth player)
    {
        for (int i = 0; i < bilanganTrick; i++)
        {
            yield return new WaitForSeconds(selaAnraraTrick);
            player.TakeDamage(damageRacun);
            Debug.Log("Player kena kesan racun! Damage racun: " + damageRacun);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (arahHadapan != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(arahHadapan.position, radius);
        }
    }   
}
