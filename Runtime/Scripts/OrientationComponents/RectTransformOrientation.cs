using System;
using UnityEngine;

namespace KasaiFudo.ScreenOrientation
{
    [RequireComponent(typeof(RectTransform))]
    public class RectTransformOrientation : OrientationAwareComponent
    {
        [Serializable]
        protected struct RectTransformStruct
        {
            [field: SerializeField]public Vector2 SizeDelta {get; private set; }
            [field: SerializeField]public Vector2 AnchoredPosition {get; private set; }
            [field: SerializeField]public Vector2 AnchorMin {get; private set; }
            [field: SerializeField]public Vector2 AnchorMax {get; private set; }
            
            public RectTransformStruct(RectTransform rectTransform)
            {
                SizeDelta = rectTransform.sizeDelta;
                AnchoredPosition = rectTransform.anchoredPosition;
                AnchorMin = rectTransform.anchorMin;
                AnchorMax = rectTransform.anchorMax;
            }
        }

        [SerializeField] private RectTransformStruct _portraitData;
        [SerializeField] private RectTransformStruct _landscapeData;
        
        private RectTransform _rectTransform;
        
        public RectTransform RectTransform
        {
            get
            {
                if(_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
                
                return _rectTransform;
            }
        }

        [ContextMenu("Rewrite Portrait Data")]
        public void RewritePortraitData()
        {
            _portraitData = GetCurrentValues();
        }

        [ContextMenu("Rewrite Landscape Data")]
        public void RewriteLandscapeData()
        {
            _landscapeData = GetCurrentValues();
        }

        protected override void ChangeOrientationImmediate(BasicScreenOrientation orientation)
        {
            var data = (RectTransformStruct)GetTargetValues(orientation);
            
            RectTransform.anchoredPosition = data.AnchoredPosition;
            RectTransform.anchorMin = data.AnchorMin;
            RectTransform.anchorMax = data.AnchorMax;
            RectTransform.sizeDelta = data.SizeDelta;
            
            Canvas.ForceUpdateCanvases();
        }

        protected override object GetCurrentValues(BasicScreenOrientation oldOrientation)
        {
            return GetTargetValues(oldOrientation);
        }

        protected override object GetTargetValues(BasicScreenOrientation orientation)
        {
            return orientation == BasicScreenOrientation.Portrait ? _portraitData : _landscapeData;
        }

        protected override void OnStartAnimation(object startValues, object endValues)
        {
            Canvas.ForceUpdateCanvases();
        }

        protected override void ApplyInterpolatedValues(object startValues, object endValues, float t)
        {
            var start = (RectTransformStruct)startValues;
            var end = (RectTransformStruct)endValues;
            
            RectTransform.anchoredPosition = Vector3.Lerp(start.AnchoredPosition, end.AnchoredPosition, t);
            RectTransform.anchorMin = Vector2.Lerp(start.AnchorMin, end.AnchorMin, t);
            RectTransform.anchorMax = Vector2.Lerp(start.AnchorMax, end.AnchorMax, t);
            RectTransform.sizeDelta = Vector2.Lerp(start.SizeDelta, end.SizeDelta, t);
        }

        protected override void OnEndAnimation(object startValues, object endValues)
        {
            Canvas.ForceUpdateCanvases();
        }

        private RectTransformStruct GetCurrentValues()
        {
            return new RectTransformStruct(RectTransform);
        }
    }
}
