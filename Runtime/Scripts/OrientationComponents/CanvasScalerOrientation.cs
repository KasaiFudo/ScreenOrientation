using System;
using UnityEngine;
using UnityEngine.UI;

namespace KasaiFudo.ScreenOrientation
{
    [RequireComponent(typeof(CanvasScaler))]
    public class CanvasScalerOrientation : AnimatedOrientationAwareble<CanvasScalerOrientation.CanvasScalerStruct>
    {
        [Serializable]
        public struct CanvasScalerStruct
        {
            [field: SerializeField] public CanvasScaler.ScaleMode UIScaleMode {get; private set;}
            [field: SerializeField] public Vector2 ReferenceResolution { get; private set; }
            
            [field: SerializeField] public CanvasScaler.ScreenMatchMode ScreenMatchMode {get; private set;}
            [field: SerializeField, Range(0, 1)] public float MatchWidthOrHeight { get; private set; }
            [field: SerializeField] public float ReferencePixelPerUnit { get; private set; }

            /*public CanvasScalerStruct()
            {
                UIScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                ReferenceResolution = new Vector2(1080, 1920);
                
                ScreenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                MatchWidthOrHeight = 0.5f;
                ReferencePixelPerUnit = 100; 
            }*/

            public CanvasScalerStruct(CanvasScaler canvasScaler)
            {
                UIScaleMode = canvasScaler.uiScaleMode;
                ReferenceResolution = canvasScaler.referenceResolution;
                
                ScreenMatchMode = canvasScaler.screenMatchMode;
                MatchWidthOrHeight = canvasScaler.matchWidthOrHeight;
                ReferencePixelPerUnit = canvasScaler.referencePixelsPerUnit;
            }
        }

        private CanvasScaler _canvasScaler;
        
        public CanvasScaler CanvasScaler
        {
            get
            {
                if(_canvasScaler == null) _canvasScaler = GetComponent<CanvasScaler>();
                
                return _canvasScaler;
            }
        }

        protected override void ApplyInterpolated(CanvasScalerStruct start, CanvasScalerStruct end, float t)
        {
            var interpolatedUIScaleMode = end.UIScaleMode;
            var interpolatedScreenMatchMode = end.ScreenMatchMode;
            var interpolatedMatchWidthOrHeight = Mathf.Lerp(start.MatchWidthOrHeight, end.MatchWidthOrHeight, t);
            var interpolatedReferencePixelPerUnit = Mathf.Lerp(start.ReferencePixelPerUnit, end.ReferencePixelPerUnit, t);
            
            ApplyValues(interpolatedUIScaleMode, interpolatedScreenMatchMode, interpolatedMatchWidthOrHeight, interpolatedReferencePixelPerUnit);

        }

        protected override void ApplyImmediate(CanvasScalerStruct data)
        {
            ApplyValues(data.UIScaleMode, data.ScreenMatchMode, data.MatchWidthOrHeight, data.ReferencePixelPerUnit);
        }

        protected override CanvasScalerStruct GetCurrentValues()
        {
            return new CanvasScalerStruct(CanvasScaler);
        }
        
        private void ApplyValues(CanvasScaler.ScaleMode dataUIScaleMode, CanvasScaler.ScreenMatchMode dataScreenMatchMode, float dataMatchWidthOrHeight, float dataReferencePixelPerUnit)
        {
            CanvasScaler.uiScaleMode = dataUIScaleMode;
            CanvasScaler.screenMatchMode = dataScreenMatchMode;
            CanvasScaler.matchWidthOrHeight = dataMatchWidthOrHeight;
            CanvasScaler.referencePixelsPerUnit = dataReferencePixelPerUnit;
            
            LayoutRebuilder.MarkLayoutForRebuild((RectTransform)CanvasScaler.transform);
        }
    }
}