using UnityEngine;
using UnityEngine.UI;

public class ToolBarController : MonoBehaviour
{
    public Image Serlahan;
    public RectTransform[] toolIcons;
    public PlayerAction pemainAction;
    public GameObject BeliungPrefab;
    public GameObject CangkulPrefab;
    public GameObject KapakPrefab;
    public GameObject SabitPrefab;
    public GameObject PenyiramPrefab;

    private int currentIndex = 0;
    private float actionCooldown = 0.5f;
    private float lastActionTime = 0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentIndex = Mathf.Max(0, currentIndex - 1);
            MoveSerlahan();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentIndex = Mathf.Min(toolIcons.Length - 1, currentIndex + 1);
            MoveSerlahan();
        }

        if (Input.GetKeyDown(KeyCode.G) && Time.time - lastActionTime >= actionCooldown)
        {
            lastActionTime = Time.time;

           int toolType = currentIndex + 1;

           pemainAction.Selection(toolType);

           if (toolType == 3) pemainAction.Equip(BeliungPrefab);

           if (toolType == 1) pemainAction.Equip(CangkulPrefab);
        
           if (toolType == 2) pemainAction.Equip(KapakPrefab);

           if (toolType == 5) pemainAction.Equip(SabitPrefab);

           if (toolType == 4) pemainAction.Equip(PenyiramPrefab);

           Vector2 arahTerakhir = FindAnyObjectByType<PlayerMovement>().LastMove();
           pemainAction.DoAction(arahTerakhir);
        } 
    }

    void MoveSerlahan()
    {
        Serlahan.rectTransform.position = toolIcons[currentIndex].position;
    }
}
