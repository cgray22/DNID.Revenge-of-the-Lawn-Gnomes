using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    [SerializeField] private float healthValue = 10f;

    private void Start()
    {
        Debug.Log("HealthCollectible initialized with healthValue: " + healthValue);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Health playerHealth = collision.GetComponent<Health>();
            if (playerHealth != null)
            {
                if (healthValue > 0)
                {
                    playerHealth.Heal(healthValue);
                    Debug.Log("Player healed by: " + healthValue);
                }
                else
                {
                    Debug.LogWarning("Health value is not positive. No healing applied.");
                }
                gameObject.SetActive(false); // Disable the collectible instead of destroying immediately
            }
            else
            {
                Debug.LogError("Health component missing on Player object.");
            }
        }
    }
}
