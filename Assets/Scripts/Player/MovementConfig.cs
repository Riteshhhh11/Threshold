using UnityEngine;
[System.Serializable]
public class MovementConfig
{
    [Header("Player Settings")]
    public float moveSpeed = 5f;
    public float sprintSpeed = 8f;
    public float jumpHeight = 2f;
    public float jumpBufferTime = 0.05f;
    public float jumpBufferTimer = 0f;
    public float AirControlFactor = 0.4f;

    [Header("Gravity")]
    public float gravity = -20f;
    public float constGravity = -2f;

    [Header("Ground Check Settings")]
    //public float groundDistance;
    public float checkSphereRadius;

}
