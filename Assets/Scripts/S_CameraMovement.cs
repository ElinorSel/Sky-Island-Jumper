using UnityEngine;
using UnityEngine.InputSystem; //dont forget this for imput system!s
public class S_CameraMovement : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset = new Vector3(0, 5, -7);
    public float rotateSpeed = 10.0f;
    private PlayerInputActions inputActions;
    private Vector2 lookInput;
    float xRotation = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }
    private void OnEnable()
    {
        inputActions.Player.Enable(); // Enable the entire action map!
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + offset;
        // Rotate the camera horizontally (yaw)
        transform.Rotate(Vector3.up, lookInput.x * rotateSpeed * Time.deltaTime);
        Debug.Log(lookInput.x);
    }
}
