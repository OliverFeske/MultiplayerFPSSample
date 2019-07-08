using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private void Start()
    {
        // Start the game without Cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // sort it by Input, Movement, Jump,(maybe Input for actions)

        float translation = Input.GetAxis("Vertical") * moveSpeed;
        float strafe = Input.GetAxis("Horizontal") * moveSpeed;
        translation *= Time.deltaTime;
        strafe *= Time.deltaTime;

        transform.Translate(strafe, 0f, translation);
    }

}
