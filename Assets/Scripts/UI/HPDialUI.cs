using System.Collections;
using System.Collections.Generic;
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

    public void UpdateSlider(float value, bool isHealth = true)
    {
        if (isHealth)
        {
            if(HealthRoutine != null)
                StopCoroutine(HealthRoutine);

            HealthRoutine = StartCoroutine(SliderLerp(value, HPSlider));
        }
        else
        {
            if(ManaRoutine!=null)
                StopCoroutine(ManaRoutine);

            ManaRoutine = StartCoroutine(SliderLerp(value, ManaSlider));
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

    private IEnumerator SliderLerp(float updateValue, Slider currentSlider )
    {
        float currentValue = currentSlider.value;
        if (currentValue >= updateValue)
        {
            currentSlider.value = updateValue;
        }

        while (currentValue < updateValue)
        {
            currentValue += (Mathf.Abs(currentValue - updateValue) > 1f ? 3f :1f) * TimeManager.Instance.DeltaTime;
            if (currentValue > updateValue)
                currentValue = updateValue;

            currentSlider.value = currentValue;
            yield return new WaitForEndOfFrame();
        }
        yield return null;


    }
}
