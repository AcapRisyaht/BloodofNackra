using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiupanKilat : MonoBehaviour, IBossAttack
{
   public Transform arahHadapan;
   public int damagePerTick = 10;
   public float tickInterval = 0.2f;
   public float totalDuration = 1f;
   public float radius = 2f;
   public float stunDuration = 1.5f;
   public LayerMask playerLayer;

   public void SetArah(Transform arah)
   {
       arahHadapan = arah;
   }

   void Start()
   {
      StartCoroutine(KilatMerebak());
       Destroy(gameObject, totalDuration + 0.2f); 
   }

    IEnumerator KilatMerebak()
    {
        float elapsed = 0f;

        while (elapsed < totalDuration)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, playerLayer);
            Debug.Log("TiupanKilat mengesan " + hits.Length + " sebanyak " + damagePerTick);

            foreach (Collider2D hit in hits)
            {
                PlayerHealth playerHealth = hit.GetComponentInParent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damagePerTick);
                    Debug.Log("Damage kilat kepada: " + hit.name);
                }

                PlayerMovement playerMovement = hit.GetComponentInParent<PlayerMovement>();
                if (playerMovement != null)
                {
                    StartCoroutine(StunPlayer(playerMovement, stunDuration));
                }
            }

            yield return new WaitForSeconds(tickInterval);
            elapsed += tickInterval;
        }
    }

    IEnumerator StunPlayer(PlayerMovement playerMovement, float duration)
    {
        playerMovement.isStunned = true;
        yield return new WaitForSeconds(stunDuration);
        playerMovement.isStunned = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
