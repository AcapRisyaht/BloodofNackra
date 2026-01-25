using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// InventoryUI — Skrip modular untuk kawal UI beg.
/// Guard: Skrip ini hanya urus paparan UI, bukan logik inventori.
/// </summary>
public class InventoryUI : MonoBehaviour
{
    // Guard: Rujukan ke PlayerInventory (data item & geliga)
    public PlayerInventory playerInventory;

    // Guard: Panel beg (Canvas/Panel yang akan dihidupkan/mati)
    public GameObject bagPanel;

    // Guard: Parent untuk slot item (contoh GridLayoutGroup)
    public Transform itemGridParent;

    // Guard: Prefab UI slot item (ikon + jumlah)
    public GameObject itemSlotPrefab;

    void Update()
    {
        // Guard: Tekan B untuk toggle beg
        if (Input.GetKeyDown(KeyCode.I))
        {
            bool isActive = bagPanel.activeSelf;
            bagPanel.SetActive(!isActive);

            if (!isActive)
            {
                RefreshUI();
            }
        }
    }

    // Guard: Fungsi untuk refresh UI beg
    public void RefreshUI()
    {
        // Kosongkan slot lama
        foreach (Transform child in itemGridParent)
        {
            Destroy(child.gameObject);
        }

        // Loop semua item dalam PlayerInventory
        foreach (var slot in playerInventory.itemSlots)
        {
            var uiSlot = Instantiate(itemSlotPrefab, itemGridParent);
            // Guard: Pastikan prefab ada skrip ItemSlotUI
            uiSlot.GetComponent<ItemSlotUI>().Setup(slot);
        }
    }
}