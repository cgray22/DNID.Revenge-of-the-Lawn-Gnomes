using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce; // Separate variable for jump force
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

        // Flip character only if moving and not on a wall or wall jumping
        if (!onWall() && horizontalInput != 0)
        {
            if (horizontalInput > 0.01f)
                transform.localScale = Vector3.one; // Facing right
            else if (horizontalInput < -0.01f)
                transform.localScale = new Vector3(-1, 1, 1); // Facing left
        }

        // Set animator parameters
        anim.SetBool("Run", horizontalInput != 0);
        anim.SetBool("Ground", isGrounded());

        // Handle movement and jumps, with cooldown for wall jumps
        if (wallJumpCooldown < 0.3f)
        {
            // Horizontal movement
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            // On-wall logic
            if (onWall() && !isGrounded())
            {
                body.gravityScale = 1;  // Reduced gravity for wall sliding
                body.velocity = new Vector2(0, body.velocity.y * 0.8f);  // Apply a small downward velocity for sliding
            }
            else
            {
                body.gravityScale = 3;  // Re-enable gravity when not on a wall
            }

            // Jump if space is pressed
            if (Input.GetKey(KeyCode.Space))
                Jump();
        }
        else
        {
            // Cooldown timer after a wall jump
            wallJumpCooldown += Time.deltaTime;
        }
    }

    private void Jump()
    {
        // Ground jump
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);  // Apply jump force
            anim.SetTrigger("Jump");
            wallJumpCooldown = 0;  // Reset cooldown
        }
        // Wall jump
        else if (onWall() && !isGrounded())
        {
            if (horizontalInput == 0)
            {
                // Jump away from the wall if no horizontal input
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 7, jumpForce);
            }
            else
            {
                // Diagonal wall jump with horizontal input
                body.velocity = new Vector2(horizontalInput * speed * 0.8f, jumpForce);
            }
            wallJumpCooldown = 0;  // Reset cooldown after wall jump
        }
    }

    private bool isGrounded()
    {
        // BoxCast to detect if player is on the ground
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        // BoxCast to detect if player is on a wall
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
}
