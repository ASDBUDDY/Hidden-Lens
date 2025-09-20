using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMainScript : MonoBehaviour
{
    #region Variables
    [Header("Player Stats")]
    public PlayerStats MainStats;

    [Header("Layer Checks")]
    public Transform GroundCheckPos;
    [SerializeField]
    private Vector2 groundCheckSize = new Vector2(0.5f, 0.005f);
    public LayerMask GroundLayerMask;


    private Rigidbody2D playerRigidbody;
    private SpriteRenderer playerSpriteRenderer;
    private PlayerAnimatorScript playerAnimatorScript;

        #region Movement variables
        private float horizontalMovement = 0f;
        private float movementSmoothTime = 0.3f;
        private Vector2 velocityRef = Vector2.zero;

        private float timeLastPressedJump = 0f;
        private float timeSinceLastJump = 0f;
        private float lastTimeOnGround = 0f;
        private float coyoteTime = 0.2f;
        private float jumpBufferInterval = 0.1f; 
        private bool isHalfJump = false;
    #endregion
    public float VelocityY;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimatorScript = GetComponent<PlayerAnimatorScript>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandlePlayerMovement();
    }
    #endregion

    #region OnReturn Methods

    public bool isGrounded()
    {
        if(Physics2D.OverlapBox(GroundCheckPos.position, groundCheckSize, 0, GroundLayerMask))
        {
            return true;
        }
        return false;
    }

    #endregion

    #region Movement Functions
    public void MoveFunction(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }
    private void HandlePlayerMovement()
    {
        if (playerRigidbody != null)
        {
            if (horizontalMovement < 0f)
            {
                transform.rotation = Quaternion.Euler(transform.position.x, 180, transform.position.z);

            }
            else if (horizontalMovement > 0f) 
            {
                transform.rotation = Quaternion.Euler(transform.position.x, 0, transform.position.z);
            }
                playerRigidbody.velocity = Vector2.SmoothDamp(playerRigidbody.velocity, new Vector2(horizontalMovement * MainStats.PlayerSpeed, playerRigidbody.velocity.y), ref velocityRef, movementSmoothTime);
            
            playerAnimatorScript.SetVelocity(Mathf.Clamp01(Mathf.Abs(playerRigidbody.velocity.x)), playerRigidbody.velocity.y);

            GroundCheck();
            if(isGrounded() && Time.time - timeLastPressedJump <= jumpBufferInterval)
            {
                
                PerformJump(isHalfJump);
            }

            VelocityY = playerRigidbody.velocity.y;
        }
    }
    private void GroundCheck()
    {
        if (isGrounded())
        {
            lastTimeOnGround = Time.time;
            if(playerRigidbody.velocity.y <= 0f)
                playerAnimatorScript.SetJump(false);
        }
    }

    public void JumpFunction(InputAction.CallbackContext context)
    {
        timeLastPressedJump = Time.time;
        isHalfJump = context.performed ? false : context.canceled ? true : false;

        if (isGrounded() || Time.time - lastTimeOnGround <= coyoteTime)
        {

            PerformJump(isHalfJump);
            
        }
    }

    private void PerformJump(bool isHalf = false)
    {
        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, MainStats.PlayerJumpSpeed * (isHalf ? 0.5f : 1f));
        timeLastPressedJump = Mathf.NegativeInfinity;
        timeSinceLastJump = Time.time;
        playerAnimatorScript.SetJump();
    }
    #endregion


    /*private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(GroundCheckPos.position, groundCheckSize);
    }*/
}

[System.Serializable]
public class PlayerStats
{
    public float PlayerSpeed;
    public float PlayerJumpSpeed;

}