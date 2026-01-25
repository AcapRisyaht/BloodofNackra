using UnityEngine;

public class ItemEffect : MonoBehaviour
{
    public void UseItem(ItemData item, PlayerStatus player)
    {
        if (item.namaItem == "Penawar Tanah")
        {
            player.RemoveSlow();
        }
        // boleh tambah else if untuk item lain
    }
}