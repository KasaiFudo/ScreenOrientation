using UnityEngine;

namespace KasaiFudo.ScreenOrientation
{
    public abstract class OrientationAwareComponent : MonoBehaviour, IOrientationListener
    {
        private void OnEnable()
        {
            ChangeOrientationImmediate(ScreenOrientationObserver.CurrentOrientation);
        }

        public void OnOrientationChanged(BasicScreenOrientation newOrientation, BasicScreenOrientation oldOrientation)
        {
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
    }
}
