using System.Collections.Generic;
using UnityEngine;

public class KuasaPemain : MonoBehaviour  
{
   public List<JenisGeliga> koleksiGeliga = new List<JenisGeliga>();
   public JenisGeliga elemenAktif = JenisGeliga.None;
   public int bonusDamage = 10;
   public RadialMenu radialMenu;
   public GeligaInventory inventory;

    void Awake()
    {
        if (radialMenu == null) radialMenu = FindObjectOfType<RadialMenu>();
        if (inventory == null) inventory = FindObjectOfType<GeligaInventory>();
    }

    void Update()
    {
        // Tekan E untuk aktifkan geliga slot semasa
        if (Input.GetKeyDown(KeyCode.E))
        {
           if (radialMenu == null || inventory == null || inventory.slots == null)
            {
                Debug.LogWarning("RadialMenu / Inventory / slot tiada.");
                return;
            }

            int index = radialMenu.slotAktif;

            // Guard: index sah
            if (index < 0 || index >= inventory.slots.Length)
            {
                Debug.LogWarning($"Index slot {index} di luar julat inventori.");
                return;
            }

            // Guard: slot mesti unlocked (rujuk UI)
            if (!radialMenu.IsSlotUnlocked(index))
            {
                Debug.LogWarning($"Slot {index} belum unlock, tidak boleh aktifkan geliga.");
                return;
            }

            // Guard: slot ada GeligaData
            GeligaData aktif = inventory.slots[index];
            if (aktif == null)
            {
                Debug.LogWarning($"Slot {index} kosong (tiada GeligaData).");
                return;
            }
            
            AktifkanGeliga(aktif);
        }
    }

    public void AktifkanGeliga(GeligaData data)
    {
        if (data == null)
        {
            Debug.LogWarning("AktifkanGeliga dipanggil dengan data null .");
            return;
        }
        
        // Simpan dalam koleksi jika belum ada
        if (!koleksiGeliga.Contains(data.jenis))
        {
            koleksiGeliga.Add(data.jenis);
            Debug.Log($"Pemain dapat geliga {data.jenis}!");

        }

        // Tetapkan elemen aktif    
        elemenAktif = data.jenis;
        Debug.Log($"Elemen aktif sekarang: {elemenAktif}");

        // Jalankan efek khas geliga
        data.Activate(gameObject);
    }

    public int KiraDamage(int asasDamage)
    {
        if ( elemenAktif != JenisGeliga.None && inventory != null && inventory.slots != null)
        {
            foreach (GeligaData g in inventory.slots)
            {
                if (g != null && g.jenis == elemenAktif)
                    return asasDamage + g.bonusDamage;
            }
        }
        
        return asasDamage;
    }

    public void TukarGeliga(JenisGeliga jenis)
    {
        if (koleksiGeliga.Contains(jenis))
        {
            elemenAktif = jenis;
            Debug.Log($"Pemain tukar ke elemen {elemenAktif}");
        }
       
    }
}
