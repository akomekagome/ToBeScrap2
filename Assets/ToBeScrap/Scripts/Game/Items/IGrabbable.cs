using UniRx;

namespace ToBeScrap.Game.Items
{
    public interface IGrabbable
    {
        IReadOnlyReactiveProperty<bool> IsGrabbed { get; }
        void Grab();
        void UnGrab();
    }
}