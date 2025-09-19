using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorScript : MonoBehaviour
{
    private Animator playerAnimator;

    #region Parameters Variables

    private static string velocity = "Velocity";
    private int vel = 0;

    #endregion
    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        vel = Animator.StringToHash(velocity);
    }

    public void SetVelocity(float velocity)
    {
        playerAnimator.SetFloat(vel, velocity);
    }
    
}
