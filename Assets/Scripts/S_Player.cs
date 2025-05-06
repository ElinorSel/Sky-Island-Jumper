using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem; //dont forget this for imput system!s

public class S_Player : MonoBehaviour
{
    //initilize variables
    private Rigidbody playerRB;
    public float jumpForce;
    public float movementForce = 1.5f;
    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private int jumpCount;
    public int allowedJumps = 2;
    public bool grounded;
    public float stamina = 100;
    public float boostForce = 1.1f;

    public float staminaLoss = 20.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Get player components
        playerRB = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        //+= "subscribes to the action before it" aka when move is preformed run this function.
        // => is a quick way to write function in line. you can write the function traditionally
        // and runt it after += (like js arrow functions)
        // inputActions.Player.Move.performed += methodThatDoesStuff;
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero; //when move action not active, set to 0 movement
        inputActions.Player.Move.Enable(); // enable the action
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        Debug.Log(stamina);

    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
            grounded = true;
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
    }

    void PlayerMovement()
    {
        // TODO: Question.. this should be declared here because im only gon use it here not globally
        //Input from player's keypresses

        Vector3 direction = new Vector3(moveInput.x, 0, moveInput.y);

        //rotate player if there is input
        if (direction.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            float rotationSpeed = 15f; // adjust this value for faster/slower rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }


        // Jump (spacebar input)
        if (Keyboard.current.spaceKey.wasPressedThisFrame && (jumpCount < allowedJumps))
        {
            float jumpForceTemp = jumpForce;
            if (grounded == false && jumpCount == 0)
            {
                jumpCount++;
            }

            if (jumpCount > 0)
            {
                jumpForce /= 1.5f;
            }
            //neutralizing gravity before jump lol
            playerRB.linearVelocity = new Vector3(playerRB.linearVelocity.x, 0f, playerRB.linearVelocity.z);

            //apply jumping force!
            playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); //vertical movement
            jumpCount++;
            jumpForce = jumpForceTemp;
        }

        //Apply forces to move player
        //ForceMode changes how the force is applied, instantly? or slowly over time? etc
        playerRB.AddForce(direction * movementForce, ForceMode.VelocityChange);

        //boost!
        if (Keyboard.current.leftShiftKey.isPressed && (stamina > 0))
        {
            playerRB.AddForce(direction * boostForce, ForceMode.Impulse);
            stamina -= staminaLoss * Time.deltaTime;
            Debug.Log("shift press!");
        }
    }

}
