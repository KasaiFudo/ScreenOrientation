namespace KasaiFudo.ScreenOrientation
{
    public interface IAnimatedOrientationListener : IOrientationListener
    {
        void Initialize(float delay);
    }
}