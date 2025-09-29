using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LensManager : MonoBehaviour
{
    public static LensManager Instance;

    
    private float lensTimer;
    [SerializeField]
    private float lensCooldown;
    public LensStats LensData;

    private bool isLensActive = false;
    private Coroutine LensRoutine;
    public List<EnemyBaseScript> AllEnemies;

    [Header("For DEBUG")]
    public float GaugeSize;
    public int GaugeCount;

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
       
    }
    private void Start()
    {
        LensData = new LensStats(GaugeSize * GaugeCount, GaugeSize);
    }

    private void Update()
    {
        if (lensTimer>0f)
        {
            lensTimer -= TimeManager.Instance.DeltaTime;

            if(lensTimer < 0f)
                lensTimer = 0f;
        }
    }
    private IEnumerator DepleteLens()
    {
        yield return new CustomWaitForSeconds(1f);

        while (isLensActive)
        {
            LensData.DepleteMana(LensData.VisibilityDepletion);

            if(LensData.CurrentLensMana <=0)
                ToggleLens();

            yield return new CustomWaitForSeconds(1f);
        }

    }
    public bool ToggleLens()
    {
        

        if (!isLensActive)
        {
            if (LensData.CurrentLensMana <= 0 || lensTimer>0f)
                return false;

            isLensActive = true;
            OnLensToggle();
            if(LensRoutine != null)
                StopCoroutine(LensRoutine);

            lensTimer = 0f;

            LensRoutine = StartCoroutine(DepleteLens());
        }
        else
        {
            isLensActive = false;
            OnLensToggle();

            if (LensRoutine != null)
                StopCoroutine(LensRoutine);

            lensTimer = lensCooldown;
        }

        return true;
    }

    public void OnLensToggle()
    {
        foreach(var item in AllEnemies) 
        {
            if(item.IsDead) continue;

            item.SwapMaterial(isLensActive);
        }

    }


}

[System.Serializable]
public class LensStats
{
    public float LensMaxSize;
    public float CurrentLensMana;
    public int activeGauges;
    private float gaugeSize;
    public float VisibilityDepletion; 

    public LensStats(float lensMaxSize, float gaugeSize)
    {
        LensMaxSize = lensMaxSize;
        CurrentLensMana = lensMaxSize;
        this.gaugeSize = gaugeSize;
        VisibilityDepletion = gaugeSize / 15f;

        CheckActiveGauge();
    }

    public void CheckActiveGauge()
    {
        activeGauges = Mathf.FloorToInt(CurrentLensMana / gaugeSize);
    }

    public void DepleteMana(float mana)
    {
        CurrentLensMana -= mana;

        if (CurrentLensMana < 0f)
            CurrentLensMana = 0f;

        CheckActiveGauge();
    }

    public void RefillMana(float mana)
    {
        CurrentLensMana += mana;

        if(CurrentLensMana>LensMaxSize)
            CurrentLensMana=LensMaxSize;

        CheckActiveGauge();
    }

    public void UpdateMaxMana(float mana)
    {
        LensMaxSize = mana;

       
    }
    public bool GaugeCapacity(int Gauges)
    {
        return activeGauges >= Gauges;
    }
    public void DepleteGauge(int Gauges)
    {
        if (GaugeCapacity(Gauges))
        {
            DepleteMana(Gauges * gaugeSize);
        }
    }
}
