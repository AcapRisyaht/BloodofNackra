using UnityEngine;

public class LombongBijihTimah : MonoBehaviour
{
    public ItemData bijihTimah;
    public PlayerInventory playerInventory;
    public int jumlahdrop = 1;
    public int ketahanan = 3;

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
           if (ketahanan > 0)
        {
            playerInventory.AddItem(bijihTimah, jumlahdrop);
            ketahanan--;

            if (ketahanan == 0)
            {
                Destroy(gameObject);
            }
        }
        }
        
    }
}
