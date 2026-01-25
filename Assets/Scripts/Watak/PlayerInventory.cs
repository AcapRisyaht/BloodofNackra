using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// PlayerInventory — Simpan item, geliga, dan duit player.
/// Guard: Urus tambah/buang item stackable, geliga lore, kawalan duit, dan guna item.
/// </summary>
public class PlayerInventory : MonoBehaviour
{
    public List<ItemSlot> itemSlots = new List<ItemSlot>();
    public List<GeligaSlot> geligaSlots = new List<GeligaSlot>();

    // Guard: Duit player
    public int jumlahSM = 100; // Guard: jumlah syiling Malaya (mula dengan 100)

   // ============================
// Kawalan Item Umum (stackable)
// ============================

// Guard (BM): Tambah item, agih mengikut maxStack dan cipta slot baharu jika perlu
public void AddItem(ItemData item, int jumlah)
{
    if (item == null || jumlah <= 0)
    {
        Debug.LogWarning("Guard: Item null atau jumlah <= 0. Tambah dibatalkan.");
        return;
    }

    int baki = jumlah;

    // Guard: Isi slot sedia ada yang padan hingga penuh
    for (int i = 0; i < itemSlots.Count && baki > 0; i++)
    {
        var slot = itemSlots[i];
        if (slot.item == item && slot.jumlah < item.maxStack)
        {
            int ruang = item.maxStack - slot.jumlah;
            int tambah = Mathf.Min(ruang, baki);
            slot.jumlah += tambah;
            baki -= tambah;
        }
    }

    // Guard: Jika masih ada baki, cipta slot baharu dan isi ikut maxStack
    while (baki > 0)
    {
        int letak = Mathf.Min(item.maxStack, baki);
        ItemSlot newSlot = new ItemSlot { item = item, jumlah = letak };
        itemSlots.Add(newSlot);
        baki -= letak;
    }

    Debug.Log($"Guard: Tambah {jumlah}x {item.namaItem}. Slot kini: {itemSlots.Count}");
}

// Guard (BM): Kira jumlah keseluruhan item merentas semua slot
public int CountItem(ItemData item)
{
    if (item == null)
    {
        Debug.LogWarning("Guard: ItemData null semasa kira jumlah.");
        return 0;
    }

    int total = 0;
    for (int i = 0; i < itemSlots.Count; i++)
    {
        var slot = itemSlots[i];
        if (slot.item == item)
        {
            total += slot.jumlah;
        }
    }
    return total;
}

// Guard (BM): Buang sejumlah item merentas beberapa slot; pulangkan true jika berjaya
public bool RemoveItem(ItemData item, int jumlah)
{
    if (item == null || jumlah <= 0)
    {
        Debug.LogWarning("Guard: Item null atau jumlah <= 0. Buang dibatalkan.");
        return false;
    }

    int tersedia = CountItem(item);
    if (tersedia < jumlah)
    {
        Debug.LogWarning($"Guard: Item {item.namaItem} tak cukup. Ada {tersedia}, perlu {jumlah}.");
        return false;
    }

    int baki = jumlah;

    // Guard: Iterasi dari belakang (selamat remove) dan tolak dari slot-slot berkaitan
    for (int i = itemSlots.Count - 1; i >= 0 && baki > 0; i--)
    {
        var slot = itemSlots[i];
        if (slot.item != item) continue;

        int tolak = Mathf.Min(slot.jumlah, baki);
        slot.jumlah -= tolak;
        baki -= tolak;

        if (slot.jumlah <= 0)
        {
            itemSlots.RemoveAt(i);
        }
    }

    Debug.Log($"Guard: Buang {jumlah}x {item.namaItem}. Baki kini: {CountItem(item)}");
    return true;
}

    public bool HasGeliga(GeligaData geliga)
    {
        foreach (var slot in geligaSlots)
        {
            if (slot.geliga == geliga && slot.unlocked)
            {
                return true;
            }
        }
        return false;
    }

    // ============================
    // Kawalan Duit
    // ============================
    public void TambahSM(int nilai)
    {
        jumlahSM += nilai;
        Debug.Log("Tambah " + nilai + " SM. Jumlah sekarang: " + jumlahSM);
    }
    

    public bool TolakSM(int jumlah)
    {
        if (jumlahSM >= jumlah)
        {
            jumlahSM -= jumlah;
            Debug.Log("Tolak " + jumlah + " SM. Baki sekarang: " + jumlahSM);
            return true;
        }
        else
        {
            Debug.LogWarning("SM tidak cukup! Baki: " + jumlahSM + ", Perlu: " + jumlah);
            return false;
        }
    }

    // ============================
    // Guna Item (contoh: Ubat/Posyen)
    // ============================
    // Guard: Guna PlayerHealth, bukan PlayerStatus
    public void GunaItem(ItemData item, PlayerHealth player)
    {
     if (item == null || player == null) return; // Guard: elak NullReference

     if (item.kategori == ItemKategori.Ubat)
       {
         player.TambahHP(item.jumlahSembuh); // Guard: panggil method yang betul
         Debug.Log(item.namaItem + " digunakan, sembuh " + item.jumlahSembuh + " HP.");
         RemoveItem(item, 1); // Guard: buang 1 selepas guna
       }
    }

    public void pedangBiasa(ItemData pedangBiasa)
    {
        if (pedangBiasa == null)
        {
            Debug.LogWarning("Guard: pedangBiasa adalah null.");
            return;
        }   

        AddItem(pedangBiasa, 1);
    }

    public void pedangDarahKirmizi(ItemData pedangDarahKirmizi)
    {
        if (pedangDarahKirmizi == null)
        {
            Debug.LogWarning("Guard: pedangDarahKirmizi adalah null.");
            return;
        }

        AddItem(pedangDarahKirmizi, 1);
    }
}