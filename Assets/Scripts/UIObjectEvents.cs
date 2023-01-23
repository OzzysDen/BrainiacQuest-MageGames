using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIObjectEvents:MonoBehaviour
{
    public bool isPopupSound;
    public UnityEvent OnEnableEvent;
    public UnityEvent OnDisableEvent;

    private void OnEnable()
    {
        if (isPopupSound)
        {
            AudioManager.Instance.PlayAudio("Popup");
        }
        OnEnableEvent?.Invoke();
    }

    private void OnDisable()
    {
        OnDisableEvent?.Invoke();
    }
}