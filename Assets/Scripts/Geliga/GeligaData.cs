using UnityEngine;

[CreateAssetMenu(menuName = "Item/Geliga")]
public class GeligaData : ScriptableObject
{
   public string namaGeliga;
   public string keterangan;
   public Sprite ikon;
   [TextArea] public string asalUsul;
   [TextArea] public string kegunaan;
   public int bonusDamage;
   public JenisGeliga jenis;

   // Optionsl: efek visual & bunyi
   public GameObject efekPrefab;
   public AudioClip bunyiAktif;

   // Fungsi khas bila geliga digunakan (E)
   public void Activate(GameObject player)
   {
      Debug.Log("Kuasa geliga " + namaGeliga + " diaktifkan!");
      
      if (efekPrefab != null)
          GameObject.Instantiate(efekPrefab, player.transform.position, Quaternion.identity);

      if (bunyiAktif != null)
      AudioSource.PlayClipAtPoint(bunyiAktif, player.transform.position);
   }
}
