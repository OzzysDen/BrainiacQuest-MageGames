using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerUIAnimator
{
    public class GOEventManager : MonoBehaviour
    {
        [Tooltip("0 ise obje kapanmaz")]
        public float CloseAfterSeconds = 0;
        [Tooltip("onEnable eventinin calismasini geciktirir.")]
        public float onEnableLatency = 0;

        public UnityEvent onEnable;
        public UnityEvent onClose;

        private void OnEnable()
        {
            if (onEnableLatency == 0)
            {
                onEnable.Invoke();
            }
            else
            {
                StartCoroutine(IEOnEnableLatency());
            }

            if (CloseAfterSeconds > 0)
                StartCoroutine(Close());
        }

        private void OnDisable()
        {
            onClose.Invoke();
            StopAllCoroutines();
        }

        private IEnumerator Close()
        {
            yield return new WaitForSecondsRealtime(CloseAfterSeconds);
            gameObject.SetActive(false);
        }

        private IEnumerator IEOnEnableLatency()
        {
            yield return new WaitForSecondsRealtime(onEnableLatency);
            onEnable.Invoke();
        }
    }

}
