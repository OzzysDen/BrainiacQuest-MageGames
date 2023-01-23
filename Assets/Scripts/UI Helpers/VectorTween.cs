using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PlayerUIAnimator
{
    public class VectorTween : Tween.TweenBase
    {
        [System.Serializable]
        public class AnimationInfo
        {
            public bool positionAnimation = true;
            public Vector3 position;

            public bool rotationAnimation = true;
            public Vector3 rotation;

            public bool scaleAnimation = true;
            public Vector3 scale = new Vector3(1, 1, 1);

            [Tooltip("Bitirme zamani, saniye cinsinden")]
            public float finishTimePosition = 60f;
            [Tooltip("Bitirme zamani, saniye cinsinden")]
            public float finishTimeRotation = 60f;
            [Tooltip("Bitirme zamani, saniye cinsinden")]
            public float finishTimeScale = 60f;
        }
        [NonReorderable]
        public AnimationInfo[] animationStations;

        public bool awakePlay;
        public bool loop;
        [Space]

        public RectTransform targetObjeRectTransform;
        [Space]

        [Header("Play ve PlayBack methodlarin basinda calisir.")]
        public UnityEvent onStart;
        public UnityEvent onLoopStart;
        public UnityEvent onFinish;

        [Space]

        public RectTransform layout;


        private Coroutine animationManageCoroutine;
        private Coroutine positionAnimationCoroutine;
        private Coroutine rotationAnimationCoroutine;
        private Coroutine scaleAnimationCoroutine;

        private void Awake()
        {
            if (targetObjeRectTransform == null)
                targetObjeRectTransform = GetComponent<RectTransform>();

            if (awakePlay)
            {
                Play();
            }
        }

        private void OnEnable()
        {
            if (targetObjeRectTransform != null && awakePlay)
            {
                Play();
            }
        }

        public void Set(RectTransform rect)
        {
            layout = rect;
        }

        public override void Play()
        {
            if (!gameObject.activeInHierarchy)
                return;

            if (animationManageCoroutine != null)
            {
                StopCoroutine(animationManageCoroutine);
                animationManageCoroutine = null;

                if (positionAnimationCoroutine != null)
                {
                    StopCoroutine(positionAnimationCoroutine);
                    positionAnimationCoroutine = null;

                }

                if (rotationAnimationCoroutine != null)
                {
                    StopCoroutine(rotationAnimationCoroutine);
                    rotationAnimationCoroutine = null;
                }

                if (scaleAnimationCoroutine != null)
                {
                    StopCoroutine(scaleAnimationCoroutine);
                    scaleAnimationCoroutine = null;
                }
            }

            animationManageCoroutine = StartCoroutine(AnimationManage());
        }

        public override void Stop()
        {
            StopAllCoroutines();

            animationManageCoroutine = null;
            positionAnimationCoroutine = null;
            rotationAnimationCoroutine = null;
            scaleAnimationCoroutine = null;
        }

        /// <summary>
        /// Hazirlanan animasyonu ters yonde calistirir
        /// </summary>
        public override void PlayBack(bool setDeactive = false)
        {
            if (!gameObject.activeInHierarchy)
                return;

            if (animationManageCoroutine != null)
            {
                StopCoroutine(animationManageCoroutine);
                animationManageCoroutine = null;

                if (positionAnimationCoroutine != null)
                {
                    StopCoroutine(positionAnimationCoroutine);
                    positionAnimationCoroutine = null;

                }

                if (rotationAnimationCoroutine != null)
                {
                    StopCoroutine(rotationAnimationCoroutine);
                    rotationAnimationCoroutine = null;
                }

                if (scaleAnimationCoroutine != null)
                {
                    StopCoroutine(scaleAnimationCoroutine);
                    scaleAnimationCoroutine = null;
                }
            }

            animationManageCoroutine = StartCoroutine(AnimationManageBack(setDeactive));
        }

        private IEnumerator AnimationManage()
        {
            onStart.Invoke();
            int lastIndex = -1;
            while (true)
            {
                if (positionAnimationCoroutine == null && rotationAnimationCoroutine == null && scaleAnimationCoroutine == null)
                {
                    if (lastIndex == -1)
                    {
                        if (onLoopStart != null)
                            onLoopStart.Invoke();

                        lastIndex = 0;
                    }
                    else
                    {
                        //     onFinish.Invoke();
                    }

                    if (lastIndex + 1 == animationStations.Length && !loop)
                    {
                        animationManageCoroutine = null;
                        if (onFinish != null)
                            onFinish.Invoke();
                        yield break;
                    }
                    else if (lastIndex + 1 == animationStations.Length && loop)
                    {
                        lastIndex = 0;

                        if (onLoopStart != null)
                            onLoopStart.Invoke();

                        if (onFinish != null)
                            onFinish.Invoke();
                    }

                    AnimationInfo start = animationStations[lastIndex];
                    AnimationInfo end = animationStations[lastIndex + 1];

                    if (start.positionAnimation && end.positionAnimation)
                    {
                        positionAnimationCoroutine = StartCoroutine(AnimationPosition(start.position, end.position, start.finishTimePosition));
                    }

                    if (start.rotationAnimation && end.rotationAnimation)
                    {
                        rotationAnimationCoroutine = StartCoroutine(AnimationRotation(start.rotation, end.rotation, start.finishTimeRotation));
                    }

                    if (start.scaleAnimation && end.scaleAnimation)
                    {
                        scaleAnimationCoroutine = StartCoroutine(AnimationScale(start.scale, end.scale, start.finishTimeScale));
                    }

                    lastIndex++;
                }

                yield return new WaitForEndOfFrame();
            }
        }

        /// <summary>
        /// Hazirlanan animasyonu ters yonde calistirmayi yonetir
        /// </summary>
        /// <returns></returns>
        private IEnumerator AnimationManageBack(bool setDeactive)
        {
            onStart.Invoke();
            int lastIndex = animationStations.Length - 1;
            while (true)
            {
                if (positionAnimationCoroutine == null && rotationAnimationCoroutine == null && scaleAnimationCoroutine == null)
                {
                    if (lastIndex == 0 && !loop)
                    {
                        animationManageCoroutine = null;
                        if (setDeactive)
                        {
                            gameObject.SetActive(false);
                        }

                        yield break;
                    }
                    else if (lastIndex == 0 && loop)
                    {
                        lastIndex = animationStations.Length - 1;
                    }

                    AnimationInfo start = animationStations[lastIndex];
                    AnimationInfo end = animationStations[lastIndex - 1];

                    if (start.positionAnimation && end.positionAnimation)
                    {
                        positionAnimationCoroutine = StartCoroutine(AnimationPosition(start.position, end.position, start.finishTimePosition));
                    }

                    if (start.rotationAnimation && end.rotationAnimation)
                    {
                        rotationAnimationCoroutine = StartCoroutine(AnimationRotation(start.rotation, end.rotation, start.finishTimeRotation));
                    }

                    if (start.scaleAnimation && end.scaleAnimation)
                    {
                        scaleAnimationCoroutine = StartCoroutine(AnimationScale(start.scale, end.scale, start.finishTimeScale));
                    }

                    lastIndex--;
                }

                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator AnimationPosition(Vector3 start, Vector3 end, float finishTime)
        {
            Vector3 to = end;
            Vector3 currentPos = start;

            targetObjeRectTransform.anchoredPosition = currentPos;

            float Speed = Vector3.Distance(end, start) / finishTime;

            while (Vector3.Distance(currentPos, to) > 0.2f)
            {
                currentPos = Vector3.MoveTowards(currentPos, to, Speed * Time.deltaTime);
                targetObjeRectTransform.anchoredPosition = currentPos;


                yield return new WaitForEndOfFrame();

                if (layout != null)
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(layout);
                    LayoutRebuilder.MarkLayoutForRebuild(layout);
                }
            }

            targetObjeRectTransform.anchoredPosition = to;

            for (int i = 0; i < tweenPerformanceValue + 1; i++)
            {
                yield return new WaitForEndOfFrame();
            }


            positionAnimationCoroutine = null;
        }

        private IEnumerator AnimationRotation(Vector3 start, Vector3 end, float finishTime)
        {
            Vector3 to = end;
            Vector3 currentRot = start;

            Quaternion q = targetObjeRectTransform.rotation;
            q.eulerAngles = currentRot;
            targetObjeRectTransform.rotation = q;

            float SpeedX = Mathf.Abs(currentRot.x - to.x) / finishTime;
            float SpeedY = Mathf.Abs(currentRot.y - to.y) / finishTime;
            float SpeedZ = Mathf.Abs(currentRot.z - to.z) / finishTime;

            while (Mathf.Abs(currentRot.x - to.x) > 0.2f || Mathf.Abs(currentRot.y - to.y) > 0.2f || Mathf.Abs(currentRot.z - to.z) > 0.2f)
            {
                currentRot.x = Mathf.MoveTowards(currentRot.x, to.x, SpeedX * Time.deltaTime);
                currentRot.y = Mathf.MoveTowards(currentRot.y, to.y, SpeedY * Time.deltaTime);
                currentRot.z = Mathf.MoveTowards(currentRot.z, to.z, SpeedZ * Time.deltaTime);

                q = targetObjeRectTransform.rotation;
                q.eulerAngles = currentRot;
                targetObjeRectTransform.rotation = q;

                yield return new WaitForEndOfFrame();
            }

            q = targetObjeRectTransform.rotation;
            q.eulerAngles = to;
            targetObjeRectTransform.rotation = q;

            for (int i = 0; i < tweenPerformanceValue + 1; i++)
            {
                yield return new WaitForEndOfFrame();
            }

            rotationAnimationCoroutine = null;
        }

        private IEnumerator AnimationScale(Vector3 start, Vector3 end, float finishTime)
        {

            Vector3 to = end;
            Vector3 currentScale = start;

            targetObjeRectTransform.localScale = currentScale;

            float Speed = Vector3.Distance(end, start) / finishTime;

            while (Vector3.Distance(currentScale, to) > 0.002f)//kaldirabilirim
            {
                currentScale = Vector3.MoveTowards(currentScale, to, Speed * Time.deltaTime);
                targetObjeRectTransform.localScale = currentScale;

                yield return new WaitForEndOfFrame();
            }

            targetObjeRectTransform.localScale = to;

            scaleAnimationCoroutine = null;

            for (int i = 0; i < tweenPerformanceValue + 1; i++)
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }

}
