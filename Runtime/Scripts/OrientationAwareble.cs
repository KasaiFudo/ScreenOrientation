using UnityEngine;

namespace KasaiFudo.ScreenOrientation
{
    public abstract class OrientationAwareble<TData> : MonoBehaviour, IOrientationListener
    {
        [SerializeField] protected TData _portrait;
        [SerializeField] protected TData _landscape;
        private void OnEnable()
        {
            ApplyImmediate(ScreenOrientationObserver.CurrentOrientation);
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
            ApplyImmediate(o == BasicScreenOrientation.Portrait ? _portrait : _landscape);
        }

        protected virtual TData GetValuesByOrientation(BasicScreenOrientation orientation)
        {
            return orientation == BasicScreenOrientation.Landscape ? _landscape : _portrait;
        }

        protected abstract void ApplyImmediate(TData data);
        
        protected abstract TData GetCurrentValues();
    }
}