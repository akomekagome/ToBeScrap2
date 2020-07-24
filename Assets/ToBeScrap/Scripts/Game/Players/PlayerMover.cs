using ToBeScrap.Game.Players.Inputs;
using ToBeScrap.Utils;
using UnityEngine;
using Zenject;
using UniRx;
using UniRx.Triggers;
using Unity.Mathematics;

namespace ToBeScrap.Game.Players
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMover : MonoBehaviour
    {
        [Inject] private IPlayerInput _input;
        [Inject] private Rigidbody2D _rb2D;
        [SerializeField] private ContactFilter2D filter2d;
        private const float MaxSpeed = 4f;
        private const float MaxSpeedTime = 1f;
        private const float MaxJumpCount = 2f;
        private float _movedHorizontallyTime;
        private int _jumpCount;
        private BoolReactiveProperty _isJumping = new BoolReactiveProperty(false);
        public IReadOnlyReactiveProperty<bool> IsJumping => _isJumping.ToReadOnlyReactiveProperty();
            
        private void Start()
        {
            this.FixedUpdateAsObservable()
                .Select(_ => _rb2D.IsTouching(filter2d))
                .Subscribe(x => _isJumping.Value = !x);

            _isJumping
                .Where(x => !x)
                .Subscribe(_ => _jumpCount = 0)
                .AddTo(this);

            this.FixedUpdateAsObservable()
                .Select(_ => _input.MoveDirection.Value.x)
                .SkipWhile(x => Mathf.Approximately(x, 0f))
                .TakeWhile(x => !Mathf.Approximately(x, 0f))
                .DoOnCompleted(() => _movedHorizontallyTime = 0f)
                .RepeatUntilDestroy(this)
                .Subscribe(MoveHorizontally);
            
            _input.HasPressingJumpButton
                .Where(x => x && _jumpCount < MaxJumpCount)
                .AsUnitObservable()
                .BatchFrame(0, FrameCountType.FixedUpdate)
                .Subscribe(_ => Jump())
                .AddTo(this);
        }

        private void Jump()
        {
            _jumpCount++;
            
            _rb2D.velocity = _rb2D.velocity.SetY(7f);
        }

        private void MoveHorizontally(float sign)
        {
            _movedHorizontallyTime += Time.deltaTime;
            
            _rb2D.velocity = _rb2D.velocity.SetX(sign * (_movedHorizontallyTime > MaxSpeedTime ?
                MaxSpeed : CircleEasing(_movedHorizontallyTime / MaxSpeedTime) * MaxSpeed));
        }

        private float CircleEasing(float x)
        {
            return Mathf.Sqrt(1 - Mathf.Pow(1 - x, 2f));
        }
    }
}