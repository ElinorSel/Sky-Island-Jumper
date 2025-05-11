using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem; //dont forget this for imput system!s

public class S_Player : MonoBehaviour
{
    //initilize variables
    private Rigidbody playerRB;
    public GameObject playerPrefab;

    public ParticleSystem jumpParticle; //TODO: particle play when run
    public float jumpForce;
    public float moveSpeed = 1.5f;
    public float baseMoveSpeed = 1.5f;
    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private int jumpCount;
    public int allowedJumps = 2;
    public bool grounded;
    public float stamina = 100;
    public float boostSpeed = 10f;
    public float maxSpeed = 5f;
    public float gravity = 10f;

    public float staminaLoss = 20.0f;
    public int deathCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Get player components
        playerRB = GetComponent<Rigidbody>();
        var main = jumpParticle.main;
        main.simulationSpeed = 8f;


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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Death"))
        {
            Respawn();
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
            if (grounded == false && jumpCount == 0)
            {
                jumpCount++;
            }

            if (jumpCount > 0)
            {
                jumpParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                jumpParticle.Play();
            }
            //neutralizing gravity before jump lol
            playerRB.linearVelocity = new Vector3(playerRB.linearVelocity.x, 0f, playerRB.linearVelocity.z);

            //apply jumping force!
            playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); //vertical movement
            jumpCount++;
        }

        //boost!
        if (Keyboard.current.leftShiftKey.isPressed && (stamina > 0))
        {
            moveSpeed = boostSpeed;
            stamina -= staminaLoss * Time.deltaTime;
        }
        else
        {
            moveSpeed = baseMoveSpeed;
        }

        Vector3 move = direction.normalized * moveSpeed * Time.fixedDeltaTime;


        playerRB.MovePosition(playerRB.position + move);


        //Apply forces to move player
        //ForceMode changes how the force is applied, instantly? or slowly over time? etc



        if (!grounded)
        {
            playerRB.AddForce(Vector3.down * gravity); // tweak value
        }

    }

    void Respawn()
    {
        deathCount++; //TODO: connect this so it shows in the UI
        transform.position = new Vector3(0, 0, 0);
    }

}
