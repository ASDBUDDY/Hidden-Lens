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
    public List<HiddenLensObjects> HiddenLayerObjs;
    public bool IsActive { get { return isLensActive; } }

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
       
            GaugeCount = PlayerPrefs.GetInt(GameConstants.PlayerPrefConstants.GAUGE_UNLOCK, 0);

       SetupLensMana(GaugeCount);
        
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

    public void SetupLensMana(int gaugeCount =0)
    {
        GaugeCount = gaugeCount;
        UpdateGauge(GaugeCount);
        HPDialUI.Instance.SetupManaSlider(GaugeCount);

       PlayerPrefs.SetInt(GameConstants.PlayerPrefConstants.GAUGE_UNLOCK, GaugeCount);
    }
    public void ResetLens()
    {
        Start();
        foreach (var item in AllEnemies)
        {
            item.gameObject.SetActive(false);
            item.gameObject.SetActive(true);
            item.Start();
        }
    }

    public void RefillGauge(int gaugeCount = 1)
    {
        LensData.RefillMana(gaugeCount * GaugeSize);
        HPDialUI.Instance.UpdateSlider(LensData.CurrentLensMana, false);
    }
    public void RefillGauge(float refillAmount)
    {
        LensData.RefillMana(refillAmount);
        HPDialUI.Instance.UpdateSlider(LensData.CurrentLensMana, false);
    }
    public void UpdateGauge(int gaugeCount)
    {
        LensData = new LensStats(GaugeSize * gaugeCount, GaugeSize);
    }
    private IEnumerator DepleteLens()
    {
        

        while (isLensActive)
        {
            LensData.DepleteMana(LensData.VisibilityDepletion);
            

            if (LensData.CurrentLensMana <=0)
                ToggleLens();

            HPDialUI.Instance.UpdateSlider(LensData.CurrentLensMana, false);

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

        HPDialUI.Instance.ToggleMainLens(isLensActive);
        return true;
    }

    public void OnLensToggle()
    {
        foreach(var item in AllEnemies) 
        {
            if(!item.gameObject.activeInHierarchy)
                continue;

            if (item.IsDead)
                continue;

            item.SwapMaterial(isLensActive);
        }

        foreach(var item in HiddenLayerObjs)
        {
            item.ToggleReveal(isLensActive);
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
