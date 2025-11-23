using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class FloatingTextUniversal : MonoBehaviour
{
    public TextMeshProUGUI text;

    public float floatSpeed = 20f;
    public float fadeDuration = 1f;
    public Vector3 offset = new Vector3(0, 0.3f, 0);
    public bool useRandomOffset = true;

    private CanvasGroup canvasGroup;
    private Vector3 moveDirection = Vector3.up;

    void Awake()
    {
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.GetComponent<CanvasGroup>();
    }

    public void ShowDamage(int amount, Transform spawnPoint)
    {
        Vector3 basePosition = spawnPoint.position;
        Vector3 offset = new Vector3(Random.Range(-0.2f, 0.2f), 0.3f, 0);
        Vector3 finalPosition = basePosition + offset;
        finalPosition.z = 0;

        transform.position = finalPosition;

        text.text = amount.ToString();
        text.color = Color.red;
        Debug.Log("Warna damage text ditetapkan ke: " + text.color);
        

        StartCoroutine(FadeOutAndMove());
    }

    void Update()
    {
        transform.position += moveDirection * floatSpeed * Time.deltaTime;
    }
    
    IEnumerator FadeOutAndMove()
    {
        float elapsedTime = 0f;
        Color originalColor = text.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            canvasGroup.alpha = alpha;
            yield return null;
        }

        Destroy(gameObject);
    }
}
