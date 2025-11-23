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
            Debug.Log("Boss dijumpai oleh zon: " + bossObj.name);
        }
        else
        {
            Debug.LogWarning("Boss TIDAK dijumpai oleh zon!!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Player masuk zon  boss");
        if (other.CompareTag("Player") && bossHealth != null && !bossHealth.isDead)
        {
            Debug.Log("Panggilan TunjukUI(true)");
            bossHealth.TunjukUI(true);
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Player keluar zon boss" + other.name);
        if (other.CompareTag("Player") && bossHealth != null)
        {
            bossHealth.TunjukUI(false);
        }
    }
}
