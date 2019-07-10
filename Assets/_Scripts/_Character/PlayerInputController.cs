using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float mouseSensitivity = 3f;

    private PlayerMotor motor;

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
    }

    private void Update()
    {
        CalculateMovement();
        CalculateRotation();
    }

    // Calculates Player Movement as Vector3
    private void CalculateMovement()
    {
        float xMovement = Input.GetAxisRaw("Horizontal");
        float yMovement = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * xMovement;
        Vector3 MoveVertical = transform.forward * yMovement;

        Vector3 _velocity = (moveHorizontal + MoveVertical).normalized * moveSpeed;

        motor.Move(_velocity);
    }

    // Calculate Rotation as Vector3
    private void CalculateRotation()
    {
        // this is for turning around
        float yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, yRot, 0f) * mouseSensitivity;

        motor.Rotate(_rotation);

        // this is for the camera rotation
        float xRot = Input.GetAxisRaw("Mouse Y");

        float _cameraRotationX = xRot * mouseSensitivity;

        motor.RotateCamera(_cameraRotationX);
    }
}
