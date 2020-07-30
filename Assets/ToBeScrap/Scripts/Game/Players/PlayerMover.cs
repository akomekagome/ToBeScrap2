using System.Threading;
using Cysharp.Threading.Tasks;
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
        [Inject] private PlayerDamageable _playerDamageable;
        [SerializeField] private ContactFilter2D filter2d;
        private const float MaxSpeed = 4f;
        private const float MaxSpeedTime = 1f;
        private const float MaxJumpCount = 2f;
        private float _movedHorizontallyTime;
        private int _jumpCount;
        private BoolReactiveProperty _isJumping = new BoolReactiveProperty(false);
        public IReadOnlyReactiveProperty<bool> IsJumping => _isJumping.ToReadOnlyReactiveProperty();
        private BoolReactiveProperty _isBlowingAway = new BoolReactiveProperty(false);
        public IReadOnlyReactiveProperty<bool> IsBlowingAway => _isBlowingAway.ToReadOnlyReactiveProperty();

        private void Start()
        {
            var token = this.GetCancellationTokenOnDestroy();
            this.FixedUpdateAsObservable()
                .Select(_ => _rb2D.IsTouching(filter2d))
                .Subscribe(x => _isJumping.Value = !x);
            
            this.UpdateAsObservable()
                .Where(_ => !IsJumping.Value && !_isBlowingAway.Value)
                .Subscribe(_ => _jumpCount = 0)
                .AddTo(this);

            this.FixedUpdateAsObservable()
                .Where(_ => !_isBlowingAway.Value)
                .Select(_ => _input.MoveDirection.Value.x)
                .Pairwise()
                .SkipWhile(x => Mathf.Approximately(x.Current, 0f))
                .TakeWhile(x => !Mathf.Approximately(x.Current, 0f) && x.Current >= 0 == x.Previous >= 0)
                .DoOnCompleted(() => _movedHorizontallyTime = 0f)
                .RepeatUntilDestroy(this)
                .Subscribe(x => MoveHorizontally(x.Current));
            
            _input.HasPressingJumpButton
                .Where(x => x && _jumpCount < MaxJumpCount)
                .AsUnitObservable()
                .BatchFrame(0, FrameCountType.FixedUpdate)
                .Subscribe(_ => Jump())
                .AddTo(this);

            _playerDamageable.ApplyDamageObservable
                .Subscribe(x => BlowAwayAsync(x.BlowingDirection, token).Forget());
        }

        private async UniTaskVoid BlowAwayAsync(Vector2 direction, CancellationToken token = default)
        {
            _isBlowingAway.Value = true;
            
            await UniTask.Yield(PlayerLoopTiming.FixedUpdate);

            _rb2D.velocity = direction;

            while (_rb2D.velocity.magnitude > Time.deltaTime * 9.8f * _rb2D.gravityScale)
                await UniTask.DelayFrame(1, cancellationToken: token);

            _isBlowingAway.Value = false;
        }
        
        private void Jump()
        {
            _jumpCount++;
            
            _rb2D.velocity = _rb2D.velocity.SetY(7f);
        }

        private void MoveHorizontally(float sign)
        {
            _movedHorizontallyTime = Mathf.Clamp(_movedHorizontallyTime + Time.deltaTime, 0, MaxSpeedTime);
            
            _rb2D.velocity = _rb2D.velocity.SetX(sign * CircleEasing(_movedHorizontallyTime / MaxSpeedTime) * MaxSpeed);
        }

        private float CircleEasing(float x)
        {
            return (x >= 0 ? 1 : -1) * Mathf.Sqrt(1 - Mathf.Pow(1 - Mathf.Abs(x), 2f));
        }
    }
}