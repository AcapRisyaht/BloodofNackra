using UnityEngine;

public class Alatan : MonoBehaviour
{
   public ItemData dataAlatan;

   // Guard: Fungsi untuk menggunakan alatan
   public void GunakanAlatan(GameObject target)
   {
       if (dataAlatan.namaItem.Contains("Kapak"))
       {
           // Tebang pokok atau kayu
           TebangPokok pokok = target.GetComponent<TebangPokok>();
           if (pokok != null) pokok.Tebang();
          
       }
       else if (dataAlatan.namaItem.Contains("Cangkul"))
       {
           // Logik untuk menggunakan cangkul pada sasaran
           KorekTanah tanah = target.GetComponent<KorekTanah>();
           if (tanah != null) tanah.Korek();
           Debug.Log("Menggunakan cangkul pada " + target.name);
       }
       else if (dataAlatan.namaItem.Contains("Sabit"))
        {
              // Logik untuk menggunakan sabit pada sasaran
              PotongTanaman rumput = target.GetComponent<PotongTanaman>();
              if (rumput != null) rumput.Potong();
              Debug.Log("Menggunakan sabit pada " + target.name);
        }
       else if (dataAlatan.namaItem.Contains("Beliung"))
       {
           // Logik untuk menggunakan penggali pada sasaran
           GaliTanah tanah = target.GetComponent<GaliTanah>();
           if (tanah != null) tanah.Gali();
           Debug.Log("Menggunakan beliung pada " + target.name);
       }
       else if (dataAlatan.namaItem.Contains("Penyiram"))
       {
           // Logik untuk menggunakan penyiram pada sasaran
           Penyiram tanaman = target.GetComponent<Penyiram>();
           if (tanaman != null) tanaman.Siram();
           Debug.Log("Menggunakan penyiram pada " + target.name);
       }
       else if (dataAlatan.namaItem.Contains("Penyodok"))
       {
           // Logik untuk menggunakan penyodok pada sasaran
           GaliTanah tanah = target.GetComponent<GaliTanah>();
           if (tanah != null) tanah.Gali();
           Debug.Log("Menggunakan penyodok pada " + target.name);
       }

   }
}
