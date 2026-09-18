using UnityEngine;
using UnityEngine.InputSystem;

public class Walk : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 0.1f;
    public Transform playerCamera;

    private CharacterController controller;

    private float xRotation = 0f;
    private float verticalVelocity = 0f;

    public float gravity = -20f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // =========================
        // ХОДЬБА
        // =========================

        Vector2 input = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed)
                input.y += 1;

            if (Keyboard.current.sKey.isPressed)
                input.y -= 1;

            if (Keyboard.current.aKey.isPressed)
                input.x -= 1;

            if (Keyboard.current.dKey.isPressed)
                input.x += 1;
        }

        Vector3 move =
            transform.forward * input.y +
            transform.right * input.x;

        move = move.normalized * speed;


        // =========================
        // ГРАВИТАЦИЯ
        // =========================

        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;

        move.y = verticalVelocity;


        // Двигаем игрока через Character Controller
        controller.Move(move * Time.deltaTime);


        // =========================
        // МЫШЬ
        // =========================

        if (Mouse.current != null)
        {
            Vector2 mouse = Mouse.current.delta.ReadValue();

            float mouseX = mouse.x * mouseSensitivity;
            float mouseY = mouse.y * mouseSensitivity;

            // Влево / вправо
            transform.Rotate(Vector3.up * mouseX);

            // Вверх / вниз
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            playerCamera.localRotation =
                Quaternion.Euler(xRotation, 0f, 0f);
        }
    }
}