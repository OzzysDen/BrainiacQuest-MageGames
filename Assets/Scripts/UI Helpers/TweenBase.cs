using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PlayerUIAnimator
{
    namespace Tween
    {
        public abstract class TweenBase : MonoBehaviour
        {
            [Range(0, 10)]
            [Tooltip("Sadece loop'da ise yarar.Deger ne kadar fazlaysa o kadar performans artar. Animasyon bozulmalarina neden olabilir.")]
            public int tweenPerformanceValue = 0;

            public virtual void Play()
            {

            }

            public virtual void PlayBack(bool setDeactive = false)
            {

            }

            public virtual void Stop()
            {
                StopAllCoroutines();
            }
        }
    }

}
