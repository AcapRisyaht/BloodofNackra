using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TiupanKilat — Serangan boss jenis AoE.
/// Guard: Kawal damage + stun pada player.
/// </summary>
public class TiupanKilat : MonoBehaviour, IBossAttack
{
    public Transform arahHadapan;
    public int damagePerTick = 10;
    public float tickInterval = 0.2f;
    public float totalDuration = 1f;
    public float radius = 2f;
    public float stunDuration = 1.5f;
    public LayerMask playerLayer;

    private bool hasAppliedStun = false;

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

            foreach (Collider2D hit in hits)
            {
                // Guard: Damage kepada player
                PlayerHealth playerHealth = hit.GetComponentInParent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damagePerTick);
                }

                // Guard: Stun player (guna PlayerStatus)
                PlayerStatus playerStatus = hit.GetComponentInParent<PlayerStatus>();
                if (playerStatus != null && !hasAppliedStun)
                {
                    hasAppliedStun = true;
                    playerStatus.ApplyStun(stunDuration);
                }
            }

            yield return new WaitForSeconds(tickInterval);
            elapsed += tickInterval;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}