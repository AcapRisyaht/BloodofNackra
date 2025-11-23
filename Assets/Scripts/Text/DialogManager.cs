using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;

    public GameObject dialogBox;
    public TMP_Text DialogText;
    public Button continueButton;

    private string[] Lines;
    private int currentLine;
    private bool isTyping = false;

    void Start()
    {
        if (dialogBox == null || DialogText == null || continueButton == null)
        {
            Debug.LogError("DialogManager tidak diatur dengan benar. Pastikan semua referensi diisi.");
            return;
        }
    }

    void Awake()
    {
        Instance = this;
        dialogBox.SetActive(false);
        continueButton.gameObject.SetActive(false);
        continueButton.onClick.AddListener(NextLine);
    }

    public void ShowDialog(string[] dialoglines)
    {
        if (dialoglines == null || dialoglines.Length == 0)
        {
            Debug.LogWarning("Dialog lines are empty or null.");
            return;
        }
        Lines = dialoglines;
        currentLine = 0;
        DialogText.text = Lines[currentLine];
        dialogBox.SetActive(true);
        continueButton.gameObject.SetActive(true);
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        DialogText.text = "";
        foreach (char c in Lines[currentLine].ToCharArray())
        {
            DialogText.text += c;
            yield return new WaitForSeconds(0.02f);
        }
        isTyping = false;
    }

    void NextLine()
    {

        if (isTyping)
        {
            StopAllCoroutines();
            DialogText.text = Lines[currentLine];
            isTyping = false;
            return;
        }

        currentLine++;
        if (currentLine < Lines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            dialogBox.SetActive(false);
            continueButton.gameObject.SetActive(false);
            currentLine = 0;
            DialogText.text = "";
            Lines = null;
        }
    }   
}
