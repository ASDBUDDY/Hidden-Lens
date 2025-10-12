using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScript : MonoBehaviour
{

   
    public GameObject MainCam;
    public Vector2 ParallaxEffect;
    public bool HorizontalEffect,VerticalEffect;

    private Vector2 spriteLength;
    private Vector3 lastCameraPos;
  
    // Start is called before the first frame update
    void Start()
    {
      
        Sprite MainSprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = MainSprite.texture;
        spriteLength = new Vector2(texture.width/MainSprite.pixelsPerUnit,texture.height/MainSprite.pixelsPerUnit);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Transform camTransform = MainCam.transform;

        Vector3 deltaMovement = camTransform.position - lastCameraPos;
        transform.position += new Vector3(deltaMovement.x * ParallaxEffect.x, deltaMovement.y * ParallaxEffect.y);
       lastCameraPos = camTransform.position;

      if(HorizontalEffect)
       if(Mathf.Abs(camTransform.position.x - transform.position.x)>= spriteLength.x)
       {
            float offsetPosX = (camTransform.position.x - transform.position.x) % spriteLength.x;
            transform.position = new Vector3(camTransform.position.x +offsetPosX,transform.position.y);
       }

      if(VerticalEffect)
       if (Mathf.Abs(camTransform.position.y - transform.position.y) >= spriteLength.y)
       {
            float offsetPosY = (camTransform.position.y - transform.position.y) % spriteLength.y;
            transform.position = new Vector3(transform.position.x,camTransform.position.y + offsetPosY);
       }
    }
}
