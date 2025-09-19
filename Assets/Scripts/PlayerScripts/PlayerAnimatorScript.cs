using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorScript : MonoBehaviour
{
    private Animator playerAnimator;

    #region Parameters Variables

    private static string velocityX = "VelocityX";
    private int velX = 0;
    private static string velocityY = "VelocityY";
    private int velY = 0;
    private static string isJumping = "IsJumping";
    private int jump = 0;

    #endregion
    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        velX = Animator.StringToHash(velocityX);
        velY = Animator.StringToHash(velocityY);
        jump = Animator.StringToHash(isJumping);
    }

    public void SetVelocity(float velocityX, float velocityY)
    {
        playerAnimator.SetFloat(velX, velocityX);
        playerAnimator.SetFloat(velY, velocityY);
    }

    public void SetJump(bool flag = true) 
    {   if(playerAnimator.GetBool(jump) != flag)
            playerAnimator.SetBool(jump, flag); 
    }
}
