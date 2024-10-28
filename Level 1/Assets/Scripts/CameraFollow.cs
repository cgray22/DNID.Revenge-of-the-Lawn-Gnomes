using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform playerTransform; // Reference to the player's transform
    [SerializeField] private Vector3 offset; // Offset between the camera and player
    [SerializeField] private float smoothSpeed = 0.125f; // Smooth following speed

    private void LateUpdate()
    {
        // Compute the desired position based on the player's position and offset
        Vector3 desiredPosition = playerTransform.position + offset;

        // Smoothly interpolate the camera's position towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Ensure the camera stays upright by locking its rotation
        transform.rotation = Quaternion.identity;
    }
}
