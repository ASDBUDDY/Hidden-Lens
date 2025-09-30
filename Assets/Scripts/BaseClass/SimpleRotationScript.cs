using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotationScript : MonoBehaviour
{
    public bool IsActive;

    public float RotationSpeed = 2f;

    public bool IsClockwise = true;

    private Coroutine TurnCoroutine;

    private RectTransform UIRect;

    private void Awake()
    {
        UIRect = GetComponent<RectTransform>();
    }

    public void ToggleRotation(bool value)
    {
        IsActive = value;

        if (TurnCoroutine != null)
            StopCoroutine(TurnCoroutine);

        if (IsActive)
        {
            TurnCoroutine = StartCoroutine(RotationRoutine());
        }
       
    }


    private IEnumerator RotationRoutine()
    {
        while (IsActive)
        {
            Vector3 EulerAngles = UIRect.rotation.eulerAngles;
            EulerAngles.z += RotationSpeed * TimeManager.Instance.DeltaTime * (IsClockwise ? 1f : -1f);

            if (EulerAngles.z >= 360f || EulerAngles.z <= -360f)
                EulerAngles.z = 0f;

            UIRect.eulerAngles = EulerAngles;
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }
}
