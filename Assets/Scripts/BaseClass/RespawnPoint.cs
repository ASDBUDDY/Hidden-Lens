using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public bool IsActive = false;
    public GameObject LightningVFX;
    public SpriteRenderer Glow;
    
    
    public void TogglePoint(bool flag)
    {
        if (flag) { 

            if(!IsActive)
            {
                LightningVFX.SetActive(true);
                StartCoroutine(GlowRender());
            }
          
        }
        else
        {
            Color newColor = Glow.color;
            newColor.a = 0f;
            Glow.color = newColor;
        }

        IsActive = flag;
        
    }

    private IEnumerator GlowRender()
    {

        while(Glow.color.a != 1)
        {
            Color color = Glow.color;
            color.a = Mathf.Lerp(Glow.color.a,1f,TimeManager.Instance.DeltaTime);
            Glow.color = color;
            yield return new WaitForEndOfFrame();
           
        }

        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsActive)
        {
            if (collision.gameObject.layer == GameConstants.Layers.PLAYER_LAYER)
            {
                TogglePoint(true);
                RespawnManager.Instance.UpdateSpawn(this);
            }
        }
        else
        {
            if (collision.gameObject.layer == GameConstants.Layers.PLAYER_LAYER)
            {
                if(GameManager.Instance.IsLensUnlocked && LensManager.Instance.IsLensEmpty)
                {
                    LensManager.Instance.RefillGauge();
                }
            }
        }
    }
}
