using UnityEngine;

public class S_Player : MonoBehaviour
{
    //initilize variables
    private Rigidbody playerRB;
    public float jumpForce;
    public float movementForce = 1.5f;
    private PlayerInputActions inputActions;

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
        // TODO: Question.. this should be declared here because im only gon use it here not globally
        //Input from player's keypresses

        Vector3 direction = new Vector3(moveInput.x, moveInput.y);
        
        // Jump (spacebar input)
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); //vertical movement
        }

        //Apply forces to move player
        //ForceMode changes how the force is applied, instantly? or slowly over time? etc
        playerRB.AddForce(direction * movementForce, ForceMode.Impulse);


    }
}
