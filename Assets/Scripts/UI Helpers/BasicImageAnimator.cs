using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PlayerUIAnimator
{
    [RequireComponent(typeof(Image))]
    public class BasicImageAnimator : MonoBehaviour
    {
        [System.Serializable]
        public class SpriteData
        {
            public Sprite sprite;
            [Tooltip("Sprite bekleme zamani.Saniye cinsinden")]
            public float waitTime = 1f;

            public UnityEvent _event;
        }

        public float defaultWaitTime = 0.15f;
        public bool autoStart = false;
        public bool loop = false;
        [NonReorderable]
        public SpriteData[] spriteDataList;

        public Sprite normalSprite;

        public UnityEvent animStartingEvent;
        public UnityEvent animStoppingEvent;

        private Coroutine animCoroutine;



        private void OnEnable()
        {
            // GetComponent<Image>().sprite = normalSprite;
            if (autoStart)
            {
                StartAnim();
            }
            else
            {
                StopAnim();
            }
        }


        public void StartAnim()
        {
            if (animCoroutine != null)
            {
                StopCoroutine(animCoroutine);
                animCoroutine = null;
            }

            try
            {
                if (gameObject.activeInHierarchy)
                {
                    animStartingEvent?.Invoke();
                    animCoroutine = StartCoroutine(AnimationCoroutineMethod());//Gameobject kapaninca hata veriyor
                }
            }
            catch { }

        }

        public void StopAnim()
        {
            if (animCoroutine != null)
            {
                StopCoroutine(animCoroutine);
                animCoroutine = null;
            }

            animStoppingEvent?.Invoke();
            GetComponent<Image>().sprite = normalSprite;
        }
        private IEnumerator AnimationCoroutineMethod()
        {
            yield return new WaitForEndOfFrame();

            int currentArrayIndex = 0;
            Image image = GetComponent<Image>();
            while (currentArrayIndex < spriteDataList.Length)
            {
                float time = spriteDataList[currentArrayIndex].waitTime;
                if (time == 0)
                {
                    time = defaultWaitTime;
                }
                image.sprite = spriteDataList[currentArrayIndex].sprite;

                spriteDataList[currentArrayIndex]?._event.Invoke();
                yield return new WaitForSeconds(time);

                currentArrayIndex++;
            }

            animCoroutine = null;

            if (loop)
            {
                StartAnim();
            }
        }
    }




}
