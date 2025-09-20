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
    private static string isWallSliding = "IsWallSliding";
    private int wallSlide = 0;
    private static string isAttacking = "IsAttacking";
    private int attack = 0;

    #endregion
    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        velX = Animator.StringToHash(velocityX);
        velY = Animator.StringToHash(velocityY);
        jump = Animator.StringToHash(isJumping);
        wallSlide = Animator.StringToHash(isWallSliding);
        attack = Animator.StringToHash(isAttacking);
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

    public void SetWallSide(bool flag = true)
    {
        if (playerAnimator.GetBool(wallSlide) != flag)
            playerAnimator.SetBool(wallSlide, flag);
    }

    public void SetAttack(bool flag = true)
    {
        if (playerAnimator.GetBool(attack) != flag)
            playerAnimator.SetBool(attack, flag);
    }
}
