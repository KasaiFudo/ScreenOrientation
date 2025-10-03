using System;
using System.Collections;
using UnityEngine;

namespace KasaiFudo.ScreenOrientation
{
    public abstract class OrientationAwareComponent : MonoBehaviour, IOrientationListener
    {
        [Serializable]
        private struct AnimationData
        {
            [field: SerializeField] public bool IsAnimated;
            [Tooltip("Delay To Animate controlled by general config as default if 0, but you can rewrite there your value")]
            [Min(0)] [field: SerializeField] public float DelayToAnimate;
            [Min(0)] [field: SerializeField] public float TransitionDuration;
            [field: SerializeField] public AnimationCurve TransitionCurve;
        }
        
        [SerializeField] private AnimationData _animationData;
        
        private Coroutine _animateCoroutine;

        public void Initialize(float animationDelay)
        {
            _animationData.DelayToAnimate = _animationData.DelayToAnimate == 0 ? animationDelay : _animationData.DelayToAnimate;
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
        
        protected abstract void ChangeOrientationImmediate(BasicScreenOrientation orientation);
        protected abstract object GetCurrentValues(BasicScreenOrientation oldOrientation);
        protected abstract object GetTargetValues(BasicScreenOrientation orientation);

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