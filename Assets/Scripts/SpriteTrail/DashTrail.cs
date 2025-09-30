using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DashTrail : MonoBehaviour
{
    public SpriteRenderer LeadingSprite;

    public int TrailSegments;
    public float TrailTime;
    public GameObject TrailObject;

    private float spawnInterval;
    private float spawnTimer;
    private bool trailEnabled;

    private List<GameObject> trailObjectsInUse;
    private Queue<GameObject> trailObjectsNotInUse;

    // Use this for initialization
    void Start()
    {
        spawnInterval = TrailTime / TrailSegments;
        trailObjectsInUse = new List<GameObject>();
        trailObjectsNotInUse = new Queue<GameObject>();

        for (int i = 0; i < TrailSegments; i++)
        {
            GameObject trail = GameObject.Instantiate(TrailObject);
            trail.transform.SetParent(transform);
            trailObjectsNotInUse.Enqueue(trail);
        }

        trailEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (trailEnabled)
        {
            spawnTimer += TimeManager.Instance.DeltaTime;

            if (spawnTimer >= spawnInterval && trailObjectsNotInUse.Count>0)
            {
                GameObject trail = trailObjectsNotInUse.Dequeue();
                if (trail != null)
                {
                    DashTrailObject trailObject = trail.GetComponent<DashTrailObject>();

                    trailObject.Initiate(TrailTime, LeadingSprite.sprite, transform.position, this);
                    trailObjectsInUse.Add(trail);

                    spawnTimer = 0;
                }
            }
        }
    }

    public void RemoveTrailObject(GameObject obj)
    {
        trailObjectsInUse.Remove(obj);
        trailObjectsNotInUse.Enqueue(obj);
    }

    public void SetEnabled(bool enabled)
    {
        trailEnabled = enabled;

        if (enabled)
        {
            spawnTimer = spawnInterval;
        }
    }

}