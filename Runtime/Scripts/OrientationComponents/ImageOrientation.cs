using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KasaiFudo.ScreenOrientation
{
    [RequireComponent(typeof(Image))]
    public class ImageOrientation : OrientationAwareComponent
    {
        [SerializeField] private Sprite _portraitSprite;
        [SerializeField] private Sprite _landscapeSprite;
        
        private Image _image;
        
        public Image Image
        {
            get
            {
                if(_image == null) _image = GetComponent<Image>();
                
                return _image;
            }
        }

        protected override void ChangeOrientationImmediate(BasicScreenOrientation orientation)
        {
            Image.sprite = (Sprite)GetTargetValues(orientation);
        }

        protected override object GetCurrentValues(BasicScreenOrientation oldOrientation)
        {
            return GetTargetValues(oldOrientation);
        }

        protected override object GetTargetValues(BasicScreenOrientation orientation)
        {
            return orientation == BasicScreenOrientation.Portrait ? _portraitSprite : _landscapeSprite;
        }
    }
}
