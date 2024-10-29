using UnityEngine;
using System.Collections;  // Add this directive for IEnumerator

public class Health : MonoBehaviour
{
    public float startingHealth = 3f;
    public float currentHealth;
    public bool isDead = false;

    public Animator anim;  // Public Animator reference
    public Behaviour[] components;  // Components to disable during death
    private PlayerRespawn playerRespawn;  // Reference to PlayerRespawn script

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        playerRespawn = GetComponent<PlayerRespawn>();  // Get reference to PlayerRespawn script
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        anim.SetTrigger("hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        isDead = true;
        anim.SetTrigger("die");
        GetComponent<PlayerMovement>().enabled = false;

        // Disable components on death (if needed)
        foreach (Behaviour component in components)
        {
            component.enabled = false;
        }

        // Trigger the respawn after a delay
        StartCoroutine(RespawnAfterDelay(2f));
    }

    public void Heal(float amount)
    {
        if (!isDead)
        {
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, startingHealth);
        }
    }

    private IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Use the PlayerRespawn component to move the player to the last checkpoint
        playerRespawn.Respawn();

        Respawn();
    }

    public void Respawn()
    {
        isDead = false;
        Heal(startingHealth);  // Restore health

        // Reset the death trigger and set the animator to the idle animation
        anim.ResetTrigger("die");
        anim.ResetTrigger("hurt");
        anim.SetTrigger("respawn");  // Trigger the transition to Idle

        StartCoroutine(Invulnerability());

        // Reactivate all attached component classes after respawn
        GetComponent<PlayerMovement>().enabled = true;
        foreach (Behaviour component in components)
        {
            component.enabled = true;
        }
    }


    private IEnumerator Invulnerability()
    {
        // Example of an Invulnerability Coroutine
        // Player is invulnerable for a short period after respawn
        Physics2D.IgnoreLayerCollision(10, 11, true);
        yield return new WaitForSeconds(2f);  // Set your invulnerability time
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }
}
