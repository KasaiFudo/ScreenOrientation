using UnityEngine;
using UnityEngine.UI;

namespace KasaiFudo.ScreenOrientation
{
    [RequireComponent(typeof(Image))]
    public class ImageOrientation : OrientationAwareble<Sprite>
    {
        private Image _image;
        
        public Image Image
        {
            get
            {
                if(_image == null) _image = GetComponent<Image>();
                
                return _image;
            }
        }

        public void SetSprites(Sprite portraitSprite, Sprite landscapeSprite)
        { 
            _portrait = portraitSprite;
            _landscape = landscapeSprite;
            ApplyImmediate(ScreenOrientationObserver.CurrentOrientation);
        }

        protected override void ApplyImmediate(Sprite data)
        {
            Image.sprite = data;
        }

        protected override Sprite GetCurrentValues()
        {
            return Image.sprite;
        }
    }
}
