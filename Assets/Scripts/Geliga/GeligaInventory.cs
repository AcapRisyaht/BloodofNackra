using UnityEngine;

public class GeligaInventory : MonoBehaviour
{
    public GeligaData[] slots = new GeligaData[7];
    public int selectedSlot = -1;

    // Tambah geliga ke slot kosong
    public void AddGeliga(GeligaData geliga)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = geliga;
                Debug.Log("Geliga ditambah ke slot " + i);
                return;
            }
        }
        Debug.Log("Semua slot penuh!");
    }

    // Pilih slot aktif
    public void SetActiveSlot(int index)
    {
        if (index >= 0 && index < slots.Length && slots[index] != null)
        {
            selectedSlot = index;
            Debug.Log("Slot aktif: " + index + " => " + slots[index].namaGeliga);
        }
        else
        {
            Debug.Log("Slot kosong atau index tidak sah.");
        }
    }

    //Guna geliga dari slot aktif (E)
    public void UseSelectedGeliga(GameObject player)
    {
        if (selectedSlot >= 0 && slots[selectedSlot] != null)
        {
            slots[selectedSlot].Activate(player);
        }
        else
        {
            Debug.Log("Tiada geliga untuk digunakan.");
        }
    }

    // Dapat elemen aktif untuk serangan biasa
    public GeligaData GetActiveGeliga()
    {
        if (selectedSlot >= 0 && slots[selectedSlot] != null)
            return slots[selectedSlot];
        return null;
    }
}
