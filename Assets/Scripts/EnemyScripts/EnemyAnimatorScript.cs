using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorScript : MonoBehaviour
{
    private Animator enemyAnimator;

    #region Animator Variables
    private static string Velocity = "Velocity";
    private int velocity = 0;
    private static string attackTrigger = "TriggerAttack";
    private int attack = 0;
    private static string hurtTrigger = "TriggerHurt";
    private int hurt = 0;
    private static string deathTrigger = "TriggerDeath";
    private int death = 0;
    private static string attackSecondTrigger = "TriggerAttackSecond";
    private int attackSecond = 0;


    #endregion

    private void Awake()
    {
        enemyAnimator = GetComponent<Animator>();
        velocity = Animator.StringToHash(Velocity);
        attack = Animator.StringToHash(attackTrigger);
        hurt = Animator.StringToHash(hurtTrigger);
        death = Animator.StringToHash(deathTrigger);
        attackSecond = Animator.StringToHash(attackSecondTrigger);
    }


    

    public void SetVelocity(float newVelocity) => enemyAnimator.SetFloat(velocity, newVelocity);
    public void TriggerHurt() => enemyAnimator.SetTrigger(hurt);
    public void TriggerInitialAttack() => enemyAnimator.SetTrigger(attack);
    public void TriggerSecondAttack() => enemyAnimator.SetTrigger(attackSecond);
    public void TriggerDeath() => enemyAnimator.SetTrigger(death);

}
