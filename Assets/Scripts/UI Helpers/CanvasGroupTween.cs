using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerUIAnimator
{
    public class CanvasGroupTween : Tween.TweenBase
    {
        public CanvasGroup canvasGroup;

        [Space]

        public float startAlpha = 0;
        public float endAlpha = 1;

        [Space]
        public bool awakePlay;
        public bool loop;

        public float duration = 5f;

        [Space]
        public UnityEvent startEvent;
        public UnityEvent finishEvent;

        private Coroutine coroutine_IEPlay;

        private void Start()
        {
            if (awakePlay)
            {
                Play();
            }
        }

        private void OnEnable()
        {
            if (awakePlay)
            {
                Play();
            }

            canvasGroup.alpha = startAlpha;
        }

        private void OnDisable()
        {
            Stop();
        }

        public override void Stop()
        {
            StopAllCoroutines();
            coroutine_IEPlay = null;
        }

        public override void Play()
        {
            if (coroutine_IEPlay != null)
            {
                StopCoroutine(coroutine_IEPlay);
                coroutine_IEPlay = null;
            }
            coroutine_IEPlay = StartCoroutine(IEPlay());
        }


        private IEnumerator IEPlay()
        {
            do
            {
                if (startEvent != null)
                    startEvent.Invoke();

                canvasGroup.alpha = startAlpha;

                float speed = (endAlpha - startAlpha) / duration;//1 - 0 = 1 && 0 - 1 = -1

                while (Mathf.Abs(endAlpha - canvasGroup.alpha) > 0.05f)//1 - 0.95 = 0.05 && 0 - 0.05 = 
                {
                    canvasGroup.alpha += speed * Time.deltaTime;

                    yield return new WaitForEndOfFrame();
                }

                for (int i = 0; i < tweenPerformanceValue + 1; i++)
                {
                    yield return new WaitForEndOfFrame();
                }

                canvasGroup.alpha = endAlpha;

                if (finishEvent != null)
                    finishEvent.Invoke();
            } while (loop);

            coroutine_IEPlay = null;
        }
    }

}
