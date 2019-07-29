using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiFPS
{
    [RequireComponent(typeof(PlayerMotor))]
    public class PlayerInputController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 15f;
        [SerializeField] private float mouseSensitivity = 3f;

        private PlayerMotor motor;

        void Start()
        {
            motor = GetComponent<PlayerMotor>();
        }

        void Update()
        {
			if(PauseMenu.IsOn) { return; }

            CalculateMovement();
            CalculateRotation();
        }

        // Calculates Player Movement as Vector3
        void CalculateMovement()
        {
            float xMovement = Input.GetAxis("Horizontal");
            float yMovement = Input.GetAxis("Vertical");

            Vector3 moveHorizontal = transform.right * xMovement;
            Vector3 MoveVertical = transform.forward * yMovement;

            Vector3 _velocity = (moveHorizontal + MoveVertical) * moveSpeed;

            motor.Move(_velocity);
        }

        // Calculate Rotation as Vector3
        void CalculateRotation()
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
}
