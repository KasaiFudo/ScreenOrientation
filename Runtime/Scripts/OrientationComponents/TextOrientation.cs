using TMPro;
using UnityEngine;

namespace KasaiFudo.ScreenOrientation
{
    public class TextOrientation : AnimatedOrientationAwareble<float>
    {
        [SerializeField] private float _portraitTextSize;
        [SerializeField] private float _landscapeTextSize;
        
        private TMP_Text _text;
        
        public TMP_Text Text
        {
            get
            {
                if(_text == null) _text = GetComponent<TMP_Text>();
                
                return _text;
            }
        }

        protected override void ApplyImmediate(float data)
        {
            Text.fontSize = data;
        }

        protected override float GetCurrentValues()
        {
            return _text.fontSize;
        }

        protected override void ApplyInterpolated(float start, float end, float t)
        {
            Text.fontSize = Mathf.Lerp(start, end, t);
        }
        
        private void OnValidate()
        {
            _portrait = _portraitTextSize;
            _landscape = _landscapeTextSize;
        }
    }
}
