using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KuasaPemain : MonoBehaviour
{
    public GeligaKuasa geligaAktif;

    public void AktifkanGeliga(GeligaKuasa geliga)
    {
        geligaAktif = geliga;
        Debug.Log("Geliga diaktifkan: " + geligaAktif.namaGeliga);

        switch (geliga.jenis)
        {
            case JenisGeliga.Tanah:
                TambahKetahanan();
                break;
            case JenisGeliga.Angin:
                TambahKelajuan();
                break;
            case JenisGeliga.Air:
                TambahPemulihan();
                break;
            case JenisGeliga.Fizikal:
                LifeStealBoost();
                break;
            case JenisGeliga.Kilat:
                TambahSeranganPantas();
                break;
            case JenisGeliga.Bayang:
                KlonBayang();
                break;
            case JenisGeliga.Api:
                SeranganBakar();
                break;
        }
    }

    void TambahKetahanan() { /* logik Perisai Tanah */ }
    void TambahKelajuan() { /* logik Kelajuan Angin */ }
    void TambahPemulihan() { /* logik Pemulihan Air */ }
    void LifeStealBoost() { /* logik Life Steel Fizikal */ }
    void TambahSeranganPantas() { /* logik Serangan Pantas Kilat */ }
    void KlonBayang() { /* logik Klon Bayang */ }
    void SeranganBakar() { /* logik Serangan Bakar Api */ }
  
}
