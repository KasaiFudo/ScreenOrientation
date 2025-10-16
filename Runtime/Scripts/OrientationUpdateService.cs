using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KasaiFudo.ScreenOrientation
{
    public class OrientationUpdateService : MonoBehaviour
    {
        private static OrientationConfig _orientationConfig;
        
        private static ScreenOrientationObserver _screenOrientationObserver;
        private static HashSet<IOrientationListener> _knownListeners = new HashSet<IOrientationListener>();
        private float _lastUpdatedTime;
        private float _lastListenerCheckTime;

        public static void Initialize()
        {
            _orientationConfig = Resources.Load<OrientationConfig>("Configs/OrientationConfig");
            
            _screenOrientationObserver = new ScreenOrientationObserver(InitializeListeners(out _).ToArray());
        }

        private void Update()
        {
            if(_screenOrientationObserver == null) return;
            
            if (_orientationConfig.DetectNewAwareComponentsInRuntime)
                UpdateListenersIfNeeded();

            if (_orientationConfig.IgnoreRapidChanges && Time.time - _lastUpdatedTime < _orientationConfig.RapidChangeThreshold) return;
            
            if (_screenOrientationObserver.CheckOrientation())
            {
                _lastUpdatedTime = Time.time;
            }
        }
        
        private void UpdateListenersIfNeeded()
        {
            if (Time.time - _lastListenerCheckTime < _orientationConfig.ListenerCheckInterval)
                return;
            
            _lastListenerCheckTime = Time.time;
        
            InitializeListeners(out var hasNewListeners);
            
            _knownListeners.RemoveWhere(listener => listener == null || (listener is MonoBehaviour mb && mb == null));
        
            if (hasNewListeners)
            {
                _screenOrientationObserver.UpdateListeners(_knownListeners.ToArray());
            }
        }

        private static HashSet<IOrientationListener> InitializeListeners(out bool hasNewListeners)
        {
            var components = FindObjectsByType<OrientationAwareComponent>(FindObjectsSortMode.None);
            hasNewListeners = false;
        
            foreach (var component in components)
            {
                if (component != null && component is IOrientationListener listener)
                {
                    if (_knownListeners.Add(listener))
                    {
                        Debug.Log("[OrientationUpdateService]New listeners found");
                        if(component is AnimateOrientationAwareComponent animatedComponent)
                            animatedComponent.Initialize(_orientationConfig.AnimationDelayForAllComponents);
                        
                        hasNewListeners = true;
                    }
                }
            }
        
            return _knownListeners;
        }
    }
}
