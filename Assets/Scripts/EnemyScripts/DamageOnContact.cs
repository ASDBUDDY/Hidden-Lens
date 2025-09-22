using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
    public float DamagePower = 5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GameConstants.Layers.PLAYER_LAYER)
        {
            PlayerMainScript target = collision.gameObject.GetComponent<PlayerMainScript>();

            if (target != null)
            {
                target.OnDamage(DamagePower);
            }
        }
    }
   
}
