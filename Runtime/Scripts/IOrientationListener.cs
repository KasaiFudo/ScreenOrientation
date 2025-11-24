namespace KasaiFudo.ScreenOrientation
{
    public interface IOrientationListener
    {
        void OnOrientationChanged(BasicScreenOrientation newOrientation);
        void ApplyPortraitData();
        void ApplyLandscapeData();
    }
}