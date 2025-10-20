using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInteractableScript : InteractableBaseScript
{
    private Animator animator;

    public GameObject RewardItem;
    public int RewardAmount;
 
    public int ChestID;

    public ChestRewards Rewards;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        if (PlayerPrefs.HasKey(GameConstants.Interactables.CHEST_ID + ChestID))
        {
            IsActivated = true;

            animator.SetBool("IsOpened", IsActivated);
        }

        RewardItem.SetActive(false);
    }
    private void OnEnable()
    {
        if (IsActivated)
        {
            animator.SetBool("IsOpened", IsActivated);
            RewardItem.SetActive(false);
        }
    }
    public override void OnActivate()
    {
        if (IsActivated)
            return;

        base.OnActivate();

        PlayerPrefs.SetInt(GameConstants.Interactables.CHEST_ID + ChestID, 1);

        animator.SetBool("IsOpened", IsActivated);

        RewardItem.SetActive(true);

        GiveReward();
    }

    private void GiveReward()
    {
        switch (Rewards)
        {
            case ChestRewards.HiddenLens:
                GameManager.Instance.ActivateLens(); break;
            case ChestRewards.NewGauge:
                LensManager.Instance.SetupLensMana(LensManager.Instance.GaugeCount + RewardAmount); break;
            case ChestRewards.ManaPotion:
                LensManager.Instance.RefillGauge((float) RewardAmount); break;
            case ChestRewards.HealthPotion: 
                GameManager.Instance.HealPlayer(RewardAmount); break;
            case ChestRewards.DashAbility:
                GameManager.Instance.ActivateDash(); break;
            case ChestRewards.WallClimbAbility:
                GameManager.Instance.ActivateWallSlide(); break;
            default: break;

        }
    }

    public enum ChestRewards
    {
        HiddenLens = 0,
        NewGauge = 1,
        ManaPotion = 2,
        HealthPotion = 3,
        DashAbility = 4,
        WallClimbAbility = 5,
    }
}
