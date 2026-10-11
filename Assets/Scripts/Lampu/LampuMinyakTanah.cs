using UnityEngine;

public class LampuMinyakTanah : MonoBehaviour
{
   Animator animator;

   void Start()
   {
      animator = GetComponent<Animator>();
      animator.Play("Lampu Minyak Idle");
   }
}
