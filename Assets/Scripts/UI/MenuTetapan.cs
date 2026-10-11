using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTetapan : MonoBehaviour
{
    public GameObject panelTetapan;
    private bool aktif = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleTetapanOn();
        }
    }

    public  void ToggleTetapanOn()
    {
       aktif = !aktif;
       panelTetapan.SetActive(aktif);

       if (aktif)
       {
           Time.timeScale = 0f; // Hentikan waktu saat menu aktif
       }
       else
       {
           Time.timeScale = 1f; // Lanjutkan waktu saat menu tidak aktif
       }
    }
}
