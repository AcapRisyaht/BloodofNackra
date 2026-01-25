using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TanamBenih : MonoBehaviour
{
   public GameObject PokokKecilprefab;
   public PlayerInventory playerInventory;
   public ItemData benihPokok;

    public void Tanam(Vector2 posisiTanah)
    {
       int kira = playerInventory.CountItem(benihPokok);
       if (kira > 0)
       {
           Instantiate(PokokKecilprefab, posisiTanah, Quaternion.identity);
           playerInventory.RemoveItem(benihPokok, 1);
           Debug.Log("Benih pokok telah ditanam!");
       }
       else
       {
           Debug.Log("Tiada benih pokok dalam inventori!");
       }
    }
}
