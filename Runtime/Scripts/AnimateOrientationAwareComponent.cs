using System;
using System.Collections;
using UnityEngine;

namespace KasaiFudo.ScreenOrientation
{
    public abstract class AnimateOrientationAwareComponent : OrientationAwareComponent, IOrientationListener
    {
        [Serializable]
        public struct AnimationData
        {
            [SerializeField] public bool IsAnimated;

            [Tooltip("Override default delay from global config")]
            [SerializeField] public bool OverrideDelayToAnimate;

            [Tooltip("Custom delay when override is ON")]
            [Min(0)] [SerializeField] public float DelayToAnimate;

            [Min(0)] [SerializeField] public float TransitionDuration;
            [SerializeField] public AnimationCurve TransitionCurve;
            
            public AnimationData(bool isAnimated)
            {
                IsAnimated = isAnimated;
                OverrideDelayToAnimate = false;
                DelayToAnimate = 0f;
                TransitionDuration = 0.5f;
                TransitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
            }
        }
        
        [SerializeField] private AnimationData _animationData;
        
        private Coroutine _animateCoroutine;

        public void Initialize(float animationDelay)
        {
            _animationData.DelayToAnimate = _animationData.OverrideDelayToAnimate ? _animationData.DelayToAnimate : animationDelay;
        }

        private void Reset()
        {
            _animationData = new AnimationData(false);
        }

        private void OnEnable()
        {
            ChangeOrientationImmediate(ScreenOrientationObserver.CurrentOrientation);
        }

        public void OnOrientationChanged(BasicScreenOrientation newOrientation, BasicScreenOrientation oldOrientation)
        {
            if (_animationData.IsAnimated && gameObject.activeInHierarchy)
            {
                if (_animateCoroutine != null)
                {
                    StopCoroutine(_animateCoroutine);
                }
                
                _animateCoroutine = StartCoroutine(AnimateOrientationChange(newOrientation, oldOrientation));
            }
            else
                ChangeOrientationImmediate(newOrientation);
        }
        
        [ContextMenu("Apply Portrait Data")]
        public void ApplyPortraitData()
        {
            ChangeOrientationImmediate(BasicScreenOrientation.Portrait);
        }
        
        [ContextMenu("Apply Landscape Data")]
        public void ApplyLandscapeData()
        {
            ChangeOrientationImmediate(BasicScreenOrientation.Landscape);
        }

        protected virtual void OnStartAnimation(object startValues, object endValues)
        {
            
        }
        
        protected virtual void OnEndAnimation(object startValues, object endValues)
        {
            
        }
        
        protected virtual void ApplyInterpolatedValues(object startValues, object endValues, float t)
        {
            throw new NotImplementedException("As default method is not implemented. If you want to use animations, " +
                                              "please override ApplyInterpolatedValues method in your class or simply not use animations");
        }
        private IEnumerator AnimateOrientationChange(BasicScreenOrientation newOrientation, BasicScreenOrientation oldOrientation)
        {
            float elapsed = 0f;
            
            var startValues = GetCurrentValues(oldOrientation);
            var endValues = GetTargetValues(newOrientation);
            
            yield return new WaitForSeconds(_animationData.DelayToAnimate);

            OnStartAnimation(startValues, endValues);
            
            while (elapsed < _animationData.TransitionDuration)
            {
                elapsed += Time.deltaTime;
                float t = _animationData.TransitionCurve.Evaluate(elapsed / _animationData.TransitionDuration);
                
                ApplyInterpolatedValues(startValues, endValues, t);
            
                yield return null;
            }

            OnEndAnimation(startValues, endValues);
            _animateCoroutine = null;
        }

    }

    

    public interface IOrientationListener
    {
        void OnOrientationChanged(BasicScreenOrientation newOrientation, BasicScreenOrientation oldOrientation);
    }
}