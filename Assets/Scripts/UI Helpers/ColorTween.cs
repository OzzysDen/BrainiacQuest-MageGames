using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PlayerUIAnimator
{
    public class ColorTween : Tween.TweenBase
    {
        [Tooltip("Saniye cinsinden renk degisimin bitis suresi")]
        public float duration = 5;

        public bool awakePlay = true;

        public bool loop = false;
        public int loopCount = 0;
        [Space]

        public Color firstColor = Color.red;
        public Color secondaryColor = Color.white;

        [Space]

        [Tooltip("Deger atanmasi scriptin bulundugu objeden almaya calisir (TextMesh veya Image)")]
        public MonoBehaviour targetComponent;
        [Space]

        public UnityEvent onStart;
        public UnityEvent onEnd;

        private Coroutine coroutineLerp;

        public override void Play()
        {
            if (coroutineLerp != null)
            {
                StopCoroutine(coroutineLerp);
                coroutineLerp = null;
            }

            coroutineLerp = StartCoroutine(LerpColor());
        }

        public override void Stop()
        {
            StopAllCoroutines();
            coroutineLerp = null;
        }

        public void ResetColor()
        {
            if (coroutineLerp != null)
            {
                StopCoroutine(coroutineLerp);
                coroutineLerp = null;
            }

            StartCoroutine(ResetColor_());
        }

        private void Awake()
        {
            if (targetComponent == null)
            {
                targetComponent = GetComponent<Image>();

                if (targetComponent == null)
                {
                    targetComponent = GetComponent<TextMeshProUGUI>();
                }
            }
        }

        private void OnEnable()
        {
            if (awakePlay)
                Play();
        }

        private void OnDisable()
        {
            StopAllCoroutines();

            coroutineLerp = null;
        }

        private void SetColor(Color color)
        {
            if (targetComponent.GetType() == typeof(Image))
            {
                ((Image)targetComponent).color = color;
            }
            else if (targetComponent.GetType() == typeof(TextMeshProUGUI))
            {
                ((TextMeshProUGUI)targetComponent).color = color;
            }
        }

        //Reset durumunda coroutine calisirken kapatilmayacagi icin yanlis renk atamasi yapilabilir
        private IEnumerator ResetColor_()
        {
            SetColor(secondaryColor);
            yield return new WaitForEndOfFrame();
            SetColor(secondaryColor);
        }

        private IEnumerator LerpColor()
        {
            onStart.Invoke();
            int currentCount = 0;
            SetColor(firstColor);
            do
            {
                currentCount++;

                float progress = 0;

                while (progress < 1)
                {
                    //  image.color = 
                    if (currentCount % 2 != 0)
                        SetColor(Color.Lerp(firstColor, secondaryColor, progress));
                    else
                        SetColor(Color.Lerp(secondaryColor, firstColor, progress));

                    progress += Time.deltaTime / duration;

                    yield return new WaitForEndOfFrame();
                }

                if (currentCount % 2 != 0)
                    SetColor(secondaryColor);
                else
                    SetColor(firstColor);

                for (int i = 0; i < tweenPerformanceValue + 1; i++)
                {
                    yield return new WaitForEndOfFrame();
                }

            } while (loop || (loopCount != 0 && loopCount > currentCount));

            onEnd.Invoke();

            coroutineLerp = null;
            yield break;
        }
    }

}
