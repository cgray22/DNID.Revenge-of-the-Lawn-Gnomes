using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float wallSlideSpeed = 0.5f;  // How fast player slides down a wall
    [SerializeField] private float wallJumpLerp = 10f;     // Smoothing factor for wall jumps
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private float wallJumpCooldown;
    private float horizontalInput;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        // Get horizontal input
        horizontalInput = Input.GetAxis("Horizontal");

        // Flip character based on movement direction
        if (!onWall() && horizontalInput != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(horizontalInput), 1, 1);
        }

        // Set animator parameters
        anim.SetBool("Run", horizontalInput != 0);
        anim.SetBool("Ground", isGrounded());

        // Wall jump cooldown timer
        if (wallJumpCooldown > 0)
        {
            wallJumpCooldown -= Time.deltaTime;
        }

        // Movement and jump logic
        if (wallJumpCooldown <= 0)
        {
            // Horizontal movement
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            // Wall sliding logic: slow down when on a wall
            if (onWall() && !isGrounded())
            {
                // Slow down descent when sliding
                body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, -wallSlideSpeed, float.MaxValue));
            }

            // Jumping logic
            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
            }
        }
        else
        {
            // During wall jump cooldown, lerp towards the desired velocity for smoother jumps
            body.velocity = Vector2.Lerp(body.velocity, new Vector2(horizontalInput * speed, body.velocity.y), wallJumpLerp * Time.deltaTime);
        }
    }

    private void Jump()
    {
        if (isGrounded())
        {
            // Ground jump
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            anim.SetTrigger("Jump");
        }
        else if (onWall() && !isGrounded())
        {
            // Wall jump logic
            if (horizontalInput != 0)
            {
                // Diagonal wall jump away from the wall
                body.velocity = new Vector2(horizontalInput * speed * 1.2f, jumpForce);
            }
            else
            {
                // Jump straight off the wall with more force
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 8f, jumpForce);
            }
            wallJumpCooldown = 0.2f; // Small cooldown to prevent immediate second wall jump
        }
    }

    private bool isGrounded()
    {
        // BoxCast to check if grounded
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        // BoxCast to check if on a wall
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
}
