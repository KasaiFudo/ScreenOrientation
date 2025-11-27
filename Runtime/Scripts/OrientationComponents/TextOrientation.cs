using TMPro;
using UnityEngine;

namespace KasaiFudo.ScreenOrientation
{
    public class TextOrientation : AnimatedOrientationAwareble<float>
    {
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
            return Text.fontSize;
        }

        protected override void ApplyInterpolated(float start, float end, float t)
        {
            Text.fontSize = Mathf.Lerp(start, end, t);
        }
    }
}
