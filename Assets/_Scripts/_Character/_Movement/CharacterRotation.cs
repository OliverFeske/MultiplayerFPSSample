using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRotation : MonoBehaviour
{
    Vector2 mouseLook;
    Vector2 smooth;

    [SerializeField] private float sensitivity = 1f;
    [SerializeField] private float smoothing = 2f;

    GameObject character;

    private void Start()
    {
        character = this.transform.parent.gameObject;
    }

    private void Update()
    {
        var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        smooth.x = Mathf.Lerp(smooth.x, mouseDelta.x, 1f / smoothing);
        smooth.y = Mathf.Lerp(smooth.y, mouseDelta.y, 1f / smoothing);
        mouseLook += smooth;

        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);
    }
}
