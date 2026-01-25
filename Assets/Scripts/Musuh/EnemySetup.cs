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
            
        }

        // Tukar sprite dan animator ikut data
        spriteRenderer.sprite = enemyData.sprite;
        animator.runtimeAnimatorController = enemyData.animator;
    }   
}
