using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;           // Reference to the player
    public float attackRange = 1.5f;   // Range at which enemy attacks
    public float detectionRange = 5f;  // Range at which the enemy detects the player
    public float loiterSpeed = 1f;     // Speed at which the enemy loiters
    public float loiterRange = 1f;     // The small range the enemy loiters in
    public float damageAmount = 1f;    // Amount of damage the enemy deals
    public float attackCooldown = 1f;  // Time between attacks in seconds
    private Animator animator;         // Reference to Animator component
    private Health playerHealth;       // Reference to the player's Health script
    private float lastAttackTime;      // Tracks when the enemy last attacked
    private bool isLoitering = true;   // Whether the enemy is currently loitering
    private Vector3 originalPosition;  // The original position the enemy started at
    private Vector3 loiterTarget;      // The target position for loitering
    private bool isDetectingPlayer = false; // Whether the player is detected

    void Start()
    {
        animator = GetComponent<Animator>();
        playerHealth = player.GetComponent<Health>();
        lastAttackTime = -attackCooldown;
        originalPosition = transform.position;
        SetLoiterTarget();
        FaceLeft();
    }

    void Update()
    {
        // Calculate the distance between the enemy and the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // If player is in detection range but not in attack range
        if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange)
        {
            StopLoitering();
            isDetectingPlayer = true;
            animator.SetBool("isMoving", false);  // Stop the running animation
            animator.SetBool("isAttacking", false); // Attack animation not triggered yet
        }
        // If player is in attack range
        else if (distanceToPlayer <= attackRange)
        {
            StopLoitering();
            isDetectingPlayer = true;
            animator.SetBool("isMoving", false);  // Stop moving
            animator.SetBool("isAttacking", true);  // Play attack animation
            TryToAttackPlayer();
        }
        // If player is outside detection range, loiter
        else
        {
            isDetectingPlayer = false;
            Loiter();
            animator.SetBool("isAttacking", false);  // Ensure no attacking while loitering
            animator.SetBool("isMoving", true);  // Play running animation during loitering
        }
    }

    // Loitering behavior when the player is not detected
    void Loiter()
    {
        if (!isLoitering) return;

        // Move towards the loiter target
        transform.position = Vector3.MoveTowards(transform.position, loiterTarget, loiterSpeed * Time.deltaTime);

        // If the enemy reaches the loiter target, pick a new target
        if (Vector3.Distance(transform.position, loiterTarget) < 0.1f)
        {
            SetLoiterTarget();
        }
    }

    // Stop loitering when the player is detected
    void StopLoitering()
    {
        if (isLoitering)
        {
            isLoitering = false;  // Enemy should stop loitering when detecting the player
            animator.SetBool("isMoving", false);  // Stop the running animation
        }
    }

    // Set a new loiter target within the small loiter range
    void SetLoiterTarget()
    {
        float randomOffset = Random.Range(-loiterRange, loiterRange);
        loiterTarget = new Vector3(originalPosition.x + randomOffset, originalPosition.y, originalPosition.z);

        // Face the direction of the loiter target
        FaceDirection(loiterTarget - transform.position);
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
