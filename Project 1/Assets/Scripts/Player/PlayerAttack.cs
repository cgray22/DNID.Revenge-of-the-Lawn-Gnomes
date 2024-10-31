using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 1f;    // Cooldown time between attacks
    [SerializeField] private float attackRange = 1f;       // Range for detecting enemies
    [SerializeField] private int attackDamage = 10;        // Damage dealt to enemies
    [SerializeField] private LayerMask enemyLayer;         // Layer mask for detecting enemies

    private Animator anim;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        anim = GetComponent<Animator>();   // Get the Animator component for triggering animations
    }

    private void Update()
    {
        // Increase the cooldown timer over time
        cooldownTimer += Time.deltaTime;

        // Detect mouse input and trigger attack if cooldown is over
        if (Input.GetMouseButtonDown(0) && cooldownTimer > attackCooldown)
        {
            Attack();
        }
    }

    private void Attack()
    {
        // Trigger the attack animation
        anim.SetTrigger("Attack");

        // Reset the cooldown timer
        cooldownTimer = 0;

        // Detect enemies within attack range using OverlapCircleAll
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

        // Loop through detected enemies and apply damage
        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);  // Apply damage to the enemy
                Debug.Log("Damaged enemy: " + enemy.name);  // Debug log to check damage applied
            }
        }
    }

    // Draw the attack range in the editor to visualize the area of attack
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
