using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPDialUI : MonoBehaviour
{
    public static HPDialUI Instance;

    [Header("HPBar")]
    public Slider HPSlider;

    [Header("ManaSlider")]
    public Slider ManaSlider;
    private Coroutine HealthRoutine;
    private float GaugeWidth =200f;
    public List<GameObject> GaugeList;
    public List<SimpleRotationScript> CogList;
    public Sprite ActiveCog;
    public Sprite InactiveCog;

    private Coroutine ManaRoutine;

    [Header("MainLens")]
    public List<SimpleRotationScript> MainCogList;
    public ColorLerpScript MainLens;
    public GameObject VFXObj;

    [Header("CurrencyBar")]
    public TextMeshProUGUI CurrencyAmount;
    private int currentCurrency;
    public float incrementSpeed;
    private Coroutine CurrencyRoutine;

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }

    }
    public void SetupHealth(float maxHealth)
    {
        HPSlider.maxValue = maxHealth;
        HPSlider.value = maxHealth;
    }
    public void SetupManaSlider(int gaugeCount)
    {
        RectTransform newRect = ManaSlider.GetComponent<RectTransform>();
        newRect.sizeDelta = new Vector2(gaugeCount*GaugeWidth,newRect.sizeDelta.y);

        ManaSlider.maxValue = LensManager.Instance.LensData.LensMaxSize;
        ManaSlider.value = LensManager.Instance.LensData.CurrentLensMana;

        SetGauges(gaugeCount);
        CheckForActiveCogs();
    }
    public void UpdateCurrency(int currency)
    {
        if(CurrencyRoutine != null) 
            StopCoroutine(CurrencyRoutine);

        CurrencyRoutine = StartCoroutine(CurrencyUpdation(currency));
    }
    public void UpdateSlider(float value, bool isHealth = true, bool isDamage =true)
    {
        if (isHealth)
        {
            if(HealthRoutine != null)
                StopCoroutine(HealthRoutine);

            
            HealthRoutine = StartCoroutine( isDamage ? SliderLerpNegative(value, HPSlider): SliderLerpPositive(value, HPSlider));
        }
        else
        {
            if(ManaRoutine!=null)
                StopCoroutine(ManaRoutine);

            ManaRoutine = StartCoroutine(isDamage ? SliderLerpNegative(value, ManaSlider) : SliderLerpPositive(value, ManaSlider));
            CheckForActiveCogs();
        }
    }

    public void ToggleMainLens(bool active)
    {
        foreach(var item in MainCogList)
        {
            item.ToggleRotation(active);
        }

        MainLens.ToggleColorLerp(active);
        VFXObj.SetActive(active);
    }
    private void SetGauges(int gaugeCount)
    {
        for (int i = 0; i < GaugeList.Count; i++) 
        {
            GaugeList[i].gameObject.SetActive(i<gaugeCount);
        }

        for (int i = 0; i < CogList.Count; i++)
        {
            CogList[i].ToggleRotation(false);
            CogList[i].gameObject.SetActive(i < gaugeCount);
            
        }

    }

    private void CheckForActiveCogs()
    { 
        int activeCogs = LensManager.Instance.LensData.activeGauges;

        for (int i = 0; i < CogList.Count; i++)
        {
            if (!CogList[i].gameObject.activeInHierarchy)
                break;

            Image currentSprite = CogList[i].GetComponent<Image>();
            if (i < activeCogs)
            {
                currentSprite.sprite = ActiveCog;
                CogList[i].ToggleRotation(true);
            }
            else
            {
                currentSprite.sprite = InactiveCog;
                CogList[i].ToggleRotation(false);
            }
        }
    }

    private IEnumerator SliderLerpPositive(float updateValue, Slider currentSlider )
    {
        float currentValue = currentSlider.value;
        if (currentValue >= updateValue)
        {
            currentSlider.value = updateValue;
            
        }
        float slideSpeed = (Mathf.Abs(currentValue - updateValue) > 4f ? 6f : 1f);
        while (currentValue < updateValue)
        {
            currentValue += slideSpeed * TimeManager.Instance.DeltaTime;
            if (currentValue > updateValue)
                currentValue = updateValue;

            currentSlider.value = currentValue;
            yield return new WaitForEndOfFrame();
        }
        yield return null;


    }
    private IEnumerator SliderLerpNegative(float updateValue, Slider currentSlider)
    {
        float currentValue = currentSlider.value;
        if (currentValue <= updateValue)
        {
            currentSlider.value = updateValue;
            
        }
        float slideSpeed = (Mathf.Abs(currentValue - updateValue) > 4f ? 6f : 1f);
        while (currentValue > updateValue)
        {
            currentValue -= slideSpeed * TimeManager.Instance.DeltaTime;
            if (currentValue < updateValue)
                currentValue = updateValue;

            currentSlider.value = currentValue;
            yield return new WaitForEndOfFrame();
        }
        yield return null;


    }

    private IEnumerator CurrencyUpdation(int finalAmount)
    {
        float time = 0;
        int startingAmount = currentCurrency;
        CurrencyAmount.text = startingAmount.ToString();

       

        while (currentCurrency != finalAmount)
        {
            yield return null;

            time += TimeManager.Instance.DeltaTime;
            float factor = time / incrementSpeed;
            currentCurrency = (int)Mathf.Lerp(startingAmount, finalAmount, factor);

            CurrencyAmount.text = currentCurrency.ToString();

        }

        yield break;
    }
}
