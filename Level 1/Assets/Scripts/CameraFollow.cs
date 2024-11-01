using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform playerTransform; // Reference to the player's transform
    [SerializeField] private Vector3 offset; // Offset between the camera and player
    [SerializeField] private float smoothSpeed = 0.125f; // Smooth following speed
    [SerializeField] private float minX, maxX, minY, maxY; // Camera bounds

    private void LateUpdate()
    {
        // Compute the desired position based on the player's position and offset
        Vector3 desiredPosition = playerTransform.position + offset;

        // Smoothly interpolate the camera's position towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Clamp the camera's position within the min/max bounds
        transform.position = new Vector3(
            Mathf.Clamp(smoothedPosition.x, minX, maxX),
            Mathf.Clamp(smoothedPosition.y, minY, maxY),
            transform.position.z);

        // Ensure the camera stays upright by locking its rotation
        transform.rotation = Quaternion.identity;
    }

    // Method to snap the camera to the player's position instantly (for respawning)
    public void SnapToPlayer()
    {
        // Instantly move the camera to the player's position + offset
        Vector3 newPosition = playerTransform.position + offset;
        transform.position = new Vector3(
            Mathf.Clamp(newPosition.x, minX, maxX),
            Mathf.Clamp(newPosition.y, minY, maxY),
            transform.position.z);
    }
}
