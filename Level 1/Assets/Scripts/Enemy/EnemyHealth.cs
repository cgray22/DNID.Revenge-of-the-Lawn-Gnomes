using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;          // Enemy's max health
    private int currentHealth;
    private Animator anim;              // Reference to the Animator
    private bool isDead = false;        // Flag to check if the enemy is dead

    private void Start()
    {
        currentHealth = maxHealth;      // Set current health to max at the start
        anim = GetComponent<Animator>(); // Get the Animator component
    }

    // Method to handle taking damage
    public void TakeDamage(int damage)
    {
        if (isDead) return; // If the enemy is already dead, do nothing

        currentHealth -= damage;        // Reduce health by the damage amount

        // Trigger the hurt animation if still alive
        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
        }

        // If health falls to 0 or below, trigger the death
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method to handle the enemy's death
    private void Die()
    {
        if (isDead) return;             // Ensure this only runs once
        isDead = true;                  // Set the isDead flag

        // Trigger the death animation
        anim.SetTrigger("die");

        // Disable the enemy's collider to prevent further interaction
        GetComponent<Collider2D>().enabled = false;

        // Optionally, destroy the enemy after the death animation plays
        Destroy(gameObject, 2f); // Adjust time based on your die animation length
    }
}
