using System;
using UnityEngine;
using UnityEngine.UI;

namespace KasaiFudo.ScreenOrientation
{
    [RequireComponent(typeof(LayoutElement))]
    public class LayoutElementOrientation : AnimatedOrientationAwareble<LayoutElementOrientation.LayoutElementStruct>
    {
        [Serializable]
        public struct LayoutElementStruct
        {
            [field: SerializeField] public bool IgnoreLayout {get; private set;}
            [field: SerializeField] public float MinWidth { get; private set; }
            [field: SerializeField] public float MinHeight {get; private set;}
            [field: SerializeField] public float PreferredWidth {get; private set;}
            [field: SerializeField] public float PreferredHeight {get; private set;}
            [field: SerializeField] public float FlexibleWidth {get; private set;}
            [field: SerializeField] public float FlexibleHeight {get; private set;}

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
        private LayoutElement _layoutElement;
        
        public LayoutElement LayoutElement
        {
            get
            {
                if(_layoutElement == null) _layoutElement = GetComponent<LayoutElement>();
                
                return _layoutElement;
            }
        }

        protected override void ApplyInterpolated(LayoutElementStruct start, LayoutElementStruct end, float t)
        {
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

        protected override void ApplyImmediate(LayoutElementStruct data)
        {
            ApplyLayoutGroupValues(data.IgnoreLayout, data.MinWidth, data.MinHeight, data.PreferredWidth, data.PreferredHeight,
                data.FlexibleWidth, data.FlexibleHeight);
        }

        protected override LayoutElementStruct GetCurrentValues()
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
