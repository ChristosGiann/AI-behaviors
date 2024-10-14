using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 8f; // Player movement speed
    public float mouseSensitivity = 100f; // Camera sensitivity
    public Transform playerBody; //Reference to player body

    float xRotation = 0f;

    void Start()
    {
        // Locking the mouse for movement purposes
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Camera rotation through mouse movement
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //Rotation restriction

        playerBody.Rotate(Vector3.up * mouseX);

        // Movement depending on player direction
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = playerBody.forward * moveZ + playerBody.right * moveX;
        transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);

        
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Unlocking mouse
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SceneManager.LoadScene("menu");
        }
    }
}
