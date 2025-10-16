using System;
using UnityEngine;
using UnityEngine.UI;

namespace KasaiFudo.ScreenOrientation
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class GirdLayoutGroupAnimateOrientation : AnimateOrientationAwareComponent
    {
        [Serializable]
        private struct GridLayoutGroupStruct
        {
            [field: SerializeField] public Vector2 CellSize { get; private set; }
            [field: SerializeField] public Vector2 Spacing { get; private set; }
            [field: SerializeField] public GridLayoutGroup.Constraint Constraint { get; private set; }
            [field: SerializeField] public int ConstraintCount { get; private set; }
            [field: SerializeField] public GridLayoutGroup.Corner StartCorner { get; private set; }
            [field: SerializeField] public GridLayoutGroup.Axis StartAxis { get; private set; }
            [field: SerializeField] public RectOffset Padding { get; private set; }

            public GridLayoutGroupStruct(GridLayoutGroup grid)
            {
                CellSize = grid.cellSize;
                Spacing = grid.spacing;
                Constraint = grid.constraint;
                ConstraintCount = grid.constraintCount;
                StartCorner = grid.startCorner;
                StartAxis = grid.startAxis;
                Padding = new RectOffset(grid.padding.left, grid.padding.right, grid.padding.top, grid.padding.bottom);
            }
        }
        
        [SerializeField] private GridLayoutGroupStruct _portraitData;
        [SerializeField] private GridLayoutGroupStruct _landscapeData;

        private GridLayoutGroup _layoutGroup;
        public GridLayoutGroup LayoutGroup
        {
            get
            {
                if (_layoutGroup == null) _layoutGroup = GetComponent<GridLayoutGroup>();
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
            if (!gameObject.TryGetComponent(out GridLayoutGroup g))
            {
                throw new Exception("LayoutGroup must be a GridLayoutGroup");
            }
        }

        protected override void ChangeOrientationImmediate(BasicScreenOrientation orientation)
        {
            var data = (GridLayoutGroupStruct)GetTargetValues(orientation);
            ApplyLayoutGroupValues(data.Padding, data.CellSize, data.Spacing, data.Constraint, data.ConstraintCount, data.StartCorner, data.StartAxis);
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
            var start = (GridLayoutGroupStruct)startValues;
            var end = (GridLayoutGroupStruct)endValues;

            var newPadding = new RectOffset(
                Mathf.RoundToInt(Mathf.Lerp(start.Padding.left, end.Padding.left, t)),
                Mathf.RoundToInt(Mathf.Lerp(start.Padding.right, end.Padding.right, t)),
                Mathf.RoundToInt(Mathf.Lerp(start.Padding.top, end.Padding.top, t)),
                Mathf.RoundToInt(Mathf.Lerp(start.Padding.bottom, end.Padding.bottom, t)));

            var newCellSize = Vector2.Lerp(start.CellSize, end.CellSize, t);
            var newSpacing = Vector2.Lerp(start.Spacing, end.Spacing, t);
            
            var newConstraint = t < 0.5f ? start.Constraint : end.Constraint;
            var newConstraintCount = t < 0.5f ? start.ConstraintCount : end.ConstraintCount;
            var newStartCorner = t < 0.5f ? start.StartCorner : end.StartCorner;
            var newStartAxis = t < 0.5f ? start.StartAxis : end.StartAxis;

            ApplyLayoutGroupValues(newPadding, newCellSize, newSpacing, newConstraint, newConstraintCount, newStartCorner, newStartAxis);
        }

        private GridLayoutGroupStruct GetCurrentValues()
        {
            return new GridLayoutGroupStruct(LayoutGroup);
        }

        private void ApplyLayoutGroupValues(RectOffset padding, Vector2 cellSize, Vector2 spacing, GridLayoutGroup.Constraint constraint, int constraintCount, GridLayoutGroup.Corner startCorner, GridLayoutGroup.Axis startAxis)
        {
            LayoutGroup.padding = new RectOffset(
                padding.left,
                padding.right,
                padding.top,
                padding.bottom);

            LayoutGroup.cellSize = cellSize;
            LayoutGroup.spacing = spacing;
            LayoutGroup.constraint = constraint;
            LayoutGroup.constraintCount = constraintCount;
            LayoutGroup.startCorner = startCorner;
            LayoutGroup.startAxis = startAxis;

            LayoutRebuilder.MarkLayoutForRebuild((RectTransform)LayoutGroup.transform);
        }
    }
}