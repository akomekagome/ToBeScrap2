using UnityEngine;
using UniRx;

namespace ToBeScrap.Game.Players.Inputs
{
    public interface IPlayerInput
    {
        IReadOnlyReactiveProperty<Vector2> MoveDirection { get; }
        IReadOnlyReactiveProperty<Vector2> ThrowDirection { get; }
        IReadOnlyReactiveProperty<bool> HasPressingJumpButton { get; }
        IReadOnlyReactiveProperty<bool> HasPressingItemButton { get; }
    }
}