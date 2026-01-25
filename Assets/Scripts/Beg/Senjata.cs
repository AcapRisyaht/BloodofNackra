using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Senjata : MonoBehaviour
{
    public ItemData dataSenjata;
    public int damage = 30;

    public void Serang(GameObject target)
    {
       EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();
       if (enemyHealth != null)
       {
            EnemyHealth enemy = target.GetComponentInParent<EnemyHealth>();
        if (enemyHealth != null) enemyHealth.TakeDamage(damage);
       }

       BossHealth bossHealth = target.GetComponent<BossHealth>();
       if (bossHealth != null)
       {
            BossHealth boss = target.GetComponentInParent<BossHealth>();
        if (bossHealth != null) bossHealth.TakeDamage(damage);
       }
    }

}
