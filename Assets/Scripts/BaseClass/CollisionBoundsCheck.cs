using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBoundsCheck : MonoBehaviour
{
    public LayerMask CollisionCheckMask;
    public bool IsColliding = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((CollisionCheckMask & (1 << collision.gameObject.layer)) != 0)
        {
            IsColliding = true;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((CollisionCheckMask & (1 << collision.gameObject.layer)) != 0)
        {
            IsColliding = false;
        }
    }
}
