using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public PlayerInventory playerInventory;
    public TMPro.TextMeshProUGUI teksSM;

    void Update()
    {
        teksSM.text = playerInventory.jumlahSM + " SM";
    }
}
