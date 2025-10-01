using System;
using UnityEngine;
using UnityEngine.UI;

namespace KasaiFudo.ScreenOrientation
{
    [RequireComponent(typeof(LayoutElement))]
    public class LayoutElementOrientation : OrientationAwareComponent
    {
        [Serializable]
        private class LayoutElementStruct
        {
            [field: SerializeField] public bool IgnoreLayout {get; private set;}
            [field: SerializeField] public float MinWidth { get; private set; }
            [field: SerializeField] public float MinHeight {get; private set;}
            [field: SerializeField] public float PreferredWidth {get; private set;}
            [field: SerializeField] public float PreferredHeight {get; private set;}
            [field: SerializeField] public float FlexibleWidth {get; private set;}
            [field: SerializeField] public float FlexibleHeight {get; private set;}

            public LayoutElementStruct()
            {
                IgnoreLayout = false;
                MinWidth = -1;
                MinHeight = -1;
                PreferredWidth = -1;
                PreferredHeight = -1;
                FlexibleWidth = -1;
                FlexibleHeight = -1;
            }

            public LayoutElementStruct(LayoutElement layoutElement)
            {
                IgnoreLayout = layoutElement.ignoreLayout;
                MinWidth = layoutElement.minWidth;
                MinHeight = layoutElement.minHeight;
                PreferredWidth = layoutElement.preferredWidth;
                PreferredHeight = layoutElement.preferredHeight;
                FlexibleWidth = layoutElement.flexibleWidth;
                FlexibleHeight = layoutElement.flexibleHeight;
            }
        }
        
        [SerializeField] private LayoutElementStruct _portraitData;
        [SerializeField] private LayoutElementStruct _landscapeData;

        private LayoutElement _layoutElement;
        
        public LayoutElement LayoutElement
        {
            get
            {
                if(_layoutElement == null) _layoutElement = GetComponent<LayoutElement>();
                
                return _layoutElement;
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
            var data = (LayoutElementStruct)GetTargetValues(orientation);
            
            ApplyLayoutGroupValues(data.IgnoreLayout, data.MinWidth, data.MinHeight, data.PreferredWidth, data.PreferredHeight, data.FlexibleWidth, data.FlexibleHeight);
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
            var start = (LayoutElementStruct)startValues;
            var end = (LayoutElementStruct)endValues;
            
            var interpolatedIgnoreLayout = Mathf.Lerp(start.IgnoreLayout ? 1f : 0f, end.IgnoreLayout ? 1f : 0f, t) > 0.5f;
            var interpolatedMinWidth = Mathf.Lerp(start.MinWidth, end.MinWidth, t);
            var interpolatedMinHeight = Mathf.Lerp(start.MinHeight, end.MinHeight, t);
            var interpolatedPreferredWidth = Mathf.Lerp(start.PreferredWidth, end.PreferredWidth, t);
            var interpolatedPreferredHeight = Mathf.Lerp(start.PreferredHeight, end.PreferredHeight, t);
            var interpolatedFlexibleWidth = Mathf.Lerp(start.FlexibleWidth, end.FlexibleWidth, t);
            var interpolatedFlexibleHeight = Mathf.Lerp(start.FlexibleHeight, end.FlexibleHeight, t);
            
            ApplyLayoutGroupValues(interpolatedIgnoreLayout, interpolatedMinWidth, interpolatedMinHeight,
                interpolatedPreferredWidth, interpolatedPreferredHeight, interpolatedFlexibleWidth, interpolatedFlexibleHeight);
        }

        private void Reset()
        {
            _portraitData = new LayoutElementStruct();
            _landscapeData = new LayoutElementStruct();
        }

        private LayoutElementStruct GetCurrentValues()
        {
            return new LayoutElementStruct(LayoutElement);
        }
        
        private void ApplyLayoutGroupValues(bool ignoreLayout, float minWidth, float minHeight, float preferredWidth, float preferredHeight, float flexibleWidth, float flexibleHeight)
        {
            LayoutElement.ignoreLayout = ignoreLayout;
            LayoutElement.minWidth = minWidth;
            LayoutElement.minHeight = minHeight;
            LayoutElement.preferredWidth = preferredWidth;
            LayoutElement.preferredHeight = preferredHeight;
            LayoutElement.flexibleWidth = flexibleWidth;
            LayoutElement.flexibleHeight = flexibleHeight;
            
            LayoutRebuilder.MarkLayoutForRebuild((RectTransform)LayoutElement.transform);
        }
    }
}
