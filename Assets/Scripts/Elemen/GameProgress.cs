using UnityEngine;

public class GameProgress : MonoBehaviour
{
   public int bossDefeated = 0;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
