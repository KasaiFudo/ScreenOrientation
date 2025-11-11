using System;
using KasaiFudo.ScreenOrientation;
using UnityEngine;
using UnityEngine.UI;

namespace MainAssets.Scripts
{
    [RequireComponent(typeof(CanvasScaler))]
    public class CanvasScalerOrientation : AnimateOrientationAwareComponent
    {
        [Serializable]
        private class CanvasScalerStruct
        {
            [field: SerializeField] public CanvasScaler.ScaleMode UIScaleMode {get; private set;}
            [field: SerializeField] public Vector2 ReferenceResolution { get; private set; }
            
            [field: SerializeField] public CanvasScaler.ScreenMatchMode ScreenMatchMode {get; private set;}
            [field: SerializeField, Range(0, 1)] public float MatchWidthOrHeight { get; private set; }
            [field: SerializeField] public float ReferencePixelPerUnit { get; private set; }

            public CanvasScalerStruct()
            {
                UIScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                ReferenceResolution = new Vector2(1080, 1920);
                
                ScreenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                MatchWidthOrHeight = 0.5f;
                ReferencePixelPerUnit = 100; 
            }

            public CanvasScalerStruct(CanvasScaler canvasScaler)
            {
                UIScaleMode = canvasScaler.uiScaleMode;
                ReferenceResolution = canvasScaler.referenceResolution;
                
                ScreenMatchMode = canvasScaler.screenMatchMode;
                MatchWidthOrHeight = canvasScaler.matchWidthOrHeight;
                ReferencePixelPerUnit = canvasScaler.referencePixelsPerUnit;
            }
        }
        
        [SerializeField] private CanvasScalerStruct _portraitData;
        [SerializeField] private CanvasScalerStruct _landscapeData;

        private CanvasScaler _canvasScaler;
        
        public CanvasScaler CanvasScaler
        {
            get
            {
                if(_canvasScaler == null) _canvasScaler = GetComponent<CanvasScaler>();
                
                return _canvasScaler;
            }
        }
        
        [ContextMenu("Rewrite Portrait Layout Data")]
        public void RewritePortraitLayoutData()
        {
            _portraitData = GetCurrentValues();
        }

        [ContextMenu("Rewrite Landscape Layout Data")]
        public void RewriteLandscapeLayoutData()
        {
            _landscapeData = GetCurrentValues();
        }

        protected override void ChangeOrientationImmediate(BasicScreenOrientation orientation)
        {
            var data = (CanvasScalerStruct)GetTargetValues(orientation);
            
            ApplyValues(data.UIScaleMode, data.ScreenMatchMode, data.MatchWidthOrHeight, data.ReferencePixelPerUnit);
        }

        protected override object GetCurrentValues(BasicScreenOrientation oldOrientation)
        {
            return GetTargetValues(oldOrientation);
        }

        protected override object GetTargetValues(BasicScreenOrientation orientation)
        {
            return orientation == BasicScreenOrientation.Portrait ? _portraitData : _landscapeData;
        }

        protected override void ApplyInterpolatedValues(object startValues, object endValues, float t)
        {
            var start = (CanvasScalerStruct)startValues;
            var end = (CanvasScalerStruct)endValues;
            
            var interpolatedUIScaleMode = end.UIScaleMode;
            var interpolatedScreenMatchMode = end.ScreenMatchMode;
            var interpolatedMatchWidthOrHeight = Mathf.Lerp(start.MatchWidthOrHeight, end.MatchWidthOrHeight, t);
            var interpolatedReferencePixelPerUnit = Mathf.Lerp(start.ReferencePixelPerUnit, end.ReferencePixelPerUnit, t);
            
            ApplyValues(interpolatedUIScaleMode, interpolatedScreenMatchMode, interpolatedMatchWidthOrHeight, interpolatedReferencePixelPerUnit);
        }

        private void Reset()
        {
            _portraitData = new CanvasScalerStruct();
            _landscapeData = new CanvasScalerStruct();
        }

        private CanvasScalerStruct GetCurrentValues()
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