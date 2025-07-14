using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotationHandler : MonoBehaviour
{
    public Transform yawTarget;    // CameraRoot
    public Transform pitchTarget;  // CameraPitch

    public float sensitivity = 3f;
    public float minPitch = -40f;
    public float maxPitch = 80f;

    private float pitch = 0f;

    void Update()
    {
        Vector2 lookInput = Mouse.current != null
            ? Mouse.current.delta.ReadValue() * sensitivity
            : Gamepad.current?.rightStick.ReadValue() * (sensitivity * 20f) ?? Vector2.zero;

        // Yaw (left/right)
        yawTarget.Rotate(Vector3.up, lookInput.x * Time.deltaTime);

        // Pitch (up/down)
        pitch -= lookInput.y * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        pitchTarget.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}
