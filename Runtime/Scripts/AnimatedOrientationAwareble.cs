using System.Collections;
using UnityEngine;

namespace KasaiFudo.ScreenOrientation
{
    public abstract class AnimatedOrientationAwareble<TData> : OrientationAwareble<TData>, IAnimatedOrientationListener
    {
        [SerializeField] private AnimationData _animationData;

        private Coroutine _animateCoroutine;
        private BasicScreenOrientation _currentOrientation;

        public void Initialize(float animationDelay)
        {
            _animationData.DelayToAnimate = _animationData.OverrideDelayToAnimate ? _animationData.DelayToAnimate : animationDelay;
        }

        private void Reset()
        {
            _animationData = new AnimationData(false);
        }

        public override void OnOrientationChanged(BasicScreenOrientation newOrientation)
        {
            if(newOrientation == _currentOrientation)
                return;
            
            if (_animationData.IsAnimated && gameObject.activeInHierarchy)
            {
                var oldOrientation = _currentOrientation;
                
                if (_animateCoroutine != null)
                {
                    StopCoroutine(_animateCoroutine);
                }
                
                _animateCoroutine = StartCoroutine(AnimateOrientationChange(newOrientation, oldOrientation));
            }
            else
                ApplyImmediate(newOrientation);
            
            _currentOrientation = newOrientation;
        }

        protected virtual void OnStartAnimation(TData startValues, TData endValues)
        {
            
        }
        
        protected virtual void OnEndAnimation(TData startValues, TData endValues)
        {
            
        }
        private IEnumerator AnimateOrientationChange(BasicScreenOrientation newOrientation, BasicScreenOrientation oldOrientation)
        {
            float elapsed = 0f;
            
            var startValues = GetValuesByOrientation(oldOrientation);
            var endValues = GetValuesByOrientation(newOrientation);
            
            yield return new WaitForSeconds(_animationData.DelayToAnimate);

            OnStartAnimation(startValues, endValues);
            
            while (elapsed < _animationData.TransitionDuration)
            {
                elapsed += Time.deltaTime;
                float t = _animationData.TransitionCurve.Evaluate(elapsed / _animationData.TransitionDuration);
                
                ApplyInterpolated(startValues, endValues, t);
            
                yield return null;
            }

            OnEndAnimation(startValues, endValues);
            _animateCoroutine = null;
        }
        
        protected abstract void ApplyInterpolated(TData start, TData end, float t);
    }
}