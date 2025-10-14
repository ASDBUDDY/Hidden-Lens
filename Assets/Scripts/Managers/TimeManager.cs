using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    public BoolEventChannelSO PauseEvent;

 
    public bool TimePaused {  get; private set; }

    /// <summary>
    /// General Time
    /// </summary>
    public float TimeInSeconds {  get; private set; }

    /// <summary>
    /// Self made Delta Time
    /// </summary>
    public float DeltaTime {  get; private set; }

    /// <summary>
    /// data retention from last frame
    /// </summary>
    [SerializeField] private float timeLastFrame;

    /// <summary>
    /// Text reference to On Screen Timer
    /// </summary>
    [SerializeField] private TextMeshProUGUI timeText;

    /// <summary>
    /// Self made Fixed Delta Time
    /// </summary>
    public float FixedDeltaTime { get; private set; }

    public float ActiveTimer;

    private string timeString = "";

    private void Awake()
    {
      
        if (Instance == null)
        {
            Instance = this;
        }
        SetupTime();
    }

    private void Update()
    {
        if (!TimePaused)
        {
            TimeFunctioning();
            CalculateOnScreenTime();
        }
        /*
        if( Input.GetKeyDown(KeyCode.P)){
            PauseGame(!TimePaused);
        }
        */
    }

    private void SetupTime()
    {
        TimeInSeconds = 0f;
        timeLastFrame = 0;
        DeltaTime = 0f;
        TimePaused = false; //Time starts paused as you are in the main menu
        FixedDeltaTime = 1f / 50;
        ActiveTimer = 0f;
    }
    private void TimeFunctioning()
    {
        TimeInSeconds += Time.deltaTime;
        DeltaTime = TimeInSeconds - timeLastFrame;
        timeLastFrame = TimeInSeconds;
    }

    public void PauseGame(bool flag)
    {
        TimePaused = flag;
        DeltaTime = flag ? 0 : TimeInSeconds - timeLastFrame;
        FixedDeltaTime = flag ? 0 : 1f / 50;
        PauseEvent.RaiseEvent(flag);
       

    }


    private void CalculateOnScreenTime()
    {
        ActiveTimer += DeltaTime;


        int mins = (int)Mathf.Floor(ActiveTimer / 60f);
        float seconds = Mathf.Floor(ActiveTimer % 60f);

        if (seconds < 10)
        {
            timeString =  $"{mins}:0{seconds}";
        }
        else
        {
            timeString = $"{mins}:{seconds}";
        }

        //timeText.text = timeString;
    }
}
