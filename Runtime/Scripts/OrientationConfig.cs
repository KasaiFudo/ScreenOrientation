using UnityEngine;

namespace KasaiFudo.ScreenOrientation
{
    [CreateAssetMenu(fileName = "OrientationConfig", menuName = "Orientation/OrientationConfig")]
    public class OrientationConfig : ScriptableObject
    {
        [Header("Detection Settings")]
        [field: SerializeField] public bool IgnoreRapidChanges { get; private set; } = true;
        [field: SerializeField] public float RapidChangeThreshold { get; private set; } = 0.5f;
        [field: SerializeField] public bool DetectNewAwareComponentsInRuntime { get; private set; }
        [field: SerializeField] public float ListenerCheckInterval { get; private set; } = 2f;
        
        [Header("Animation Settings")]
        [field: SerializeField] public float AnimationDelayForAllComponents { get; private set; } = 0.5f;

    }
}
