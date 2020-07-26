using System;
using ToBeScrap.Common.Defines;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace ToBeScrap.Game.Players.Inputs
{
    public class DebugPlayerInput : MonoBehaviour, IPlayerInput
    {
        private ReactiveProperty<Vector2> _moveDirection = new ReactiveProperty<Vector2>();
        private ReactiveProperty<Vector2> _throwDirection = new ReactiveProperty<Vector2>();
        private BoolReactiveProperty _hasPressingJumpButton = new BoolReactiveProperty();
        private BoolReactiveProperty _hasPressingItemButton = new BoolReactiveProperty();

        public IReadOnlyReactiveProperty<Vector2> MoveDirection => _moveDirection.ToReadOnlyReactiveProperty();
        public IReadOnlyReactiveProperty<Vector2> ThrowDirection => _throwDirection.ToReadOnlyReactiveProperty();
        public IReadOnlyReactiveProperty<bool> HasPressingJumpButton => _hasPressingJumpButton.ToReadOnlyReactiveProperty();
        public IReadOnlyReactiveProperty<bool> HasPressingItemButton => _hasPressingItemButton.ToReadOnlyReactiveProperty();

        private void Start()
        {
            this.UpdateAsObservable()
                .Select(_ => new Vector2(Input.GetAxis(AxisName.Horizontal), Input.GetAxis(AxisName.Vertical)))
                .Subscribe(v => _moveDirection.Value = v);
            
            this.UpdateAsObservable()
                .Select(_ => new Vector2(Input.GetAxis(AxisName.Horizontal2), Input.GetAxis(AxisName.Vertical2)))
                .Subscribe(v => _throwDirection.Value = v);

            this.UpdateAsObservable()
                .Select(_ => Input.GetKeyDown(KeyCode.Space))
                .Subscribe(x => _hasPressingJumpButton.Value = x);

            this.UpdateAsObservable()
                .Select(_ => Input.GetKeyDown(KeyCode.E))
                .Subscribe(x => _hasPressingItemButton.Value = x);
        }
    }
}