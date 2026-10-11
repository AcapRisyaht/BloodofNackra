using System.Collections;
using UnityEngine;

public class Dummy : MonoBehaviour
{
   public Animator animator;
   public ParticleSystem dustEffect;
   public float knockbackDistance = 0.2f;
   public float knockbackDuration = 0.1f;

   Rigidbody2D rb;
   Vector3 originalPos;

   void Start()
   {
        rb = GetComponent<Rigidbody2D>();
        originalPos = transform.position;
   }

   void Update()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = -(int)(transform.position.y * 100);
    }

   public void TakeHit(Transform attacker)
    {
        // Main animasi
        animator.SetTrigger("OnHit");
        
        // Main effek debu 
        if (dustEffect != null)
        {
            dustEffect.Play();
        }

        // Gerakan ke belakang sikit
        Vector2 knockDir = (transform.position - attacker.position).normalized;

        Vector3 hitPos = transform.position + (Vector3)(knockDir * knockbackDistance);

        rb.MovePosition(hitPos);

        StartCoroutine(ResetPosition());

        IEnumerator ResetPosition()
        {
            yield return new WaitForSeconds(knockbackDuration);
            rb.MovePosition(originalPos);
        }

    }
}
