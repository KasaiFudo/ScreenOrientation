using System;
using UnityEngine;
using Screen = UnityEngine.Device.Screen;

namespace KasaiFudo.ScreenOrientation
{
    public class ScreenOrientationObserver
    {
        private IOrientationListener[] _listeners;

        public static BasicScreenOrientation CurrentOrientation { get; private set; }
        public static event Action<BasicScreenOrientation> OnOrientationChanged;
        public ScreenOrientationObserver(IOrientationListener[] listeners)
        {
            UpdateListeners(listeners);
        }

        public void UpdateListeners(IOrientationListener[] listeners)
        {
            _listeners = listeners;
            
            if (listeners != null && listeners.Length > 0)
                UpdateOrientation(GetBasicOrientationByScreenOrientation(Input.deviceOrientation));
        }

        public bool CheckOrientation()
        {
            if(_listeners == null || _listeners.Length == 0) return false;

            var newOrientation = GetBasicOrientationByScreenOrientation(Input.deviceOrientation);
            if (CurrentOrientation != newOrientation)
            {
                UpdateOrientation(newOrientation);
                return true;
            }
            
            return false;
        }

        private BasicScreenOrientation GetBasicOrientationByScreenOrientation(DeviceOrientation orientation)
        {
            return orientation switch
            {
                DeviceOrientation.Portrait => BasicScreenOrientation.Portrait,
                DeviceOrientation.PortraitUpsideDown => BasicScreenOrientation.Portrait,
                DeviceOrientation.LandscapeLeft => BasicScreenOrientation.Landscape,
                DeviceOrientation.LandscapeRight => BasicScreenOrientation.Landscape,
                _ => Screen.height > Screen.width ? BasicScreenOrientation.Portrait : BasicScreenOrientation.Landscape
            };
        }
        private void UpdateOrientation(BasicScreenOrientation newOrientation)
        {
            foreach (var orientationListener in _listeners)
            {
                orientationListener.OnOrientationChanged(newOrientation, CurrentOrientation);
            }
                
            Debug.Log($"[ScreenOrientationObserver]Orientation changed from {CurrentOrientation} to {newOrientation}");
            CurrentOrientation = newOrientation;
            OnOrientationChanged?.Invoke(CurrentOrientation);
        }
    }

    public enum BasicScreenOrientation
    {
        Portrait,
        Landscape
    }
}