using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float baseSpeed; // The original base speed without power-ups
    public float speed;
    public float rotationSpeed;
    public float jumpSpeed;
    public float multiplier;
    public float powerUpDuration = 60f; // Duration of each power-up in seconds

    private CharacterController characterController;
    private float ySpeed;
    private float originalStepOffset;
    private bool isSpeedUpActive = false;
    private bool isJumpUpActive = false;
    private float speedUpTimer = 0f;
    private float jumpUpTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;
        speed = baseSpeed; // Initialize the speed with the base speed
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        float magnitude = Mathf.Clamp01(movementDirection.magnitude) * speed;
        movementDirection.Normalize();

        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (characterController.isGrounded)
        {
            characterController.stepOffset = originalStepOffset;
            ySpeed = -0.5f;

            if (Input.GetButtonDown("Jump"))
            {
                ySpeed = jumpSpeed;
            }
        }
        else
        {
            characterController.stepOffset = 0;
        }

        // Check if power-ups are active and update the timers
        if (isSpeedUpActive)
        {
            speedUpTimer -= Time.deltaTime;
            if (speedUpTimer <= 0f)
            {
                // Speed Up power-up duration expired, reset the effect
                speed = baseSpeed; // Reset the speed to the base speed
                isSpeedUpActive = false;
            }
        }
        if (isJumpUpActive)
        {
            jumpUpTimer -= Time.deltaTime;
            if (jumpUpTimer <= 0f)
            {
                // Jump Up power-up duration expired, reset the effect
                jumpSpeed = baseSpeed; // Reset the jump speed to the base speed
                isJumpUpActive = false;
            }
        }

        Vector3 velocity = movementDirection * magnitude;
        velocity.y = ySpeed;

        characterController.Move(velocity * Time.deltaTime);

        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }  // End of Update method

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Speed Up"))
        {
            if (!isSpeedUpActive)
            {
                speed = baseSpeed * multiplier; // Multiply the base speed by the multiplier
                isSpeedUpActive = true;
                speedUpTimer = powerUpDuration;
            }
        }
        if (other.CompareTag("Jump Up"))
        {
            if (!isJumpUpActive)
            {
                jumpSpeed = baseSpeed * multiplier; // Multiply the base speed by the multiplier
                isJumpUpActive = true;
                jumpUpTimer = powerUpDuration;
            }
        }
    }
}
