using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ItemSlotUI — Paparkan satu slot item dalam UI.
/// Guard: Hanya paparan, tiada logik inventori.
/// </summary>
public class ItemSlotUI : MonoBehaviour
{
    public Image icon;
    public Text jumlahText;

    public void Setup(ItemSlot slot)
    {
        if (slot == null || slot.item == null)
        {
            if (icon) icon.enabled = false;
            if (jumlahText) jumlahText.text = "";
            return;
        }

        if (icon)
        {
            icon.enabled = true;
            icon.sprite = slot.item.icon;
        }

        if (jumlahText)
        {
            jumlahText.text = slot.jumlah.ToString();
        }
    }
}