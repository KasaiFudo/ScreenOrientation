using System;
using UnityEngine;

namespace KasaiFudo.ScreenOrientation
{
    public abstract class OrientationAwareble<TData> : MonoBehaviour, IOrientationListener
    {
        [SerializeField] protected TData _portrait;
        [SerializeField] protected TData _landscape;
        [SerializeField] private AdditionDataConfig<TData> _additionDataConfig;
        private void OnEnable()
        {
            if (_additionDataConfig != null)
                AdditionDataKey.OnKeyChanged += UpdateByAdditionKey;
            
            ApplyImmediate(ScreenOrientationObserver.CurrentOrientation);
        }

        private void OnDisable()
        {
            if (_additionDataConfig != null)
                AdditionDataKey.OnKeyChanged -= UpdateByAdditionKey;
        }

        public virtual void OnOrientationChanged(BasicScreenOrientation newOrientation)
        {
            ApplyImmediate(newOrientation);
        }

        [ContextMenu("Apply Portrait Data")]
        public void ApplyPortraitData()
        {
            ApplyImmediate(BasicScreenOrientation.Portrait);
        }

        [ContextMenu("Apply Landscape Data")]
        public void ApplyLandscapeData()
        {
            ApplyImmediate(BasicScreenOrientation.Landscape);
        }
        
        [ContextMenu("Rewrite Portrait Layout Data")]
        public void RewritePortraitLayoutData()
        {
            _portrait = GetCurrentValues();
        }

        [ContextMenu("Rewrite Landscape Layout Data")]
        public void RewriteLandscapeLayoutData()
        {
            _landscape = GetCurrentValues();
        }

        private void Reset()
        {
            RewriteLandscapeLayoutData();
            RewritePortraitLayoutData();
        }

        protected virtual void ApplyImmediate(BasicScreenOrientation o)
        {
            var data = _additionDataConfig != null ? _additionDataConfig.GetOrientationData(o) :
                o == BasicScreenOrientation.Portrait ? _portrait : _landscape;
            
            ApplyImmediate(data);
        }

        protected virtual TData GetValuesByOrientation(BasicScreenOrientation orientation)
        {
            return _additionDataConfig != null ? _additionDataConfig.GetOrientationData(orientation) :
                orientation == BasicScreenOrientation.Landscape ? _landscape : _portrait;
        }

        protected abstract void ApplyImmediate(TData data);
        
        protected abstract TData GetCurrentValues();
        
        private void UpdateByAdditionKey() => ApplyImmediate(BasicScreenOrientation.Portrait);
    }
}