using UnityEngine;

public class UjianElemen : MonoBehaviour
{
   void Start()
   {
       // Contoh penggunaan ElemenHelper
       JenisGeliga elemenPemain = JenisGeliga.Api;
       JenisGeliga elemenMusuh = JenisGeliga.Angin;

      if (ElemenHelper.SeranganEfektif(elemenPemain, elemenMusuh))
      {
          Debug.Log($"{elemenPemain}kuat terhadap {elemenMusuh}!");
      }
      else if (ElemenHelper.LemahTerhadap(elemenPemain) == elemenMusuh)
      {
          Debug.Log($"{elemenPemain}lemah terhadap {elemenMusuh}!");
      }
      else
      {
          Debug.Log($"{elemenPemain} neutral terhadap {elemenMusuh}.");
      }
   }
}
