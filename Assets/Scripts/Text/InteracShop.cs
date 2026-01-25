using UnityEngine;

/// <summary>
/// InteractShop — Kawal interaksi player dengan NPC Shop.
/// Guard: Tekan F bila dekat NPC.
/// </summary>
public class InteractShop : MonoBehaviour
{
    private ShopNPC npcDekat;
    private PlayerInventory playerInventory;

    void Start()
    {
        playerInventory = GetComponent<PlayerInventory>();
    }

    void Update()
    {
        // Guard: Tekan F untuk interaksi
        if (Input.GetKeyDown(KeyCode.F) && npcDekat != null)
        {
            // Contoh beli item bernama "Penawar"
            npcDekat.BeliItem(playerInventory, "Penawar");
        }
    }

    // Detect NPC Shop bila player masuk trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        ShopNPC shop = other.GetComponent<ShopNPC>();
        if (shop != null)
        {
            npcDekat = shop;
            Debug.Log("Dekat dengan NPC Shop, tekan F untuk beli.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ShopNPC shop = other.GetComponent<ShopNPC>();
        if (shop == npcDekat)
        {
            npcDekat = null;
            Debug.Log("Keluar dari kawasan NPC Shop.");
        }
    }
}