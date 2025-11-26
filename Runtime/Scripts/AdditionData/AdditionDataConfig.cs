using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KasaiFudo.ScreenOrientation
{
    public abstract class AdditionDataConfig<TData> : ScriptableObject
    {
        [SerializeField] private List<OrientationData<TData>> _orientationDatas;
        
        private Dictionary<string, OrientationData<TData>> _dictionaryDatas;

        private IReadOnlyDictionary<string, OrientationData<TData>> Datas
        {
            get
            {
                if (_dictionaryDatas == null)
                {
                    _dictionaryDatas = new Dictionary<string, OrientationData<TData>>();
                    _dictionaryDatas = _orientationDatas.ToDictionary(d => d.Key, d => d);
                }
                
                return _dictionaryDatas;
            }
        }
        
        public TData GetOrientationData(BasicScreenOrientation orientation)
        {
            if(!Datas.TryGetValue(AdditionDataKey.Key, out var data))
                throw new KeyNotFoundException($"Key {AdditionDataKey.Key} was not found in addition data list.");
            
            return orientation == BasicScreenOrientation.Portrait 
                ? data.Portrait : data.Landscape;
        }
    }
    
    [Serializable]
    public struct OrientationData<TData>
    {
        [field: SerializeField] public string Key { get; private set; }
        [field: SerializeField] public TData Portrait { get; private set; }
        [field: SerializeField] public TData Landscape { get; private set; }

        public OrientationData(string key, TData portrait, TData landscape)
        {
            Key = key;
            Portrait = portrait;
            Landscape = landscape;
        }
    }
}