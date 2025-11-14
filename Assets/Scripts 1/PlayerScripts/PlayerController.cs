using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody2D rb;

    // Reference to the Input Actions asset
    public InputActionAsset inputActions;
    private InputAction moveAction;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        if (inputActions != null)
        {
            moveAction = inputActions.FindActionMap("Player").FindAction("Move");
            moveAction.Enable();
        }
    }

    void Update()
    {
        // Read input from Input System (covers keyboard, gamepad, and joystick)
        if (moveAction != null)
            moveInput = moveAction.ReadValue<Vector2>();
        else
            moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    void FixedUpdate()
    {
        rb.velocity = moveInput.normalized * moveSpeed;
    }

    void OnDisable()
    {
        if (rb != null) rb.velocity = Vector2.zero;
        if (moveAction != null) moveAction.Disable();
    }
}