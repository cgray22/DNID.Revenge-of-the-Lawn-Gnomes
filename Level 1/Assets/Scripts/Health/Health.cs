using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("Animator component missing on player object.");
        }
    }

    public void TakeDamage(float _damage)
    {
        Debug.Log("TakeDamage called with damage: " + _damage); // Debug log to ensure this method is called
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        Debug.Log("Current Health after damage: " + currentHealth);

        if (currentHealth > 0)
        {
            if (anim != null)
            {
                anim.SetTrigger("hurt");
                Debug.Log("Hurt animation triggered.");
            }
            // Implement invincibility frames here if needed
        }
        else
        {
            if (!dead)
            {
                if (anim != null)
                {
                    anim.SetTrigger("die");
                    Debug.Log("Die animation triggered.");
                }
                PlayerMovement playerMovement = GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    playerMovement.enabled = false;
                    Debug.Log("Player movement disabled.");
                }
                else
                {
                    Debug.LogError("PlayerMovement component missing on player object.");
                }
                dead = true;
            }
        }
    }

    public void Heal(float _amount)
    {
        if (!dead)
        {
            currentHealth = Mathf.Clamp(currentHealth + _amount, 0, startingHealth);
            Debug.Log("Player healed: " + _amount + ", Current Health: " + currentHealth);
        }
    }
    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

}