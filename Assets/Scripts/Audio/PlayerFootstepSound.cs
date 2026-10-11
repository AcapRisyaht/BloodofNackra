using UnityEngine;

public class PlayerFootstepSound : MonoBehaviour
{
   public AudioSource sumberBunyi; // rujuk Audip Source "sumber bunyi"
   public AudioClip bunyitapakkaki ; // rujuk array bunyi langkah

   public void PlayFootstepSound()
   {
       sumberBunyi.PlayOneShot(bunyitapakkaki); // mainkan bunyi langkah
   }
}
