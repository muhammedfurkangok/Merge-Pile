using System.Collections;
using UnityEngine;

namespace Runtime.Helpers
{
    public class ScaleAnim : MonoBehaviour
    {
        public AnimationCurve AnimCurve;
        public float AnimDelay;
        public float AnimTime;
        public Vector3 scaleValue;
        public bool shouldSaveStartScale = false;

        public bool looping;
        public bool onAnim;
        public bool onAwake;
        public bool onEnable;

        private Coroutine _ref;
        private Vector3 startScale;
        private void Start()
        {
            if(onAwake) StartScaleAnim();

            if (shouldSaveStartScale)
                startScale = transform.localScale;
        }

        private void OnEnable()
        {
            if (onEnable) StartScaleAnim();
        }

        private void OnDisable()
        {
            if (_ref != null)
            {
                StopCoroutine(_ref);
            }
            onAnim = false;
            transform.localScale = shouldSaveStartScale ? startScale : Vector3.one;
        }

        public void StartScaleAnim()
        {
            if (onAnim) return;
            _ref = StartCoroutine(ScaleAnimation(AnimDelay, AnimTime, transform.localScale, AnimCurve));
        }

        public IEnumerator ScaleAnimation(float delay, float time, Vector3 initScale, AnimationCurve curve)
        {
            onAnim = true;
            transform.localScale = shouldSaveStartScale ? startScale : Vector3.one;
        
            yield return new WaitForSeconds(delay);
            var passed = 0f;

            var targetScale = new Vector3(transform.localScale.x + scaleValue.x, transform.localScale.y + scaleValue.y,
                transform.localScale.z + scaleValue.z);

            while (passed < time)
            {
                passed += Time.deltaTime;
            
                var rate = curve.Evaluate(passed / time);
                transform.localScale = Vector3.LerpUnclamped(initScale,targetScale,rate);
                yield return null;
            }

            passed = 0;
        
            while (passed < time)
            {
                passed += Time.deltaTime;
            
                var rate = curve.Evaluate(passed / time);
                transform.localScale = Vector3.LerpUnclamped(targetScale,initScale,rate);
                yield return null;
            }

            onAnim = false;
        
            if (looping)
                StartScaleAnim();
        }
    }
}
