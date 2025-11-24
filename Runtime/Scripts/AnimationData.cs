using System;
using UnityEngine;

namespace KasaiFudo.ScreenOrientation
{
    [Serializable]
    public struct AnimationData
    {
        [SerializeField] public bool IsAnimated;

        [Tooltip("Override default delay from global config")]
        [SerializeField] public bool OverrideDelayToAnimate;

        [Tooltip("Custom delay when override is ON")]
        [Min(0)] [SerializeField] public float DelayToAnimate;

        [Min(0)] [SerializeField] public float TransitionDuration;
        [SerializeField] public AnimationCurve TransitionCurve;
            
        public AnimationData(bool isAnimated)
        {
            IsAnimated = isAnimated;
            OverrideDelayToAnimate = false;
            DelayToAnimate = 0f;
            TransitionDuration = 0.5f;
            TransitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        }
    }
}