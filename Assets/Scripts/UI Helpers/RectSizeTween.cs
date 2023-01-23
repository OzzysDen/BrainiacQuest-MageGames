using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerUIAnimator
{
    public class RectSizeTween : Tween.TweenBase
    {
        [System.Serializable]
        public class AnimationInfo
        {
            public bool sizeAnimation = true;
            public Vector2 size;

            [Tooltip("Bitirme zamani, saniye cinsinden")]
            public float finishTimeSize = 60f;
        }

        public AnimationInfo[] animationStations;

        public bool awakePlay;

        public RectTransform layout;

        private RectTransform _myRect;

        private Coroutine animationManageCoroutine;
        private Coroutine sizeAnimationCoroutine;
        private void Awake()
        {
            _myRect = GetComponent<RectTransform>();

            if (awakePlay)
            {
                Play();
            }
        }

        private void OnEnable()
        {
            if (_myRect != null && awakePlay)
            {
                Play();
            }
        }
        public override void Play()
        {
            if (animationManageCoroutine != null)
            {
                StopCoroutine(animationManageCoroutine);
                animationManageCoroutine = null;

                if (sizeAnimationCoroutine != null)
                {
                    StopCoroutine(sizeAnimationCoroutine);
                    sizeAnimationCoroutine = null;

                }
            }

            animationManageCoroutine = StartCoroutine(AnimationManage());
        }

        /// <summary>
        /// Hazirlanan animasyonu ters yonde calistirir
        /// </summary>
        public override void PlayBack(bool setDeactive = false)
        {
            if (animationManageCoroutine != null)
            {
                StopCoroutine(animationManageCoroutine);
                animationManageCoroutine = null;

                if (sizeAnimationCoroutine != null)
                {
                    StopCoroutine(sizeAnimationCoroutine);
                    sizeAnimationCoroutine = null;

                }
            }

            animationManageCoroutine = StartCoroutine(AnimationManageBack());
        }


        public override void Stop()
        {
            StopAllCoroutines();

            animationManageCoroutine = null;
            sizeAnimationCoroutine = null;
        }
        private IEnumerator AnimationManage()
        {
            int lastIndex = 0;
            while (true)
            {
                if (sizeAnimationCoroutine == null)
                {
                    if (lastIndex + 1 == animationStations.Length)
                    {
                        animationManageCoroutine = null;
                        yield break;
                    }

                    AnimationInfo start = animationStations[lastIndex];
                    AnimationInfo end = animationStations[lastIndex + 1];

                    if (start.sizeAnimation && end.sizeAnimation)
                    {
                        sizeAnimationCoroutine = StartCoroutine(AnimationSize(start.size, end.size, start.finishTimeSize));
                    }

                    lastIndex++;
                }

                for (int i = 0; i < tweenPerformanceValue + 1; i++)
                {
                    yield return new WaitForEndOfFrame();
                }
            }
        }

        /// <summary>
        /// Hazirlanan animasyonu ters yonde calistirmayi yonetir
        /// </summary>
        /// <returns></returns>
        private IEnumerator AnimationManageBack()
        {
            int lastIndex = animationStations.Length - 1;
            while (true)
            {
                if (sizeAnimationCoroutine == null)
                {
                    if (lastIndex == 0)
                    {
                        animationManageCoroutine = null;
                        yield break;
                    }

                    AnimationInfo start = animationStations[lastIndex];
                    AnimationInfo end = animationStations[lastIndex - 1];

                    if (start.sizeAnimation && end.sizeAnimation)
                    {
                        sizeAnimationCoroutine = StartCoroutine(AnimationSize(start.size, end.size, start.finishTimeSize));
                    }

                    lastIndex--;
                }

                for (int i = 0; i < tweenPerformanceValue + 1; i++)
                {
                    yield return new WaitForEndOfFrame();
                }
            }
        }

        private IEnumerator AnimationSize(Vector2 start, Vector2 end, float finishTime)
        {
            Vector2 to = end;
            Vector2 currentSize = start;

            _myRect.sizeDelta = currentSize;

            float Speed = Vector2.Distance(end, start) / finishTime;

            while (Vector3.Distance(currentSize, to) > 0.2f)
            {
                currentSize = Vector3.MoveTowards(currentSize, to, Speed * Time.deltaTime);
                _myRect.sizeDelta = currentSize;


                yield return new WaitForEndOfFrame();

                //    LayoutRebuilder.ForceRebuildLayoutImmediate(layout); // FIX:mobil tarafinda NUllException hatasi verdigi icin kaldirildi...
                //    LayoutRebuilder.MarkLayoutForRebuild(layout);
            }

            _myRect.sizeDelta = to;

            sizeAnimationCoroutine = null;
            
        }
    }

}
