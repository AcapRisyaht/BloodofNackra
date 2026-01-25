using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossZoneTrigger : MonoBehaviour
{

   private BossHealth bossHealth;

    void Start()
    {
        GameObject bossObj = GameObject.FindGameObjectWithTag("Boss");
        if (bossObj != null)
        {
            bossHealth = bossObj.GetComponent<BossHealth>();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && bossHealth != null && !bossHealth.isDead)
        {
            bossHealth.TunjukUI(true);
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && bossHealth != null)
        {
            bossHealth.TunjukUI(false);
        }
    }
}
