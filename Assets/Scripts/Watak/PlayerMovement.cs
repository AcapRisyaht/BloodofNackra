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

        if (isMoving)
        {
            myAnimator.SetFloat("MoveX", movement.x);
            myAnimator.SetFloat("MoveY", movement.y);
            myAnimator.SetFloat("LastMoveX", movement.x > 0 ? 1 : (movement.x < 0 ? -1 : 0));
            myAnimator.SetFloat("LastMoveY", movement.y > 0 ? 1 : (movement.y < 0 ? -1 : 0));
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
            float speed = baseMoveSpeed * status.GetSpeedFactor();
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        }
    }

    public void Knockback(Vector2 force)
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(force, ForceMode2D.Impulse);
    }
}