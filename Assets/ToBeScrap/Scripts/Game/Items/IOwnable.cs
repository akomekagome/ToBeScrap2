using ToBeScrap.Game.Players;
using UniRx;

namespace ToBeScrap.Game.Items
{
    public interface IOwnable
    {
        IReadOnlyReactiveProperty<PlayerId> Owner { get; }
        void Own(PlayerId playerId);
        void UnOwn();
    }
}