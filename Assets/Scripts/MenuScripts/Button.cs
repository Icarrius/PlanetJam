using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent onClickFunction;

    public bool needToFlash = true;

    public void OnPointerClick(PointerEventData eventData)
    {
        SoundHandler.singleton.PlayUiSound();

        if (needToFlash)
        {
            FlashEffect.singleton.FlashScreen();
            StartCoroutine(UseButtonWithDelay(0.6f));
        }
        else
        {
            StartCoroutine(UseButtonWithDelay(0));
        }      
    }

    private IEnumerator UseButtonWithDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        onClickFunction.Invoke();
    }
}
