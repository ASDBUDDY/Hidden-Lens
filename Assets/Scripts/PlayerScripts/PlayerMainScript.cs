using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMainScript : MonoBehaviour
{
    #region Variables
    [Header("Player Stats")]
    public PlayerStats MainStats;

    [Header("Layer Checks")]
    //Ground Layer
    public Transform GroundCheckPos;
    private Vector2 groundCheckSize = new Vector2(0.58f, 0.12f);
    public LayerMask GroundLayerMask;
    //Wall Layers
    public Transform WallCheckPos;
    [SerializeField]
    private Vector2 wallCheckSize = new Vector2(0.58f, 0.12f);
    public LayerMask WallLayerMask;

    [Header("Gravity Params")]
    public float BaseGravity;
    public float GravityMultiplier;
    public float HangGravityMultiplier;

    private Rigidbody2D playerRigidbody;
    private SpriteRenderer playerSpriteRenderer;
    private PlayerAnimatorScript playerAnimatorScript;

        #region Movement variables
        //Movement
        private float horizontalMovement = 0f;
        private float movementSmoothTime = 0.3f;
        private Vector2 velocityRef = Vector2.zero;

        //Jump variables
        private float timeLastPressedJump = 0f;
        private float timeSinceLastJump = 0f;
        private float lastTimeOnGround = 0f;
        private float coyoteTime = 0.4f;
        private float jumpBufferInterval = 0.1f; 
        private bool isHalfJump = false;
        private bool isWallJumping = false;
        private bool isJumping = false;
    [SerializeField]
    private float wallJumpTimer = 0f;

    //Sliding variabls
    private bool isWallSliding = false;
    private float wallSlidingTimer = 0f;
    private float wallSlidingCooldown =0.2f;
    #endregion

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

    public bool IsGrounded()
    {
        if(Physics2D.OverlapBox(GroundCheckPos.position, groundCheckSize, 0, GroundLayerMask))
        {
            return true;
        }
        return false;
    }
    public bool IsOnWall()
    {
        if (Physics2D.OverlapBox(WallCheckPos.position, wallCheckSize, 0, WallLayerMask))
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
                playerRigidbody.velocity = Vector2.SmoothDamp(playerRigidbody.velocity, new Vector2(horizontalMovement * MainStats.PlayerSpeed *(IsGrounded() ? 1f:0.8f), playerRigidbody.velocity.y), ref velocityRef, movementSmoothTime * (isWallJumping ? 2f:1f ));

                playerAnimatorScript.SetVelocity(Mathf.Clamp01(Mathf.Abs(playerRigidbody.velocity.x)), playerRigidbody.velocity.y);
            

            GroundCheck();
            ProcessWallSlide();
            ProcessWallJump();
            if(IsGrounded() && Time.time - timeLastPressedJump <= jumpBufferInterval)
            {
                
                PerformJump(isHalfJump);
            }
            GravityFunctionality();
           
        }
    }

    private void GravityFunctionality()
    {
        if (playerRigidbody != null)
        {
            if (playerRigidbody.velocity.y < -MainStats.PlayerJumpHangTime && !isWallSliding)
            {
                playerRigidbody.gravityScale = BaseGravity * GravityMultiplier;
                playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, Mathf.Max(playerRigidbody.velocity.y, -MainStats.PlayerMaxFallSpeed));
            }
            else if ((isJumping ||isWallJumping ) && Mathf.Abs(playerRigidbody.velocity.y) < MainStats.PlayerJumpHangTime)
            {
                playerRigidbody.gravityScale = BaseGravity * HangGravityMultiplier;
            }
            else
            {
                playerRigidbody.gravityScale = BaseGravity;
            }
        }
    }
    private void ProcessWallSlide()
    {

        if (!IsGrounded() && IsOnWall() && horizontalMovement!=0 && wallSlidingTimer <= 0f)
        {
            isWallSliding = true;
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, Mathf.Max(playerRigidbody.velocity.y, -MainStats.PlayerWallSlideSpeed));
            
        }
        else
        {
            if (isWallSliding)
            {
                wallSlidingTimer = wallSlidingCooldown;
            }
            isWallSliding = false;
        }

        if(wallSlidingTimer > 0f) 
            wallSlidingTimer -= Time.deltaTime;

        playerAnimatorScript.SetWallSide(isWallSliding);
    }
    private void ProcessWallJump()
    {
        if (isWallSliding || IsOnWall() && !isWallJumping)
        {
            isWallJumping = false;
            wallJumpTimer = MainStats.PlayerWallJumpTime;

            CancelInvoke(nameof(CancelWallJump));
        }
        else if (wallJumpTimer > 0f)
        {
            wallJumpTimer -= Time.deltaTime;
            
        }
    }
    private void CancelWallJump()
    {
        isWallJumping = false;
    }
    private void GroundCheck()
    {
        if (IsGrounded())
        {
            lastTimeOnGround = Time.time;
            if(playerRigidbody.velocity.y <= 0.1f)
                playerAnimatorScript.SetJump(false);
            isJumping = false;
        }
    }

    public void JumpFunction(InputAction.CallbackContext context)
    {

         timeLastPressedJump = Time.time;
         isHalfJump = context.performed ? false : context.canceled ? true : false;

        if (Time.time - timeSinceLastJump > 0.5f)
        {
            if (IsGrounded() || Time.time - lastTimeOnGround <= coyoteTime)
            {

                PerformJump(isHalfJump);
                
            }

            if (context.performed && wallJumpTimer > 0f)
            {
                

                isWallJumping = true;
                float direction = transform.rotation.eulerAngles.y == 180f ? 1 : -1;
               
                playerRigidbody.velocity = new Vector2(MainStats.PlayerWallJumpSpeed * direction, MainStats.PlayerJumpSpeed);
                transform.rotation = Quaternion.Euler(transform.position.x, direction == 1 ? 0 : 180, transform.position.z);
                wallJumpTimer = 0f;


                playerAnimatorScript.SetJump(true);
                Invoke(nameof(CancelWallJump), MainStats.PlayerWallJumpTime + 0.1f);
                timeSinceLastJump = Time.time;
            }
        }
    }

   
    private void PerformJump(bool isHalf = false)
    {

        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, MainStats.PlayerJumpSpeed * (isHalf ? 0.5f : 1f));
        timeLastPressedJump = Mathf.NegativeInfinity;
        timeSinceLastJump = Time.time;
        playerAnimatorScript.SetJump();
        isJumping = true;
    }
    #endregion


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(GroundCheckPos.position, groundCheckSize);
        Gizmos.DrawWireCube(WallCheckPos.position, wallCheckSize);
    }
}

[System.Serializable]
public class PlayerStats
{
    public float PlayerSpeed;
    public float PlayerJumpSpeed;
    public float PlayerWallSlideSpeed;
    public float PlayerWallJumpTime;
    public float PlayerWallJumpSpeed;
    public float PlayerMaxFallSpeed;
    public float PlayerJumpHangTime;

}