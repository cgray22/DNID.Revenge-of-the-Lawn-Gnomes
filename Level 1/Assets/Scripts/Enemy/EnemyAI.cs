using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;           // Reference to the player
    public float attackRange = 1.5f;   // Range at which enemy attacks
    public float detectionRange = 5f;  // Range at which the enemy detects the player
    public float runSpeed = 3f;        // Speed at which the enemy runs towards the player
    public float damageAmount = 1f;    // Amount of damage the enemy deals
    public float attackCooldown = 1f;  // Time between attacks in seconds
    private Animator animator;         // Reference to Animator component
    private Health playerHealth;       // Reference to the player's Health script
    private float lastAttackTime;      // Tracks when the enemy last attacked
    private bool isDetectingPlayer = false; // Whether the player is detected
    private Vector3 idlePosition;      // Position where the enemy idles

    void Start()
    {
        animator = GetComponent<Animator>();
        playerHealth = player.GetComponent<Health>();
        lastAttackTime = -attackCooldown;
        idlePosition = transform.position; // Store the original idle position
        FaceLeft(); // Ensure the enemy is facing the default direction
    }

    void Update()
    {
        // Calculate the distance between the enemy and the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // If player is in detection range but not in attack range
        if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange)
        {
            isDetectingPlayer = true;
            animator.SetBool("isMoving", true);    // Play running animation
            animator.SetBool("isAttacking", false); // Stop attack animation
            MoveTowardsPlayer();
        }
        // If player is in attack range
        else if (distanceToPlayer <= attackRange)
        {
            isDetectingPlayer = true;
            animator.SetBool("isMoving", false);    // Stop running
            animator.SetBool("isAttacking", true);  // Play attack animation
            TryToAttackPlayer();
        }
        // If player is outside detection range, go idle
        else
        {
            if (isDetectingPlayer)
            {
                // Reset to idle state when player exits detection range
                isDetectingPlayer = false;
                animator.SetBool("isMoving", false);    // Stop running animation
                animator.SetBool("isAttacking", false); // Stop attack animation
                ReturnToIdlePosition();
            }
        }
    }

    // Move towards the player when detected
    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * runSpeed * Time.deltaTime;

        // Face the direction of the player
        FaceDirection(direction);
    }

    // Attempt to attack the player
    void TryToAttackPlayer()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            AttackPlayer();
            lastAttackTime = Time.time;  // Reset the cooldown timer
        }
    }

    // Attack the player
    void AttackPlayer()
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
        }
    }

    // Move the enemy back to the idle position
    void ReturnToIdlePosition()
    {
        transform.position = Vector3.MoveTowards(transform.position, idlePosition, runSpeed * Time.deltaTime);

        // Face the direction back to the idle position
        FaceDirection(idlePosition - transform.position);
    }

    // Face a given direction (left or right)
    void FaceDirection(Vector3 direction)
    {
        if (direction.x < 0 && transform.localScale.x > 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);  // Face left
            transform.localScale = scale;
        }
        else if (direction.x > 0 && transform.localScale.x < 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);  // Face right
            transform.localScale = scale;
        }
    }

    // Ensure the enemy is facing left
    void FaceLeft()
    {
        if (transform.localScale.x > 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);  // Flip to face left
            transform.localScale = scale;
        }
    }
}
