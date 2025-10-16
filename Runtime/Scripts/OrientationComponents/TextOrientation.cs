using TMPro;
using UnityEngine;

namespace KasaiFudo.ScreenOrientation
{
    public class TextOrientation : AnimateOrientationAwareComponent
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
        

        [ContextMenu("Rewrite Portrait Data")]
        public void RewritePortraitSize()
        {
            _portraitTextSize = GetCurrentValues();
        }

        [ContextMenu("Rewrite Landscape Data")]
        public void RewriteLandscapeSize()
        {
            _landscapeTextSize = GetCurrentValues();
        }

        protected override void ChangeOrientationImmediate(BasicScreenOrientation orientation)
        {
            Text.fontSize = (float)GetTargetValues(orientation);
        }

        protected override object GetCurrentValues(BasicScreenOrientation oldOrientation)
        {
            return GetTargetValues(oldOrientation);
        }

        protected override object GetTargetValues(BasicScreenOrientation orientation)
        {
            return orientation == BasicScreenOrientation.Portrait ? _portraitTextSize : _landscapeTextSize;
        }

        protected override void ApplyInterpolatedValues(object startValues, object endValues, float t)
        {
            Text.fontSize = Mathf.Lerp((float)startValues, (float)endValues, t);
        }

        private float GetCurrentValues()
        {
            return _text.fontSize;
        }
    }
}
