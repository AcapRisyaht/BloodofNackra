using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public JenisGeliga elemen;
    public BossHealth bossHealth; // Referensi ke BossHealth jika target ini adalah boss

    public void TerimaSerangan(JenisGeliga elemenPerangan, int damage)
    {
       if (bossHealth != null && !bossHealth.isDead)
       {
           bossHealth.currentHealth -= damage;
           bossHealth.currentHealth = Mathf.Clamp(bossHealth.currentHealth, 0, bossHealth.maxHealth);

           if (ElemenHelper.SeranganEfektif(elemenPerangan, elemen) || bossHealth.currentHealth <= 0)
           {
               Debug.Log($"{name} tewas oleh {elemenPerangan}!");
               bossHealth.Die();
           }
           else
           {
               Debug.Log($"{name} menerima {damage} kerosakan dari {elemenPerangan}.");
           }
       }
       else if (bossHealth == null)
        {
            Kalah();
        }
    }

    // Bila musuh tewas
    void Kalah()
    {
        Destroy(gameObject);
    }
}
