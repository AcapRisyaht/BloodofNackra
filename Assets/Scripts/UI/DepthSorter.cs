using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DepthSorter : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] private int offset = 0; // offset tambahan untuk beza layer
    [SerializeField] private int multiplier = 100; // kawal ketepatan

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        spriteRenderer.sortingOrder = -Mathf.RoundToInt(transform.position.y * multiplier) + offset;
    }
}
