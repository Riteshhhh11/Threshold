using UnityEngine;
[System.Serializable]
public class MovementConfig
{
    [Header("Player Settings")]
    public float moveSpeed = 5f;
    public float jumpHeight = 2f;

    [Header("Gravity")]
    public float gravity = -20f;

    [Header("Ground Check Settings")]
    public float groundDistance;
    public float checkSphereRadius;

}
