using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool isDead = false;

    public GameObject FloatingTextUniversal;
    public Transform textSpawnPoint;
    public Transform canvasTransform;
    public GameObject lootGeligaPrefab;

    public string namaUIBoss = "BarNyawaBoss"; // boleh override kalau perlu
    public static BossHealth currentBoss;

    private GameObject bossHealthUI;
    private Slider NyawaSlider;
    private SpriteRenderer sr;

    IEnumerator Start()
    {
        yield return null; // tunggu satu frame supaya semua GameObject siap

        currentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();

        int retry = 0;
        while (bossHealthUI == null && retry < 30)
        {
            bossHealthUI = GameObject.Find(namaUIBoss);
            retry++;
            yield return null;
        }
        

        if (bossHealthUI == null)
        {
            yield break; // hentikan Start() jika UI gagal dijumpai
        }

        NyawaSlider = bossHealthUI.GetComponentInChildren<Slider>();
        bossHealthUI.SetActive(false);

        if (NyawaSlider != null)
        {
            NyawaSlider.value = 1f;
        }
    }
    
    void Update()
    {
        if (bossHealthUI == null || isDead || (currentBoss != null && currentBoss != this)) return;

        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        if (Player != null)
        {
            float jarak = Vector2.Distance(transform.position, Player.transform.position);

            if (jarak <= 5f)
            {
                currentBoss = this;
                TunjukUI(true);
            }
            else if (currentBoss == this)
            {
                TunjukUI(false);
                currentBoss = null;
            }

            SemakUIKosong();
        }      
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        ShowFloatingText(damage, textSpawnPoint);

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (NyawaSlider != null && bossHealthUI.activeSelf)
        {
            float ratio = (float)currentHealth / maxHealth;
            NyawaSlider.value = ratio;
        }


        if (currentHealth <= 0)
            Die();

        if (sr != null)
        {
            sr.color = Color.red;
            StartCoroutine(ResetColor());
        }
    }
    
     void ShowFloatingText(int amount, Transform spawnPoint)
    {
        if (Camera.main == null || canvasTransform == null || textSpawnPoint == null)
            return;

        Vector3 offset = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(0.1f, 0.1f), 0);
        Vector3 spawnPosition = textSpawnPoint.position + offset;

        spawnPosition.z = 0; // depan kamera 2D

        GameObject go = Instantiate(FloatingTextUniversal, Vector3.zero, Quaternion.identity, canvasTransform);
        go.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(spawnPosition);

        FloatingTextUniversal ft = go.GetComponent<FloatingTextUniversal>();
        if (ft != null)
            ft.ShowDamage(amount, textSpawnPoint);
    }

    public void Die()
    {
        isDead = true;
        Debug.Log("Boss mati!");

        if (currentBoss == this)
            currentBoss = null;

        if (bossHealthUI != null)
        {
            Text namaBoss = bossHealthUI.GetComponentInChildren<Text>();
            if (namaBoss != null)
                namaBoss.text = "";
            
            NyawaSlider.value = 0f;
        }

        if (lootGeligaPrefab != null)
        {
            GameObject loot = Instantiate(lootGeligaPrefab, transform.position, Quaternion.identity);
            Debug.Log("Loot geliga muncul di " + loot.transform.position);
        }

        GameProgress gp = FindObjectOfType<GameProgress>();
        if (gp != null)
        {
            gp.bossDefeated++;
            Debug.Log("Jumlah bos defeated: " + gp.bossDefeated);
            
            // Update roda geliga ikut progress
            RadialMenu rm = FindObjectOfType<RadialMenu>();
            if (rm != null)
            {
                rm.UpdateGeligaSlots();
            }
        }

        gameObject.SetActive(false);
        
        SemakUIKosong();
    }

    public void TunjukUI(bool aktif)
    {
        if (bossHealthUI != null && !isDead)
        {
            bossHealthUI.SetActive(aktif);

            if (aktif)
            {
                Text namaBoss = bossHealthUI.GetComponentInChildren<Text>();
                if (namaBoss != null)
                    namaBoss.text = gameObject.name;

                if (NyawaSlider != null)
                {
                    float ratio = (float)currentHealth / maxHealth;
                    NyawaSlider.value = ratio;
                }
            }
        }
    } 

    void SemakUIKosong()
    {
        if (currentBoss == null && bossHealthUI != null)
        {
            bossHealthUI.SetActive(false);

            Text namaBoss = bossHealthUI.GetComponentInChildren<Text>();
            if (namaBoss != null)
                namaBoss.text = "";

            if (NyawaSlider != null)
                NyawaSlider.value = 0f;
        }
    }
    
    IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(0.2f);
        sr.color = Color.white;
    }
}
