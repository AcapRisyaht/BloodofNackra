using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DataSeranganIstimewa — Simpan nama + prefab serangan boss.
/// Guard: Hanya data, tiada logik.
/// </summary>
[System.Serializable]
public class DataSeranganIstimewa
{
    public string nama;
    public GameObject prefab;
}

/// <summary>
/// BossAttack — Kawal serangan istimewa boss.
/// Guard: Hanya logik serangan, bukan status player.
/// </summary>
public class BossAttack : MonoBehaviour
{
    public List<DataSeranganIstimewa> senaraiSeranganIstimewa;

    public string namaSeranganAktif = "CakarRuntuh";
    public Transform player;
    public Transform spawnPoint;
    public Transform arahHadapan;

    public float attackRange = 2f;
    public float cooldown = 4f; // Guard: panjangkan cooldown supaya bos tak spam

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
        PlayerStatus ps = player.GetComponent<PlayerStatus>();
        if (ps != null) ps.ApplyStun(0.5f); // Guard: stun lebih singkat

        yield return new WaitForSeconds(0.6f);

        int jumlahSerangan = 2; // Guard: kurang serangan berturut-turut
        float selaAntaraCakar = 1.0f; // Guard: lebih masa antara serangan

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
            }

            yield return new WaitForSeconds(selaAntaraCakar);
        }

        sedangMenyerang = false; // Guard: reset supaya bos tunggu cooldown sebelum serangan seterusnya
    }
}