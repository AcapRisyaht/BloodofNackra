using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCDialogUI : MonoBehaviour
{
    public string[] dialogLines;
    public GameObject dialogBox;
    public TMP_Text dialogText;
    public Button continueButton; // Tombol untuk dialog
    public KeyCode interactKey = KeyCode.F;

    private bool PlayerInRange = false;

    void Start()
    {
        dialogBox.SetActive(false);
        continueButton.gameObject.SetActive(false); // Pastikan tombol tidak aktif awal
    }

    void Update()
    {
        if (PlayerInRange && Input.GetKeyDown(interactKey))
        {
           DialogManager.Instance.ShowDialog(dialogLines);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRange = true;
            Debug.Log("Player masuk trigger NPC.");
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRange = false;
        }
    }
}