using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiFPS
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMotor : MonoBehaviour
    {
        [SerializeField] private Camera cam;

        [SerializeField] private float cameraRotationLimit = 80f;


        private Vector3 rotation = Vector3.zero;
        private float cameraRotationX = 0f;
        private Vector3 velocity = Vector3.zero;
        private float currentCameraRotationX = 0f;

        private Rigidbody rb;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        // Gets the _velocity Vector3 from PlayerInputController
        public void Move(Vector3 _velocity)
        {
            velocity = _velocity;
        }

        // Gets the _rotation Vector3 from PlayerInputController
        public void Rotate(Vector3 _rotation)
        {
            rotation = _rotation;
        }

        // Gets the _cameraRotation Vector3 from PlayerInputController
        public void RotateCamera(float _cameraRotationX)
        {
            cameraRotationX = _cameraRotationX;
        }

        void FixedUpdate()
        {
            PerformMovement();
            PerformRotation();
        }

        // Perform the calculated Movement
        void PerformMovement()
        {
            if (velocity != Vector3.zero)
            {
                rb.MovePosition(rb.position + velocity * Time.deltaTime);
            }
        }

        // Perform the calculated Rotations
        void PerformRotation()
        {
            rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
            if (cam != null)
            {
                // Set rotation and clamp it 
                currentCameraRotationX -= cameraRotationX;
                currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

                // apply rotation to the transform of the camera
                cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
            }
        }
    }
}
