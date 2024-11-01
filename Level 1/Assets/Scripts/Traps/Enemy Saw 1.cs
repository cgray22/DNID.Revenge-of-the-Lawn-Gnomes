using UnityEngine;

public class Enemy_Sideways : MonoBehaviour
{
    [SerializeField] private float damage;

    private void Start()
    {
        if (damage <= 0)
        {
            Debug.LogWarning("Damage value not assigned or is zero. Setting default damage to 1.");
            damage = 1f; // Set a default value if not assigned
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("Collision detected with Player"); // Debug log to confirm collision
            Health playerHealth = collision.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Damage applied to Player: " + damage);
            }
            else
            {
                Debug.LogError("Health component missing on Player object.");
            }
        }
    }
}
