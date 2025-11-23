using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySetup : MonoBehaviour
{
    public EnemyData enemyData;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        if (enemyData == null)
        {
            Debug.LogError("EnemyData belum disambung!");
        }

        // Tukar sprite dan animator ikut data
        spriteRenderer.sprite = enemyData.sprite;
        animator.runtimeAnimatorController = enemyData.animator;

        // Debug Log untuk semak sambungan
        Debug.Log("Musuh dipasang:" + enemyData.enemyName);
        Debug.Log("Nyawa Musuh:" + enemyData.maxHealth);
        Debug.Log("Waena damage: " + enemyData.damageColor);
    }   
}
