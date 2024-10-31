using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float movementRange;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float damageAmount;

    private bool isMovingLeft;
    private float leftBoundary;
    private float rightBoundary;

    private void Awake()
    {
        leftBoundary = transform.position.x - movementRange;
        rightBoundary = transform.position.x + movementRange;
    }

    private void Update()
    {
        if (isMovingLeft)
        {
            if (transform.position.x > leftBoundary)
            {
                transform.position = new Vector3(transform.position.x - movementSpeed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
            {
                isMovingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < rightBoundary)
            {
                transform.position = new Vector3(transform.position.x + movementSpeed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
            {
                isMovingLeft = true;
            }
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
                playerHealth.TakeDamage(damageAmount);
                Debug.Log("Damage applied to Player: " + damageAmount);
            }
            else
            {
                Debug.LogError("Health component missing on Player object.");
            }
        }
    }
}
