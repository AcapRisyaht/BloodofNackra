using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GeligaLoot : MonoBehaviour
{
    public GeligaData dataGeliga;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            KuasaPemain pemain = other.GetComponent<KuasaPemain>();
            if (pemain != null && dataGeliga != null)
            {
                pemain.AktifkanGeliga(dataGeliga);
                Destroy(gameObject);
            }
        }
    }
}
