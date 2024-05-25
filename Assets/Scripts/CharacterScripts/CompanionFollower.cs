using UnityEngine;

public class CompanionFollower : MonoBehaviour
{
    public Transform playerHead;
    public Vector3 offset = new Vector3(-0.5f, 0.5f, 0.5f); // Initial offset
    public float smoothSpeed = 0.1f; // Speed of smoothing
    public float noiseScale = 0.5f; // Scale for Perlin noise
    public float noiseFrequency = 1.0f; // Frequency of the Perlin noise changes
    public float gazeThreshold = 15f; // Angle threshold to stop moving
    public float minDistance = 1.0f; // Minimum distance from the player
    public float maxAngle = 45f; // Maximum angle from forward to left in degrees
    public float minForwardAngle = 20f; // Minimum angle to avoid directly forward

    private Vector3 targetPosition;
    private Vector3 noiseOffset;
    private float timeCounter = 0;

    void Start()
    {
        noiseOffset = new Vector3(Random.value * 10, Random.value * 10, Random.value * 10);
    }

    void LateUpdate()
    {
        if (playerHead != null)
        {
            // Calculate the direction to the companion
            Vector3 directionToCompanion = transform.position - playerHead.position;
            // Calculate the angle between the player's forward direction and the direction to the companion
            float angleToCompanion = Vector3.Angle(playerHead.forward, directionToCompanion);

            // Calculate the distance to the companion
            float distanceToCompanion = Vector3.Distance(playerHead.position, transform.position);

            // If the angle is within the gaze threshold, don't move the companion
            if (angleToCompanion > gazeThreshold && distanceToCompanion >= minDistance)
            {
                // Base desired position
                Vector3 desiredPosition = playerHead.position + playerHead.rotation * offset;

                // Perlin noise based movement
                float noiseX = Mathf.PerlinNoise(timeCounter, noiseOffset.x) - 0.5f;
                float noiseY = Mathf.PerlinNoise(noiseOffset.y, timeCounter) - 0.5f;
                float noiseZ = Mathf.PerlinNoise(noiseOffset.z, timeCounter) - 0.5f;

                Vector3 noise = new Vector3(noiseX, noiseY, noiseZ) * noiseScale;

                // Final target position with noise
                targetPosition = desiredPosition + noise;

                // Ensure the target position maintains the minimum distance from the player
                Vector3 directionFromPlayer = (targetPosition - playerHead.position).normalized;
                float targetDistance = Vector3.Distance(playerHead.position, targetPosition);

                if (targetDistance < minDistance)
                {
                    targetPosition = playerHead.position + directionFromPlayer * minDistance;
                }

                // Ensure the companion stays within the valid angle range
                Vector3 leftLimit = Quaternion.Euler(0, -maxAngle, 0) * playerHead.forward;
                Vector3 forwardLimit = Quaternion.Euler(0, minForwardAngle, 0) * playerHead.forward;

                float angleToLeft = Vector3.Angle(leftLimit, targetPosition - playerHead.position);
                float angleToForward = Vector3.Angle(playerHead.forward, targetPosition - playerHead.position);

                if (angleToLeft > maxAngle)
                {
                    targetPosition = playerHead.position + leftLimit * targetDistance;
                }
                else if (angleToForward < minForwardAngle)
                {
                    targetPosition = playerHead.position + forwardLimit * targetDistance;
                }

                // Smoothly interpolate to the target position
                transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

                // Increment time for the noise function
                timeCounter += Time.deltaTime * noiseFrequency;
            }
        }
    }
}
