using UnityEngine;

namespace KasaiFudo.ScreenOrientation
{
    [CreateAssetMenu(fileName = "AdditionKeyConfig", menuName = "Orientation/Addition Key Config")]
    public class AdditionKeyConfig : ScriptableObject
    {
        [field: SerializeField]public string Key { get; private set; }

        public void SetKey(string newKey)
        {
            if (!string.IsNullOrWhiteSpace(newKey))
                Key = newKey;
        }
    }
}