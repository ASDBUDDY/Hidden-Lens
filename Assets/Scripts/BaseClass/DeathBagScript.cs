using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBagScript : InteractableBaseScript
{
    private int RespawnCurrency = 0;


    public void SetCurrency(int currency) => RespawnCurrency = currency;

    public override void OnActivate()
    {
        base.OnActivate();

        GameManager.Instance.IncrementWallet(RespawnCurrency);

        Destroy(this.gameObject);
    }
    
}
