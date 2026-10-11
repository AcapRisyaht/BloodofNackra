using UnityEngine;
using System.Collections;

public class PlayerAction : MonoBehaviour
{
    public Animator animator;
    public Transform handSlotDown;
    public Transform handSlotUp;
    public Transform handSlotLeft;
    public Transform handSlotRight;
    private int toolType = 0;
    private Vector2 LastMove = Vector2.zero;
    private Transform activeSlot;
    private GameObject currentTool;
    private bool canAct = true;

    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.ResetTrigger("doAction");
        
        handSlotDown = transform.Find("HandSlotDown");
        handSlotUp = transform.Find("HandSlotUp");
        handSlotLeft = transform.Find("HandSlotLeft");
        handSlotRight = transform.Find("HandSlotRight");
    }

    public void Selection(int type)
    {
        toolType = type;
    }

    public void Equip(GameObject toolPrefab)
    {
        if (LastMove == Vector2.down) activeSlot = handSlotDown;
        else if (LastMove == Vector2.up) activeSlot = handSlotUp;
        else if (LastMove == Vector2.left) activeSlot = handSlotLeft;
        else if (LastMove == Vector2.right) activeSlot = handSlotRight;
        else activeSlot = handSlotDown;

        if (currentTool != null) Destroy(currentTool);

        currentTool = Instantiate(toolPrefab, activeSlot);
        currentTool.transform.localPosition = Vector3.zero;
        currentTool.transform.localRotation = Quaternion.identity;
    }

    public void DoAction(Vector2 lastMove)
    {
        float cooldownTime = 0.5f; // Default 

        // kalau alat ringan, pendekkan masa
        if (toolType == 4 || toolType == 5)
            cooldownTime = 1f;

        if (!canAct) return; 
        StartCoroutine(ActionCooldown(cooldownTime)); // Start cooldown coroutine

        if (animator.GetBool("IsActing")) return; // Kalau masih acting, jangan trigger lagi

        if (lastMove == Vector2.zero) lastMove =  Vector2.down; // Default to down if no movement direction is provided

        int x = Mathf.RoundToInt(lastMove.x);
        int y = Mathf.RoundToInt(lastMove.y);

        animator.SetInteger("toolType", toolType);
        animator.SetInteger("toolX", x);
        animator.SetInteger("toolY", y);
        animator.SetBool("IsActing", true);

        if (currentTool != null)
        {
            Animator toolAnimator = currentTool.GetComponent<Animator>();
            toolAnimator.SetInteger("toolX", x);
            toolAnimator.SetInteger("toolY", y);

            if (x == 0 && y == -1) toolAnimator.SetTrigger("doActionDown");
            else if (x == 0 && y == 1) toolAnimator.SetTrigger("doActionUp");
            else if (x == -1 && y == 0) toolAnimator.SetTrigger("doActionLeft");
            else if (x == 1 && y == 0) toolAnimator.SetTrigger("doActionRight");
            else toolAnimator.ResetTrigger("doActionDown");
        }
    }

    public void EndAction()
    {
        animator.SetBool("IsActing", false);
    }

    public void ToolAction(string direction)
    {
        if (currentTool != null)
        {
            Animator toolAnimator = currentTool.GetComponent<Animator>();
             
            switch (direction)
            {
                case"Down": toolAnimator.SetTrigger("doActionDown"); break;
                case"Up": toolAnimator.SetTrigger("doActionUp"); break;
                case"Left": toolAnimator.SetTrigger("doActionLeft"); break;
                case"Right": toolAnimator.SetTrigger("doActionRight"); break;
            }
        }
    }

    IEnumerator ActionCooldown(float time)
    {
        canAct = false;
        yield return new WaitForSeconds(time);
        canAct = true;
    }
}