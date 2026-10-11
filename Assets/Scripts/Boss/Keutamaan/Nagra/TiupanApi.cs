using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiupanApi : MonoBehaviour, IBossAttack
{
    public Transform arahaHadapan;
    public int damagePerTick = 5;
    public float tickInterval = 0.3f;
    public float totalDuration = 2f;
    public float radius = 3f;
    public LayerMask playerLayer;

    public void SetArah(Transform arah)
    {
        arahaHadapan = arah;
    }

    void Start()
    {
        StartCoroutine(HembusApi());
        Destroy(gameObject, totalDuration + 0.1f);
    }

    IEnumerator HembusApi()
    {
        float elapsed = 0f;

        while (elapsed < totalDuration)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, playerLayer);

            foreach (Collider2D hit in hits )
            {
                PlayerHealth playerHealth = hit.GetComponentInParent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damagePerTick);
                }
            }

            yield return new WaitForSeconds(tickInterval);
            elapsed += tickInterval;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
