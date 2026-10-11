using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float baseMoveSpeed = 5f;

    private Rigidbody2D rb;
    private PlayerControls playControls;
    private Vector2 movement;
    private Animator myAnimator;
    private PlayerStatus status; // rujuk status

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        status = GetComponent<PlayerStatus>();
    }

    void Awake()
    {
        playControls = new PlayerControls();
    }

    void Update()
    {
        PlayerInput();
    }

    void OnEnable()
    {
        playControls.Enable();
        playControls.Movement.Enable();
    }

    void OnDisable()
    {
        playControls.Disable();
        playControls.Movement.Disable();
    }

    private void PlayerInput()
    {
        movement = playControls.Movement.Move.ReadValue<Vector2>();

        bool isMoving = movement.sqrMagnitude > 0.01f;
        myAnimator.SetBool("IsMoving", isMoving);

        // Klik Kanan = untuk lari
        bool isRunning = movement.sqrMagnitude > 0.01f && Input.GetMouseButton(1);
        myAnimator.SetBool("IsRunning", isRunning);

        if (isMoving)
        {
            myAnimator.SetFloat("MoveX", movement.x);
            myAnimator.SetFloat("MoveY", movement.y);

            myAnimator.SetFloat("LastMoveX", movement.x);
            myAnimator.SetFloat("LastMoveY", movement.y);
        }
        else
        {
            myAnimator.SetFloat("MoveX", 0);
            myAnimator.SetFloat("MoveY", 0);

        }
    }

    void FixedUpdate()
    {
        if (status.isStunned)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (movement.sqrMagnitude > 0.01f)
        {
            bool isRunning = myAnimator.GetBool("IsRunning");
            float speed = isRunning ? baseMoveSpeed * 2f : baseMoveSpeed;
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        }
    }

    public void Knockback(Vector2 force)
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public Vector2 LastMove()
    {
        float lastMoveX = myAnimator.GetFloat("LastMoveX");
        float lastMoveY = myAnimator.GetFloat("LastMoveY");
        return new Vector2(lastMoveX, lastMoveY);
    }
}