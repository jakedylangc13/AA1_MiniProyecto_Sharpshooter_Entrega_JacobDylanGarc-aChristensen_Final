using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    public Vector2 MoveInput { get; private set; }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        MoveInput = playerInput.actions["Move"].ReadValue<Vector2>();
      
    }
}