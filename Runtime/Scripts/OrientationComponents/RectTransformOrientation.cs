using System;
using UnityEngine;

namespace KasaiFudo.ScreenOrientation
{
    [RequireComponent(typeof(RectTransform))]
    public class RectTransformOrientation : AnimatedOrientationAwareble<RectTransformOrientation.RectTransformStruct>
    {
        [Serializable]
        public struct RectTransformStruct
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
        
        private RectTransform _rectTransform;
        
        public RectTransform RectTransform
        {
            get
            {
                if(_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
                
                return _rectTransform;
            }
        }

        protected override void OnEndAnimation(RectTransformStruct startValues, RectTransformStruct endValues)
        {
            Canvas.ForceUpdateCanvases();
        }

        protected override void ApplyImmediate(RectTransformStruct data)
        {
            RectTransform.anchoredPosition = data.AnchoredPosition;
            RectTransform.anchorMin = data.AnchorMin;
            RectTransform.anchorMax = data.AnchorMax;
            RectTransform.sizeDelta = data.SizeDelta;
            
            Canvas.ForceUpdateCanvases();
        }

        protected override RectTransformStruct GetCurrentValues()
        {
            return new RectTransformStruct(RectTransform);
        }

        protected override void ApplyInterpolated(RectTransformStruct start, RectTransformStruct end, float t)
        {
            RectTransform.anchoredPosition = Vector3.Lerp(start.AnchoredPosition, end.AnchoredPosition, t);
            RectTransform.anchorMin = Vector2.Lerp(start.AnchorMin, end.AnchorMin, t);
            RectTransform.anchorMax = Vector2.Lerp(start.AnchorMax, end.AnchorMax, t);
            RectTransform.sizeDelta = Vector2.Lerp(start.SizeDelta, end.SizeDelta, t);
        }
    }
}
