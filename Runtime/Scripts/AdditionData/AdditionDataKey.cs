using System;
using UnityEngine;

namespace KasaiFudo.ScreenOrientation
{
    public static class AdditionDataKey
    {
        private static AdditionKeyConfig _config;
        private static string _key;
        
        private static AdditionKeyConfig Config
        {
            get
            {
                if (_config == null)
                    _config = Resources.Load<AdditionKeyConfig>("Configs/AdditionKeyConfig");

                return _config;
            }
        }

        public static string Key
        {
            get
            {
                return !string.IsNullOrWhiteSpace(_key) ? _key : Config != null ? Config.Key : "";
            }
            set
            {
                if (Config == null)
                {
                    Debug.LogError("AdditionKeyConfig not found in Resources!");
                    return;
                }

                if (string.IsNullOrWhiteSpace(value) || Config.Key == value)
                    return;

                Config.SetKey(value);
                _key = value;

#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(Config);
#endif

                OnKeyChanged?.Invoke();
            }
        }

        public static event Action OnKeyChanged;
    }
}