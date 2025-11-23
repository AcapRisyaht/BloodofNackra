using UnityEngine;

public static class ElemenHelper
{
   // Tentukan elemen yang dikalahkan (kuat terhadap)
   public static JenisGeliga KuatTerhadap(JenisGeliga elemen) => elemen switch
   {
        JenisGeliga.Tanah => JenisGeliga.Kilat,
        JenisGeliga.Kilat => JenisGeliga.Air,
        JenisGeliga.Air => JenisGeliga.Api,
        JenisGeliga.Api => JenisGeliga.Angin,
        JenisGeliga.Angin => JenisGeliga.Bayang,
        JenisGeliga.Bayang => JenisGeliga.Fizikal,
        JenisGeliga.Fizikal => JenisGeliga.Tanah,
        _ => elemen
   };

    // Tentukan elemen yang mengalahkan (lemah terhadap)
    public static JenisGeliga LemahTerhadap(JenisGeliga elemen)
    {
        // Logik: kelemahan ialah elemen yang kuat terhadap awak
        foreach (JenisGeliga jenis in System.Enum.GetValues(typeof(JenisGeliga)))
        {
            if (KuatTerhadap(jenis) == elemen)
                return jenis; 
        }

        return elemen; // fallback kalau tiada
    }

    // Semak sama ada serangan efektif
    public static bool SeranganEfektif(JenisGeliga penyerang, JenisGeliga pertahanan)
    {
        return KuatTerhadap(penyerang) == pertahanan;
    }
}
