using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Load fail dari folder Resources
        AudioClip footstepClip = Resources.Load<AudioClip>("FootstepSound");
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = footstepClip;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            GetComponent<AudioSource>().Play();
        }
    }
}
