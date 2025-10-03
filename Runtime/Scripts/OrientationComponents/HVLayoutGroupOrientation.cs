using System;
using UnityEngine;
using UnityEngine.UI;

namespace KasaiFudo.ScreenOrientation
{
    public class HVLayoutGroupOrientation : OrientationAwareComponent
    {
        [Serializable]
        private struct HVLayoutGroupStruct
        {
            [field: SerializeField] public bool IsHorizontal { get; private set; }
            [field: SerializeField] public RectOffset Padding { get; private set; }
            [field: SerializeField] public TextAnchor ChildAlignment { get; private set; }
            [field: SerializeField] public float Spacing { get; private set; }
            [field: SerializeField] public bool ChildForceExpandWidth { get; private set; }
            [field: SerializeField] public bool ChildForceExpandHeight { get; private set; }
            [field:SerializeField] public bool ControlChildSizeWidth { get; private set; }
            [field:SerializeField] public bool ControlChildSizeHeight { get; private set; }

            public HVLayoutGroupStruct(HorizontalOrVerticalLayoutGroup g)
            {
                IsHorizontal = g is HorizontalLayoutGroup;
                Padding = new RectOffset(g.padding.left, g.padding.right, g.padding.top, g.padding.bottom);
                ChildAlignment = g.childAlignment;
                Spacing = g.spacing;
                ChildForceExpandWidth = g.childForceExpandWidth;
                ChildForceExpandHeight = g.childForceExpandHeight;
                ControlChildSizeHeight = g.childControlHeight;
                ControlChildSizeWidth = g.childControlWidth;
            }
        }

        [SerializeField] private HVLayoutGroupStruct _portraitData;
        [SerializeField] private HVLayoutGroupStruct _landscapeData;

        private HorizontalOrVerticalLayoutGroup _layoutGroup;
        public HorizontalOrVerticalLayoutGroup LayoutGroup
        {
            get
            {
                if (_layoutGroup == null) _layoutGroup = GetComponent<HorizontalOrVerticalLayoutGroup>();
                return _layoutGroup;
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

        private void Reset()
        {
            RewriteLandscapeLayoutData();
            RewritePortraitLayoutData();
        }

        private void OnValidate()
        {
            if (!gameObject.TryGetComponent(out HorizontalOrVerticalLayoutGroup g))
            {
                throw new Exception("LayoutGroup must be either HorizontalLayoutGroup or VerticalLayoutGroup");
            }
        }

        protected override void ChangeOrientationImmediate(BasicScreenOrientation orientation)
        {
            var data = (HVLayoutGroupStruct)GetTargetValues(orientation);
            
            ChangeHVState(data.IsHorizontal);
            ApplyLayoutGroupValues(data.Padding, data.ChildAlignment, data.Spacing, data.ChildForceExpandWidth, data.ChildForceExpandHeight, data.ControlChildSizeWidth, data.ControlChildSizeHeight);
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
            var end = (HVLayoutGroupStruct)endValues;
            
            ChangeHVState(end.IsHorizontal);
        }

        protected override void ApplyInterpolatedValues(object startValues, object endValues, float t)
        {
            var start = (HVLayoutGroupStruct)startValues;
            var end = (HVLayoutGroupStruct)endValues;

            var newPadding = new RectOffset
            (
                Mathf.RoundToInt(Mathf.Lerp(start.Padding.left, end.Padding.left, t)),
                Mathf.RoundToInt(Mathf.Lerp(start.Padding.right, end.Padding.right, t)),
                Mathf.RoundToInt(Mathf.Lerp(start.Padding.top, end.Padding.top, t)),
                Mathf.RoundToInt(Mathf.Lerp(start.Padding.bottom, end.Padding.bottom, t))
            );
            
            var childAlignment = t < 0.5f ? start.ChildAlignment : end.ChildAlignment;
            var spacing = Mathf.Lerp(start.Spacing, end.Spacing, t);
            var childForceExpandWidth = t < 0.5f ? start.ChildForceExpandWidth : end.ChildForceExpandWidth;
            var childForceExpandHeight = t < 0.5f ? start.ChildForceExpandHeight : end.ChildForceExpandHeight;
            var controlChildSizeWidth = t < 0.5f ? start.ControlChildSizeWidth : end.ControlChildSizeWidth;
            var controlChildSizeHeight = t < 0.5f ? start.ControlChildSizeHeight : end.ControlChildSizeHeight;

            ApplyLayoutGroupValues(newPadding, childAlignment, spacing, childForceExpandWidth, childForceExpandHeight, controlChildSizeWidth, controlChildSizeHeight);
        }

        private HVLayoutGroupStruct GetCurrentValues()
        {
            return new HVLayoutGroupStruct(LayoutGroup);
        }

        private void ChangeHVState(bool isHorizontal)
        {
            if (isHorizontal && !(LayoutGroup is HorizontalLayoutGroup))
            {
                DestroyImmediate(LayoutGroup);
                _layoutGroup = gameObject.AddComponent<HorizontalLayoutGroup>();
            }
            else if (!isHorizontal && !(LayoutGroup is VerticalLayoutGroup))
            {
                DestroyImmediate(LayoutGroup);
                _layoutGroup = gameObject.AddComponent<VerticalLayoutGroup>();
            }
        }

        private void ApplyLayoutGroupValues(RectOffset padding, TextAnchor childAlignment, float spacing, bool childForceExpandWidth, bool childForceExpandHeight, bool controlChildSizeWidth, bool controlChildSizeHeight)
        {
            LayoutGroup.padding = new RectOffset(
                padding.left,
                padding.right,
                padding.top,
                padding.bottom);

            LayoutGroup.childAlignment = childAlignment;
            LayoutGroup.spacing = spacing;
            LayoutGroup.childForceExpandWidth = childForceExpandWidth;
            LayoutGroup.childForceExpandHeight = childForceExpandHeight;
            LayoutGroup.childControlWidth = controlChildSizeWidth;
            LayoutGroup.childControlHeight = controlChildSizeHeight;

            LayoutRebuilder.MarkLayoutForRebuild((RectTransform)LayoutGroup.transform);
        }
    }
}
