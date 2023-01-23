using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PlayerUIAnimator
{
    public class ScaleTween : Tween.TweenBase
    {
        [System.Serializable]
        public class AnimationInfo
        {
            public bool scaleAnimation = true;
            public Vector3 scale = new Vector3(1, 1, 1);

            [Tooltip("Bitirme zamani, saniye cinsinden")]
            public float finishTimeScale = 60f;
        }
        [NonReorderable]
        public AnimationInfo[] animationStations;

        public bool awakePlay;
        public bool loop = false;

        public RectTransform layout;

        public UnityEvent startEvent;
        public UnityEvent finishEvent;
        public UnityEvent playBackFinishEvent;

        private RectTransform _myRect;

        private Coroutine animationManageCoroutine;
        private Coroutine scaleAnimationCoroutine;

        private void Start()
        {
            if (_myRect == null)
            {
                _myRect = GetComponent<RectTransform>();

                try
                {
                    if (layout == null)
                    {
                        layout = gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<RectTransform>();
                    }
                }
                catch (System.Exception)
                {
                }
            }
        }
        private void OnEnable()
        {
            if (_myRect == null)
            {
                Start();
            }
            if (_myRect != null && awakePlay)
            {
                Play();
            }
        }
        public override void Stop()
        {
            StopAllCoroutines();
            animationManageCoroutine = null;
            scaleAnimationCoroutine = null;
        }
        public override void Play()
        {
            if (_myRect == null)
            {
                Start();
            }
            if (animationManageCoroutine != null)
            {
                if (scaleAnimationCoroutine != null)
                {
                    StopCoroutine(scaleAnimationCoroutine);
                    scaleAnimationCoroutine = null;
                }
            }
            if (gameObject.activeInHierarchy)
            {
                animationManageCoroutine = StartCoroutine(AnimationManage());
            }
        }

        /// <summary>
        /// Hazirlanan animasyonu ters yonde calistirir
        /// </summary>
        public override void PlayBack(bool setDeactive = false)
        {
            if (_myRect == null)
            {
                Start();
            }

            if (animationManageCoroutine != null)
            {
                StopCoroutine(animationManageCoroutine);
                animationManageCoroutine = null;

                if (scaleAnimationCoroutine != null)
                {
                    StopCoroutine(scaleAnimationCoroutine);
                    scaleAnimationCoroutine = null;
                }
            }

            if (gameObject.activeInHierarchy)
            {
                animationManageCoroutine = StartCoroutine(AnimationManageBack());
            }
        }

        private IEnumerator AnimationManage()
        {
            if (startEvent != null)
                startEvent.Invoke();

            int lastIndex = 0;

            while (true)
            {
                if (scaleAnimationCoroutine == null)
                {
                    if (lastIndex + 1 == animationStations.Length)
                    {
                        if (loop)
                        {
                            if (finishEvent != null)
                                finishEvent.Invoke();

                            if (startEvent != null)
                                startEvent.Invoke();

                            lastIndex = 0;
                            continue;
                        }
                        else
                        {
                            animationManageCoroutine = null;
                            finishEvent.Invoke();
                            yield break;
                        }
                    }

                    AnimationInfo start = animationStations[lastIndex];
                    AnimationInfo end = animationStations[lastIndex + 1];

                    if (start.scaleAnimation && end.scaleAnimation)
                    {
                        scaleAnimationCoroutine = StartCoroutine(AnimationScale(start.scale, end.scale, start.finishTimeScale));
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
            if (startEvent != null)
                startEvent.Invoke();

            int lastIndex = animationStations.Length - 1;
            while (true)
            {
                if (scaleAnimationCoroutine == null)
                {
                    if (lastIndex == 0)
                    {
                        if (loop)
                        {
                            playBackFinishEvent.Invoke();
                            lastIndex = animationStations.Length - 1;
                            continue;
                        }
                        else
                        {
                            animationManageCoroutine = null;
                            playBackFinishEvent.Invoke();
                            yield break;
                        }
                    }

                    AnimationInfo start = animationStations[lastIndex];
                    AnimationInfo end = animationStations[lastIndex - 1];

                    if (start.scaleAnimation && end.scaleAnimation)
                    {
                        scaleAnimationCoroutine = StartCoroutine(AnimationScale(start.scale, end.scale, start.finishTimeScale));
                    }

                    lastIndex--;
                }

                for (int i = 0; i < tweenPerformanceValue + 1; i++)
                {
                    yield return new WaitForEndOfFrame();
                }
            }
        }


        private IEnumerator AnimationScale(Vector3 start, Vector3 end, float finishTime)
        {
            Vector3 to = end;
            Vector3 currentScale = start;

            _myRect.localScale = currentScale;

            float Speed = Vector3.Distance(end, start) / finishTime;

            while (Vector3.Distance(currentScale, to) > 0.002f)//kaldirabilirim
            {
                currentScale = Vector3.MoveTowards(currentScale, to, Speed * Time.deltaTime);
                _myRect.localScale = currentScale;

                if (layout != null)
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(layout);
                    LayoutRebuilder.MarkLayoutForRebuild(layout);
                }
                yield return new WaitForEndOfFrame();
            }
            _myRect.localScale = to;
            scaleAnimationCoroutine = null;
        }
    }
}

