using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrapScript : MonoBehaviour
{
    public bool IsActive = false;
  
    public float FireRate = 1f;
    public float FireSpeed = 2f;
    public Transform DestinationPoint;
    public GameObject AmmoParent;

    private List<GameObject> ammoList;
    private List<GameObject> activeObjList;
    private bool hasFired = false;
    private float lastFired = 0f;
    // Start is called before the first frame update
    void Start()
    {
        ammoList = new List<GameObject>();
        activeObjList = new List<GameObject>();
        foreach(Transform child in AmmoParent.transform)
        {
            child.gameObject.SetActive(false);
            ammoList.Add(child.gameObject);
        }
        lastFired = FireRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsActive)
        {
            CheckForNewArrow();
            FireArrow();
        }
    }

    private void CheckForNewArrow()
    {
        if(lastFired >= FireRate)
        {
            for(int i = 0; i < ammoList.Count; i++)
            {
                if (ammoList[i].gameObject.activeInHierarchy)
                    continue;

                ammoList[i].gameObject.SetActive(true);
                activeObjList.Add(ammoList[i].gameObject);

            }
            lastFired = 0f;
        }
        else
        {
            lastFired += TimeManager.Instance.DeltaTime;
        }
    }
    private void FireArrow()
    {
        if (activeObjList.Count > 0)
        {
            for (int i = activeObjList.Count-1; i>=0; i--)
            {
                activeObjList[i].transform.position = Vector2.MoveTowards(activeObjList[i].transform.position, DestinationPoint.position, TimeManager.Instance.DeltaTime * FireSpeed);
                if (Mathf.Abs(activeObjList[i].transform.position.x - DestinationPoint.position.x) <= 0.1f)
                {
                    activeObjList[i].SetActive(false);
                   
                    activeObjList[i].transform.localPosition = Vector3.zero;

                    activeObjList.RemoveAt(i);
                    i--;
                }
            }
            
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GameConstants.Layers.PLAYER_LAYER)
        {
            IsActive = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GameConstants.Layers.PLAYER_LAYER)
        {
            IsActive = false;
            activeObjList.Clear();
            foreach(GameObject child in ammoList)
            {
                child.SetActive(false);
                child.transform.localPosition = Vector3.zero;
            }

            lastFired = FireRate;
        }
    }
}
