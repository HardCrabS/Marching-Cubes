using UnityEngine;

public class ObjectRotation : MonoBehaviour
{
    // Adjust this value to control the rotation speed
    public float rotationSpeed = 15f;

    private bool isRotating;
    private Vector3 mouseStartPosition;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Record the mouse position when the left mouse button is pressed
            isRotating = true;
            mouseStartPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            // Stop rotating when the left mouse button is released
            isRotating = false;
        }

        if (isRotating)
        {
            // Calculate the mouse movement delta
            Vector3 mouseDelta = Input.mousePosition - mouseStartPosition;

            // Rotate the object based on the mouse movement
            float rotationX = mouseDelta.y * rotationSpeed * Time.deltaTime;
            float rotationY = -mouseDelta.x * rotationSpeed * Time.deltaTime;

            // Apply the rotation to the object
            transform.Rotate(rotationX, rotationY, 0f, Space.Self);

            // Update the mouse start position for the next frame
            mouseStartPosition = Input.mousePosition;
        }
    }
}
