using UnityEngine;
using UnityEngine.UI;

namespace KasaiFudo.ScreenOrientation
{
    [RequireComponent(typeof(Image))]
    public class ImageOrientation : OrientationAwareble<Sprite>
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

        public void SetSprites(Sprite portraitSprite, Sprite landscapeSprite)
        {
            _portraitSprite = portraitSprite;
            _landscapeSprite = landscapeSprite;
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
