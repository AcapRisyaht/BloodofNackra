using UnityEngine;

/// <summary>
/// ItemData — Data asas untuk item.
/// Guard: Simpan nama, ikon, dan deskripsi.
/// </summary>

public enum ItemRank { biasa, jarang, Epik, Legenda } 

[CreateAssetMenu(menuName = "Item/Umum")]
public class ItemData : ScriptableObject
{
    [Header("Maklumat Asas Item")]
    // Guard: Nama item
    public string namaItem;
    // Guard: Ikon item (Sprite untuk UI)
    public Sprite icon;
    // Guard: Deskripsi item
    [TextArea] public string deskripsi;

    [Header("Stack & Rank")]
    // Guard: Stack maksimum
    public int maxStack = 99;
    // Guard: Peringkat item
    public ItemRank rank = ItemRank.biasa; // Guard: Default 'biasa'

    [Header("Kategori")]
    // Guard: Kategori item
    public ItemKategori kategori = ItemKategori.Bahan; // Guard: Default 'peralatan'

    [Header("Stat Khas")]
    // Guard: Nilai penyembuhan
    public int jumlahSembuh = 0;
    public int damageSenjata = 0;
}

public enum ItemKategori
{
    Bahan,
    peralatan,
    Senjata,
    Ubat,
    Alatan
    
}