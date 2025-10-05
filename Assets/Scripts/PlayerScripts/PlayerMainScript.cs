using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMainScript : MonoBehaviour
{
    #region Variables

    [Header("DEBUG Abilities")]
    [SerializeField]
    private bool dashUnlocked = false;
    [SerializeField]
    private bool wallSlideUnlocked = false;


    [Header("Player Stats")]
    public PlayerStats MainStats;

    [Header("Player Health")]
    public GameObject BloodVFX;
    private HealthComponent playerHealth;
    public GameObject DeathSmoke;

    [Header("Layer Checks")]
    //Ground Layer
    public Transform GroundCheckPos;
    private Vector2 groundCheckSize = new Vector2(0.58f, 0.12f);
    public Transform LedgeCheckPos;
    public Vector2 ledgeCheckSize = new Vector2(0.58f, 0.12f);
    public Transform SecondLedgeCheckPos;
    public Vector2 secondLedgeCheckSize = new Vector2(0.58f, 0.12f);
    public LayerMask GroundLayerMask;
    //Wall Layers
    public Transform WallCheckPos;
    [SerializeField]
    private Vector2 wallCheckSize = new Vector2(0.58f, 0.12f);
    public LayerMask WallLayerMask;

    //Attack Layers
    public Transform AttackCheckPos;
    [SerializeField]
    private Vector2 attackCheckSize = new Vector2(0.58f, 0.12f);
    public LayerMask AttackLayerMask;

    

    [Header("Gravity Params")]
    public float BaseGravity;
    public float GravityMultiplier;
    public float HangGravityMultiplier;

    [Header("Crouch Params")]
    public float OriginalYSize = 0.3785404f;
    public float OriginalYPos = -0.02223387f;
    public float CrouchYSize = 0.23f;
    public float CrouchYPos = -0.1f;

    [Header("Dash Trail")]
    public DashTrail DashSystem;

    private Vector2 pauseVelocity = Vector2.zero;
    private BoxCollider2D playerCollider;
    private Rigidbody2D playerRigidbody;
    private SpriteRenderer playerSpriteRenderer;
    private PlayerAnimatorScript playerAnimatorScript;
    private PlayerAudioScript playerAudioScript;

        #region Movement variables
        //Movement
        private float horizontalMovement = 0f;
        private float movementSmoothTime = 0.3f;
        private Vector2 velocityRef = Vector2.zero;

        //Jump variables
        private float timeLastPressedJump = 0f;
        private float timeSinceLastJump = 0f;
        private float lastTimeOnGround = 0f;
        private float coyoteTime = 0.3f;
        private float jumpBufferInterval = 0.1f; 
        private bool isHalfJump = false;
        private bool isWallJumping = false;
        private bool isInAir = false;
        private bool isJumping = false;
        private float wallJumpTimer = 0f;


    //Dash variables
    private bool canDash = true;
    private bool isDashing = false;
    private Coroutine DashRoutine;

    //Sliding variables
    private bool isWallSliding = false;
    private float wallSlidingTimer = 0f;
    private float wallSlidingCooldown =0.1f;

    //Crouch variables
    private bool isCrouching = false;

    //Grabbing variables
    private bool isGrabbing = false;
    #endregion

    #region Attack variables
    private float attackTimer = 0f;
    private bool isAttacking = false;

    #endregion

    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimatorScript = GetComponent<PlayerAnimatorScript>();
        playerCollider = GetComponent<BoxCollider2D>();  
        playerAudioScript = GetComponent<PlayerAudioScript>();
    }
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = new HealthComponent(30f);
        HPDialUI.Instance.SetupHealth(playerHealth.MaxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeManager.Instance.TimePaused)
            return; 

        HandlePlayerMovement();
        AttackUpdation();
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
        if (playerHealth.IsDead || TimeManager.Instance.TimePaused)
        {
            
                horizontalMovement = 0f;

            return;
        }
       

        horizontalMovement = context.ReadValue<Vector2>().x;
    }
    private void HandlePlayerMovement()
    {
        if (playerRigidbody != null)
        {
            if (!isAttacking && !isGrabbing && !isDashing)
            {
                

                if (horizontalMovement < 0f)
                {
                    transform.rotation = Quaternion.Euler(transform.position.x, 180, transform.position.z);

                }
                else if (horizontalMovement > 0f)
                {
                    transform.rotation = Quaternion.Euler(transform.position.x, 0, transform.position.z);
                }

                

                if (!isCrouching)
                    playerRigidbody.velocity = Vector2.SmoothDamp(playerRigidbody.velocity, new Vector2(horizontalMovement * MainStats.PlayerSpeed * (IsGrounded() ? 1f : 0.8f), playerRigidbody.velocity.y), ref velocityRef, movementSmoothTime * (isWallJumping ? 2f : 1f));
            }
                playerAnimatorScript.SetVelocity(Mathf.Clamp01(Mathf.Abs(playerRigidbody.velocity.x)), playerRigidbody.velocity.y);
            

            GroundCheck();
            ProcessLedgeGrab();
            ProcessWallSlide();
            ProcessWallJump();
            if(IsGrounded() && TimeManager.Instance.TimeInSeconds - timeLastPressedJump <= jumpBufferInterval)
            {
                
                PerformJump(isHalfJump);
            }
            GravityFunctionality();
           
        }
    }

    private void GravityFunctionality()
    {
        if (playerRigidbody != null && !isGrabbing && !isDashing)
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

            if (TimeManager.Instance.TimePaused)
            {
                playerRigidbody.velocity = Vector2.zero;
                playerRigidbody.gravityScale = 0f;
            }
        }
    }
    private void ProcessWallSlide()
    {
        if (!wallSlideUnlocked)
            return;

        if (!IsGrounded() && IsOnWall() && horizontalMovement!=0 && wallSlidingTimer <= 0f)
        {
            isWallSliding = true;
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, Mathf.Max(playerRigidbody.velocity.y, -MainStats.PlayerWallSlideSpeed));

            if (isAttacking)
            {
                ResetAttack();
            }
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
            wallSlidingTimer -= TimeManager.Instance.DeltaTime;

        playerAnimatorScript.SetWallSide(isWallSliding);
    }
    private void ProcessWallJump()
    {
        if(!wallSlideUnlocked)
            return;

        if (isWallSliding || IsOnWall() && !isWallJumping)
        {
            isWallJumping = false;
            wallJumpTimer = MainStats.PlayerWallJumpTime;

            CancelInvoke(nameof(CancelWallJump));
        }
        else if (wallJumpTimer > 0f)
        {
            wallJumpTimer -= TimeManager.Instance.DeltaTime;
            
        }
    }
    private void CancelWallJump()
    {
        isWallJumping = false;
    }

    private void ProcessLedgeGrab()
    {
        if (isGrabbing)
        {
            

            if (horizontalMovement > 0f)
            {
                ResetGrab(transform.rotation.eulerAngles.y == 0f,true);
            }
            else if (horizontalMovement < 0f)
            {
                ResetGrab(transform.rotation.eulerAngles.y == 180f,false);
            }
            
        }
        else
        {
            if (isWallSliding)
                return;

            if (Physics2D.OverlapBox(LedgeCheckPos.position, ledgeCheckSize, 0, GroundLayerMask))
            {
                if (Physics2D.OverlapBox(SecondLedgeCheckPos.position, secondLedgeCheckSize, 0, GroundLayerMask))
                {
                    return;
                }
                else
                {
                    SetupGrab();
                }
            }
        }
    }

    public void ExecuteDash(InputAction.CallbackContext context)
    {
        if (!dashUnlocked)
            return;

        if (context.performed && canDash && !isGrabbing && !isWallSliding)
        {
            if(isAttacking)
                ResetAttack();

            if (DashRoutine != null)
            {
                StopCoroutine(DashRoutine);
            }
            playerAudioScript.PlayDash();
            DashRoutine = StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        canDash = false;
        isDashing = true;
        DashSystem.SetEnabled(true);

        playerRigidbody.gravityScale = 0f;

        playerAnimatorScript.SetDash();

        float dashDirection = transform.rotation.eulerAngles.y == 180? -1f :1f;

        playerRigidbody.velocity = new Vector2(dashDirection * MainStats.PlayerDashSpeed, 0f);

        yield return new CustomWaitForSeconds(MainStats.PlayerDashDuration);

        playerRigidbody.velocity = new Vector2(0f, playerRigidbody.velocity.y);
        playerRigidbody.gravityScale = TimeManager.Instance.TimePaused ? 0f:BaseGravity;
        isDashing = false;
        playerAnimatorScript.SetDash(false);
        DashSystem.SetEnabled(false);

        yield return new CustomWaitForSeconds(MainStats.PlayerDashCooldown);
            canDash = true;
    }
    private void SetupGrab()
    {
        isGrabbing = true;
        playerRigidbody.velocity = Vector3.zero;
        playerRigidbody.gravityScale = 0f;
        playerAnimatorScript.SetGrab(isGrabbing);
        playerAudioScript.PlayJumpSound();
    }
    private void ResetGrab(bool moveUp = false, bool directionLeft =false)
    {
        isGrabbing = false;
        playerAnimatorScript.SetGrab(isGrabbing);
        if (moveUp) {
            if(directionLeft)
            transform.position = new Vector2(transform.position.x + (0.15f * transform.localScale.x), transform.position.y + 1.8f);
            else
                transform.position = new Vector2(transform.position.x - (0.15f * transform.localScale.x), transform.position.y + 1.8f);

            playerAnimatorScript.SetJump(false);
            playerAudioScript.PlayCrouch();
        }
        else
        {
            playerAnimatorScript.SetJump(true);
        }
            playerRigidbody.gravityScale = BaseGravity;
    }
    private void GroundCheck()
    {
        if (IsGrounded())
        {



            lastTimeOnGround = TimeManager.Instance.TimeInSeconds;
            if (playerRigidbody.velocity.y <= 0.1f)
            {
                playerAnimatorScript.SetJump(false);
                if (isInAir)
                {
                    playerAudioScript.PlayLanding();
                    isInAir = false;
                }
               
            }
            isJumping = false;
        }
        else
        {
            if (playerRigidbody.velocity.y > 0.1f || playerRigidbody.velocity.y < -20f)
            {
                isInAir = true;
            }

            if (playerRigidbody.velocity.y == 0f)
                isInAir = false;
        }
    }

    public void CrouchFunction(InputAction.CallbackContext context)
    {
        if (playerHealth.IsDead || TimeManager.Instance.TimePaused)
            return;
        float CheckFloat = context.ReadValue<float>();

        if (IsGrounded() || isCrouching)
        {
           

            if (CheckFloat ==1 && !isCrouching)
            {
                isCrouching = true;
                playerAnimatorScript.SetCrouch(isCrouching);
                playerCollider.size = new Vector2(playerCollider.size.x, CrouchYSize);
                playerCollider.offset = new Vector2(playerCollider.offset.x, CrouchYPos);
                playerAudioScript.PlayCrouch();
                if (isAttacking)
                    ResetAttack();
            }
            else if (CheckFloat == 0 && isCrouching)   
            {
                ResetCrouch();
            }
            else
            {

            }


        }
    }

    public void ResetCrouch()
    {
        isCrouching = false;
        playerAnimatorScript.SetCrouch(isCrouching);
        playerCollider.size = new Vector2(playerCollider.size.x, OriginalYSize);
        playerCollider.offset = new Vector2(playerCollider.offset.x, OriginalYPos);
    }
    public void JumpFunction(InputAction.CallbackContext context)
    {
        if (playerHealth.IsDead || TimeManager.Instance.TimePaused)
            return;
        timeLastPressedJump = TimeManager.Instance.TimeInSeconds;
         isHalfJump = context.performed ? false : context.canceled ? true : false;

        if (isGrabbing)
            return;

        if (TimeManager.Instance.TimeInSeconds - timeSinceLastJump > 0.5f && !isDashing)
        {
            if (IsGrounded() || TimeManager.Instance.TimeInSeconds - lastTimeOnGround <= coyoteTime)
            {

                PerformJump(isHalfJump);
                
            }

            if (context.performed && wallJumpTimer > 0f)
            {
                

                isWallJumping = true;
                float direction = transform.rotation.eulerAngles.y == 180f ? 1 : -1;

                playerAudioScript.PlayJumpSound();
                playerRigidbody.velocity = new Vector2(MainStats.PlayerWallJumpSpeed * direction, MainStats.PlayerJumpSpeed);
                transform.rotation = Quaternion.Euler(transform.position.x, direction == 1 ? 0 : 180, transform.position.z);
                wallJumpTimer = 0f;


                playerAnimatorScript.SetJump(true);
                Invoke(nameof(CancelWallJump), MainStats.PlayerWallJumpTime + 0.1f);
                timeSinceLastJump = TimeManager.Instance.TimeInSeconds;
            }
            if (isAttacking)
            {
                ResetAttack();
            }
            if(isCrouching)
            {
                ResetCrouch();
            }
        }
    }

   
    private void PerformJump(bool isHalf = false)
    {
        playerAudioScript.PlayJumpSound();
        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, MainStats.PlayerJumpSpeed * (isHalf ? 0.5f : 1f));
        timeLastPressedJump = Mathf.NegativeInfinity;
        timeSinceLastJump = TimeManager.Instance.TimeInSeconds;
        playerAnimatorScript.SetJump();
        isJumping = true;
    }
    #endregion

    #region Attack Functions

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (playerHealth.IsDead || TimeManager.Instance.TimePaused)
            return;

        if (attackTimer <= 0f)
        {
            playerRigidbody.velocity = playerRigidbody.velocity * 0.5f;

            if (!IsGrounded() && dashUnlocked)
            {
                if(DashRoutine!=null)
                {
                    StopCoroutine(DashRoutine);
                }
                playerAudioScript.PlayDash();
                DashRoutine = StartCoroutine(DashCoroutine());
            }
           

            playerAnimatorScript.SetAttack(true);
            attackTimer = MainStats.PlayerAttackSpeed;
            isAttacking = true;

            if(isCrouching)
                ResetCrouch();
        }
    }

    private void AttackUpdation()
    {
        if (attackTimer > 0f)
        {
           attackTimer -= TimeManager.Instance.DeltaTime;    
        }
    }
    public void ResetAttack()
    {
        isAttacking = false;
        playerAnimatorScript.SetAttack(false);
    }

    public void ExecuteAttack(float multiplier =1f)
    {
        if (multiplier == 0f)
            multiplier = 1f;

        Collider2D[] Colliders = Physics2D.OverlapBoxAll(AttackCheckPos.position, attackCheckSize, 0, AttackLayerMask);
        
            foreach (var item in Colliders) {

                EnemyBaseScript getEnemy = item.GetComponent<EnemyBaseScript>();
                if (getEnemy != null)
                {
                    getEnemy.OnDamage(MainStats.PlayerAttackPower * multiplier);
                }
            }
        /*if (horizontalMovement != 0f)
        {
            ResetAttack();
        }*/

    }

    #endregion

    #region Health Functions

     
    public void OnDamage(float damage)
    {
        if (playerHealth.IsDead || TimeManager.Instance.TimePaused)
            return;

            playerHealth.DamageHealth(damage);

        BloodVFX.SetActive(true);
        Debug.Log($"Player Health : {playerHealth.CurrentHealth}");

        if (playerHealth.IsDead)
        {
            CallDeath();
            Invoke(nameof(DeathVFX), 2f);
        }
        else
            CallHurt();

        playerRigidbody.velocity = Vector3.zero;

        HPDialUI.Instance.UpdateSlider(playerHealth.CurrentHealth, true);
    }

    public void CallHurt()
    {
        playerAnimatorScript.CallHurt();
        ResetAttack();
        ResetCrouch();
    }
    private void DeathVFX() => DeathSmoke.SetActive(true);
    public void CallDeath()
    {
        playerAnimatorScript.CallDeath();
        ResetAttack();
        ResetCrouch();
        if(LensManager.Instance.IsActive)
            LensManager.Instance.ToggleLens();
    }
    #endregion

    #region Other Functions

    public void ToggleLens(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LensManager.Instance.ToggleLens();
        }
    }

    public void PauseFunction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TimeManager.Instance.PauseGame(!TimeManager.Instance.TimePaused);
        }
    }
    public void OnPauseCall(bool flag)
    {
        flag = TimeManager.Instance.TimePaused;


        playerAnimatorScript.PauseAnimator(flag);
        if (flag)
        {
            pauseVelocity = playerRigidbody.velocity;
            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.gravityScale = 0;
        }
        else
        {
            playerRigidbody.velocity =  pauseVelocity;
            playerRigidbody.gravityScale = BaseGravity;

        }
    }
    #endregion
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(GroundCheckPos.position, groundCheckSize);
        Gizmos.DrawWireCube(WallCheckPos.position, wallCheckSize);
        Gizmos.DrawWireCube(AttackCheckPos.position, attackCheckSize);
        Gizmos.DrawWireCube(LedgeCheckPos.position, ledgeCheckSize);
        Gizmos.DrawWireCube(SecondLedgeCheckPos.position, secondLedgeCheckSize);
        //Gizmos.DrawWireCube(new Vector2(transform.position.x + (0.15f * transform.localScale.x), transform.position.y + 2f), ledgeCheckSize);
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
    public float PlayerAttackSpeed;
    public float PlayerAttackPower;
    public float PlayerDashSpeed;
    public float PlayerDashDuration;
    public float PlayerDashCooldown;

}