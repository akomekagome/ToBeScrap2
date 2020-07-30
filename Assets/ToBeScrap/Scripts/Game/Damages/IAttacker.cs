using ToBeScrap.Game.Players;

namespace ToBeScrap.Damages
{
    public interface IAttacker
    {
        PlayerId AttackerId { get; }
    }
}