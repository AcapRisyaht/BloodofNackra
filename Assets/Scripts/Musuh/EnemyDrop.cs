using UnityEngine;

/// <summary>
/// EnemyDrop — Drop item bila musuh mati.
/// Guard: Hanya urus spawn item, bukan logik combat.
/// </summary>
public class EnemyDrop : MonoBehaviour
{
    public ItemData itemDrop;   // Guard: Item yang akan dijatuhkan
    public int jumlahDrop = 1;  // Guard: Berapa banyak item

    public GameObject itemPrefab; // Guard: Prefab pickup di lantai

    public void OnEnemyDefeated()
    {
        if (itemPrefab != null)
        {
            GameObject drop = Instantiate(itemPrefab, transform.position, Quaternion.identity);
            PickupItem pickup = drop.GetComponent<PickupItem>();
            if (pickup != null)
            {
                pickup.itemData = itemDrop;
                pickup.jumlah = jumlahDrop;
            }
        }
    }
}