using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;           // Reference to the player
    public float attackRange = 1.5f;   // Range at which enemy attacks
    public float detectionRange = 5f;  // Range at which the enemy detects the player
    private Animator animator;         // Reference to Animator component

    void Start()
    {
        animator = GetComponent<Animator>();  // Get the Animator component
        FaceLeft();  // Ensure the enemy is always facing left initially
    }

    void Update()
    {
        // Calculate the distance between the enemy and the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // If the player is within detection range but outside attack range
        if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange)
        {
            // Gnome stays idle but detects the player
            animator.SetBool("isAttacking", false);  // Ensure attack animation is not playing yet
        }
        // If the player is within attack range, perform the attack animation
        else if (distanceToPlayer <= attackRange)
        {
            animator.SetBool("isAttacking", true);  // Play attack animation
            AttackPlayer();
        }
        // If the player is outside the detection range, idle
        else
        {
            animator.SetBool("isAttacking", false);  // Ensure gnome stays idle when the player is out of range
        }
    }

    // Attack the player (this can be extended with your attack logic)
    void AttackPlayer()
    {
        Debug.Log("Enemy is attacking the player!");
        // Add attack logic here (e.g., deal damage to player)
    }

    // Ensure the enemy is facing left
    void FaceLeft()
    {
        if (transform.localScale.x > 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);  // Flip to face left by ensuring the x scale is negative
            transform.localScale = scale;
        }
    }
}
