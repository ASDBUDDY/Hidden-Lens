using UnityEngine;
using System.Collections;

public class DashTrailObject : MonoBehaviour
{
    public SpriteRenderer Renderer;
    public Color StartColor, EndColor;

    private bool objectInUse;
    private Vector2 spawnPosition;
    private float displayTime;
    private float timeDisplayed;
    private DashTrail spawner;

    // Use this for initialization
    void Start()
    {
        Renderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (objectInUse)
        {
            transform.position = spawnPosition;

            timeDisplayed += TimeManager.Instance.DeltaTime;

            Renderer.color = Color.Lerp(StartColor, EndColor, timeDisplayed / displayTime);

            if (timeDisplayed >= displayTime)
            {
                spawner.RemoveTrailObject(gameObject);
                objectInUse = false;
                Renderer.enabled = false;
                //Destroy (gameObject);
            }
        }
    }

    public void Initiate(float displayTime, Sprite sprite, Vector2 position, DashTrail trail)
    {
        this.displayTime = displayTime;
        Renderer.sprite = sprite;
        Renderer.enabled = true;
        spawnPosition = position;
        timeDisplayed = 0;
        spawner = trail;
        objectInUse = true;
    }
}