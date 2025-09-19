using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMainScript : MonoBehaviour
{
    public PlayerStats MainStats;
    private Rigidbody2D playerRigidbody;
    private SpriteRenderer playerSpriteRenderer;

    private float horizontalMovement = 0f;
    private float movementSmoothTime = 0.5f;
    private Vector2 velocityRef = Vector2.zero;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
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
        }
    }
}

[System.Serializable]
public class PlayerStats
{
    public float PlayerSpeed;

}