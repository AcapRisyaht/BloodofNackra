using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TebangPokok : MonoBehaviour
{
    public ItemData kayu;
    public ItemData benihPokok;
    public PlayerInventory playerInventory;

    public void Tebang()
    {
        // Drop 3 kayu apabila pokok ditebang
        playerInventory.AddItem(kayu, 3);

        // Drop 1 benih 
        playerInventory.AddItem(benihPokok, 1);

        Debug.Log("Pokok telah ditebang!");
        // Logik tambahan untuk menambah kayu ke inventori pemain boleh ditambah di sini
    }
}
