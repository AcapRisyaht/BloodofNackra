using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;


[System.Serializable]
public class DataSeranganIstimewa
{
    public string nama;
    public GameObject prefab;
}

public class BossAttack : MonoBehaviour
{
    public List<DataSeranganIstimewa> senaraiSeranganIstimewa;
    public string namaSeranganAktif = "CakarRuntuh";
   
    public Transform player;
    public Transform spawnPoint;
    public Transform arahHadapan;

    public float attackRange = 2f;
    public float cooldown = 2f;
    
    private float lastAttackTime;
    private bool sedangMenyerang = false;

    void Update()
    {
        if (player == null || sedangMenyerang) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= attackRange && Time.time >= lastAttackTime + cooldown)
        {
            StartCoroutine(SeranganBerselang());
            lastAttackTime = Time.time;
            sedangMenyerang = true;
        }
    }

    IEnumerator SeranganBerselang()
    {
        // Bekukan player
        PlayerMovement pc = player.GetComponent<PlayerMovement>();
        if (pc != null)
        {
            pc.isStunned = true;
            Debug.Log("Player dibekukan oleh Cakar Bertubi-tubi");
        }

        yield return new WaitForSeconds(0.6f);

        int jumlahSerangan = 3;
        float selaAntaraCakar = 0.5f;


        for (int i = 0; i < jumlahSerangan; i++)
        {
            GameObject prefabSerangan = senaraiSeranganIstimewa.Find(s => s.nama == namaSeranganAktif)?.prefab;

            if (prefabSerangan != null && spawnPoint != null)
            {
               GameObject serangan = Instantiate(prefabSerangan, spawnPoint.position, Quaternion.identity);

               IBossAttack skrip = serangan.GetComponent<IBossAttack>();
               if (skrip != null && arahHadapan != null)
               {
                   skrip.SetArah(arahHadapan);
               }

               Debug.Log("Boss mula serangan:" + namaSeranganAktif);
            }

            yield return new WaitForSeconds(selaAntaraCakar);
        }

        if (pc != null)
        {
            pc.isStunned = false;
            Debug.Log("Player dibebaskan dari bekuan Cakar Bertubi-tubi");
        }

    }
}
