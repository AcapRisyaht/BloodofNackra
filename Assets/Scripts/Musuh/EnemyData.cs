using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public Sprite sprite;
    public RuntimeAnimatorController animator;
    public int maxHealth;
    public Color damageColor;
}