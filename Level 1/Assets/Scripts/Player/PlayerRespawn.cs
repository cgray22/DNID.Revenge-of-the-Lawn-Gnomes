using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 respawnPoint;
    private CameraFollow cameraFollow;

    private void Start()
    {
        // Set the initial respawn point to the player's starting position
        respawnPoint = transform.position;

        // Find and store reference to the CameraFollow script
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
    }

    // This method is called when the player collides with a checkpoint
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CheckPoint"))
        {
            Checkpoint checkpoint = other.GetComponent<Checkpoint>();
            if (checkpoint != null && !checkpoint.isActivated)
            {
                // Save the checkpoint position and activate it
                respawnPoint = other.transform.position;
                checkpoint.isActivated = true;  // Mark checkpoint as activated
                Debug.Log("New checkpoint reached, respawn point updated.");
            }
        }
    }

    public void Respawn()
    {
        // Move the player to the current respawn point
        transform.position = respawnPoint;
        Debug.Log("Player respawned.");

        // Snap the camera to the player's new position
        if (cameraFollow != null)
        {
            cameraFollow.SnapToPlayer();
        }
    }
}
