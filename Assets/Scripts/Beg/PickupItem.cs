using UnityEngine;

/// <summary>
/// PickupItem — Item yang boleh diambil player.
/// Guard: Urus data item + trigger pickup.
/// </summary>
public class PickupItem : MonoBehaviour
{
    public ItemData itemData;   // Guard: Data item (ikon, nama, dll.)
    public int jumlah = 1;      // Guard: Berapa banyak item

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerInventory inv = collision.GetComponent<PlayerInventory>();
        if (inv != null)
        {
            inv.AddItem(itemData, jumlah);
            Destroy(gameObject); // Guard: Hilangkan objek selepas pickup
        }
    }
}