using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls1 : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {

        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveX, moveY).normalized;
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }
}