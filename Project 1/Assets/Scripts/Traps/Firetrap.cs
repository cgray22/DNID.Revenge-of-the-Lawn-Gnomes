using UnityEngine;
using System.Collections;

public class Firetrap : MonoBehaviour
{
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;
    [SerializeField] private float damage;
    private Animator anim;
    private SpriteRenderer spriteRend;

    private bool triggered;  // When the trap gets triggered
    private bool active;     // When the trap is active and can hurt the player

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!triggered)
            {
                StartCoroutine(ActivateFiretrap());
            }

            if (active)
            {
                collision.GetComponent<Health>().TakeDamage(damage);
            }
        }
    }

    private IEnumerator ActivateFiretrap()
    {
        triggered = true;
        spriteRend.color = Color.red;  // Turn the sprite red to notify the player
        yield return new WaitForSeconds(activationDelay);

        active = true;
        spriteRend.color = Color.white;  // Turn the sprite back to its initial color
        yield return new WaitForSeconds(activeTime);

        active = false;
        triggered = false;
    }
}
