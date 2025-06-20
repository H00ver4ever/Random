using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, ProjectActions.IOverworldActions
{
    ProjectActions input;
    CharacterController cc;
    Camera mainCam;
    Animator anim;

    [Header("Collision Mask")]
    [SerializeField] private LayerMask collsionMask;
    [SerializeField] private Transform raycastOriginPoint;

    [Header("Movement Variables")]
    [SerializeField] private float initSpeed = 5.0f;
    [SerializeField] private float maxSpeed = 15.0f;
    [SerializeField] private float moveAccel = 0.2f;
    [SerializeField] private float rotationSpeed = 30.0f;
    private float curSpeed = 5.0f;

    [Header("Jump Variables")]
    [SerializeField] private float jumpHeight = 0.1f;
    [SerializeField] private float jumpTime = 0.7f;

    //values calculated using jump height and jump time
    private float timeToJumpApex; //JumpTime / 2
    private float initJumpVelocity;

    //weapon variables
    [Header("Weapon Variables")]
    [SerializeField] private Transform weaponAttachPoint;
    Weapon weapon = null;


    //Character Movement
    Vector2 direction;
    Vector3 velocity;

    //calculated based on our jump values - this is the Y velocity that we will apply
    private float gravity;

    private bool isJumpPressed = false;

    public int health;
    public bool collectedWeapon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        mainCam = Camera.main;

        timeToJumpApex = jumpTime / 2;
        gravity = (-2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        initJumpVelocity = -(gravity * timeToJumpApex);
    }

    private void OnValidate()
    {
        timeToJumpApex = jumpTime / 2;
        gravity = (-2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        initJumpVelocity = -(gravity * timeToJumpApex);
    }

    void OnEnable()
    {
        input = new ProjectActions();
        input.Enable();
        input.Overworld.SetCallbacks(this);
    }

    void OnDisable()
    {
        input.Disable();
        input.Overworld.RemoveCallbacks(this);
    }

    #region Input Functions
    public void OnJump(InputAction.CallbackContext context) => isJumpPressed = context.ReadValueAsButton();

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed) direction = context.ReadValue<Vector2>();
        if (context.canceled) direction = Vector2.zero;
    }

    public void OnDrop(InputAction.CallbackContext context)
    {
        if (weapon)
        {
            weapon.Drop(GetComponent<Collider>(), transform.forward);
            weapon = null;
        }
    }
    public void OnMoveAction(InputAction.CallbackContext context)
    {
        if (context.performed) direction = context.ReadValue<Vector2>();
        if (context.canceled) direction = Vector2.zero;
    }
    public void OnPunch(InputAction.CallbackContext context)
    {
        if (context.performed) anim.SetTrigger("Punch");
    }
    #endregion


    void Update()
    {
        Vector2 groundVel = new Vector2(velocity.x, velocity.z);
        anim.SetFloat("vel", groundVel.magnitude);

        if (!raycastOriginPoint)
            return;

        Ray ray = new Ray(raycastOriginPoint.transform.position, transform.forward);
        RaycastHit hitInfo;
        Debug.DrawLine(raycastOriginPoint.transform.position, raycastOriginPoint.transform.position + (transform.forward * 10.0f), Color.red);

        if (Physics.Raycast(ray, out hitInfo, 10.0f, collsionMask))
        {
            //if true hitInfo will have somethings to output
            Debug.Log(hitInfo.transform.gameObject);
        }
    }

    void FixedUpdate()
    {
        Vector3 desiredMoveDirection = ProjectedMoveDirection();
        cc.Move(UpdateCharacterVelocity(desiredMoveDirection));

        //rotate towards direction of movement
        if (direction.magnitude > 0)
        {
            float timeStep = rotationSpeed * Time.fixedDeltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), timeStep);
        }
    }

    private Vector3 ProjectedMoveDirection()
    {
        //grab our fwd and right vectors for camera relative movement
        Vector3 cameraForward = mainCam.transform.forward;
        Vector3 cameraRight = mainCam.transform.right;

        //remove yaw rotation
        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        return cameraForward * direction.y + cameraRight * direction.x;
    }

    private Vector3 UpdateCharacterVelocity(Vector3 desiredDirection)
    {
        if (direction == Vector2.zero) curSpeed = initSpeed;

        velocity.x = desiredDirection.x * curSpeed;
        velocity.z = desiredDirection.z * curSpeed;

        curSpeed += moveAccel * Time.fixedDeltaTime;
        curSpeed = Mathf.Clamp(curSpeed, initSpeed, maxSpeed);

        if (!cc.isGrounded) velocity.y += gravity * Time.fixedDeltaTime;
        else velocity.y = CheckJump();

        return velocity;
    }

    private float CheckJump()
    {
        if (isJumpPressed) return initJumpVelocity;
        else return -cc.minMoveDistance;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Weapon") && weapon == null)
        {
            weapon = hit.gameObject.GetComponent<Weapon>();
            weapon.Equip(GetComponent<Collider>(), weaponAttachPoint);
        }
    }

    public void SaveGamePrepare()
    {
        Debug.Log("whatishappenin");
        // Get Player Data Object
        LoadSavemanager.LevelStateData.DataPlayer data = GameManager.StateManager.levelState.player;

        // Fill in player data for save game
        data.collectedWeapon = collectedWeapon;
        data.health = health;

        data.posRotScale.posX = transform.position.x;
        data.posRotScale.posY = transform.position.y;
        data.posRotScale.posZ = transform.position.z;

        data.posRotScale.rotX = transform.localEulerAngles.x;
        data.posRotScale.rotY = transform.localEulerAngles.y;
        data.posRotScale.rotZ = transform.localEulerAngles.z;

        data.posRotScale.scaleX = transform.localScale.x;
        data.posRotScale.scaleY = transform.localScale.y;
        data.posRotScale.scaleZ = transform.localScale.z;

    }

    // Function called when loading is complete
    public void LoadGameComplete()
    {
        // Get Player Data Object
        LoadSavemanager.LevelStateData.DataPlayer data =
            GameManager.StateManager.levelState.player;

        // Load data back to Player
        health = data.health;


        // Give player weapon, activate and destroy weapon power-up
        if (data.collectedWeapon)
        {
            // Find weapon powerup in level
            GameObject weaponPowerUp = GameObject.Find("Weapon_PowerUp");

            // Send OnTriggerEnter message
            weaponPowerUp.SendMessage("OnTriggerEnter2D", GetComponent<Collider2D>(),
                SendMessageOptions.DontRequireReceiver);

        }

        // Set position
        transform.position = new Vector3(data.posRotScale.posX,
            data.posRotScale.posY, data.posRotScale.posZ);

        // Set rotation
        transform.localRotation = Quaternion.Euler(data.posRotScale.rotX,
            data.posRotScale.rotY, data.posRotScale.rotZ);

        // Set scale
        transform.localScale = new Vector3(data.posRotScale.scaleX,
            data.posRotScale.scaleY, data.posRotScale.scaleZ);

    }
    }
