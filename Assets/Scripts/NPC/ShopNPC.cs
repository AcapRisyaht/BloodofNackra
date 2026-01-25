using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// ShopNPC — NPC jual banyak item.
/// Guard: Senarai item + harga, bukan UI.
/// </summary>
public class ShopNPC : MonoBehaviour
{
    public PlayerInventory playerInventory;
    public GameObject kedaiPanel;
    public GameObject itemButtonPrefab;
    public Transform itemSlotParent;

    [System.Serializable]
    public class ItemJual
    {
        public ItemData item;   // Guard: Data item (contoh Penawar)
        public int harga;       // Guard: Harga item
    }

    // Guard: Senarai item yang dijual NPC
    public List<ItemJual> senaraiItem = new List<ItemJual>();

    /// <summary>
    /// Beli item dari NPC berdasarkan nama item.
    /// Guard: Duit ditolak dari PlayerInventory, item ditambah jika cukup duit.
    /// </summary>
    
    void Start()
    {
        DialogManager.Instance.OnDialogSelesai += BukakedaiPanel;
    }

    void BukakedaiPanel()
    {
        Debug.Log("Dialog selesai, buka panel kedai.");
        kedaiPanel.SetActive(true);

        // Kosongkan slot lama
        foreach (Transform child in itemSlotParent)
        {
            Destroy(child.gameObject);
        }
        
        // Buat butang untuk setiap item yang senarai
        foreach (ItemJual dijual in senaraiItem)
        {
            GameObject butang = Instantiate(itemButtonPrefab, itemSlotParent);
            
            // Cari komponen dalam prefab
            Image ikon = butang.transform.Find("Icon").GetComponent<Image>();
            TMPro.TextMeshProUGUI teksNama = butang.transform.Find("Nama").GetComponent<TMPro.TextMeshProUGUI>();
            TMPro.TextMeshProUGUI teksHarga = butang.transform.Find("Harga").GetComponent<TMPro.TextMeshProUGUI>();
            
            // isi data
            if (ikon != null) ikon.sprite = dijual.item.icon;
            if (teksNama != null) teksNama.text = dijual.item.namaItem;
            if (teksHarga != null) teksHarga.text = " RM" + dijual.harga.ToString();

            // Tambah fungsi beli bila klik
            string namaItem = dijual.item.namaItem; // Simpan dalam variabel lokal untuk lambda
            butang.GetComponent<Button>().onClick.AddListener(() => BeliItem(playerInventory, namaItem));
        }
                

    }
    
    public void BeliItem(PlayerInventory playerInventory, string namaItem)
    {
        ItemJual dijual = senaraiItem.Find(i => i.item.namaItem == namaItem);
        if (dijual != null)
        {
            if (playerInventory.TolakSM(dijual.harga))
            {
                playerInventory.AddItem(dijual.item, 1);
                Debug.Log("Player beli " + dijual.item.namaItem);
            }
            else
            {
                Debug.Log("SM tak cukup untuk beli " + dijual.item.namaItem);
            }
        }
        else
        {
            Debug.Log("Item " + namaItem + " tidak dijual di kedai ini.");
        }
    }

    void Update()
    {
        // Tekan F untuk berinteraksi dengan NPC
        if (Input.GetKeyDown(KeyCode.F))
        {
            string[] lines = new string[]
            {
                "Selamat datang ke kedai saya!",
                "Mata wang rasmi: Syiling Malaya (SM).",
                "Apa yang anda ingin beli?"
            };
            DialogManager.Instance.ShowDialog(lines);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            kedaiPanel.SetActive(false);
        }
    }

    public void OnClickBeliPosyen()
        {
            BeliItem(playerInventory, "Posyen Kesihatan");
        }

    public void OnClickBeliPenawar()
        {
            BeliItem(playerInventory, "Penawar Bumi");
        }
}