using System.Threading;
using Cysharp.Threading.Tasks;
using ToBeScrap.Game.Items;
using ToBeScrap.Game.Players.Inputs;
using UnityEngine;
using Zenject;
using UniRx;
using Unity.Mathematics;

namespace ToBeScrap.Game.Players
{
    public class PlayerItemGrabber : MonoBehaviour
    {
        [Inject] private PlayerItemGetter _itemGetter;
        [Inject] private IPlayerInput _input;
        [Inject] private Player _player;
        [SerializeField] private Transform rightHandTransform;
        private ReactiveProperty<Item> _currentHasItem = new ReactiveProperty<Item>();
        public IReadOnlyReactiveProperty<Item> CurrentHasItem => _currentHasItem.ToReadOnlyReactiveProperty();

        private void Start()
        {
            var token = this.GetCancellationTokenOnDestroy();
            GrabItemLoopAsync(token).Forget();
        }

        private async UniTaskVoid GrabItemLoopAsync(CancellationToken token = default)
        {
            while (!token.IsCancellationRequested)
            {
                var item = await _itemGetter.TouchingItemObservable
                    .Where(x => _input.HasPressingItemButton.Value && x.Owner.Value == PlayerId.None)
                    .FirstOrDefault()
                    .ToUniTask(cancellationToken: token);
                
                HasItem(item);

                await UniTask.DelayFrame(1, cancellationToken: token);
                await UniTask.Yield(PlayerLoopTiming.FixedUpdate);

                await _input.HasPressingItemButton.Where(x => x).FirstOrDefault().ToUniTask(cancellationToken: token);
                
                if (!(CurrentHasItem.Value != null && CurrentHasItem.Value.Owner.Value == _player.AttackerId))
                    continue;

                var direction = _input.ThrowDirection.Value;
                if(direction == Vector2.zero)
                    direction = Vector2.right;
                
                CurrentHasItem.Value.OnThrow(direction);

                await CurrentHasItem.Value.IsGrounded.Where(x => x).FirstOrDefault()
                    .ToUniTask(cancellationToken: token);
                
                CurrentHasItem.Value.UnOwn();

                    await UniTask.Delay(100, cancellationToken: token);
                await UniTask.Yield(PlayerLoopTiming.Update);
            }
        }

        private void SwitchItem(Item item)
        {
            if (CurrentHasItem.Value != null)
                CurrentHasItem.Value.UnOwn();
            item.Own(_player.AttackerId);
            _currentHasItem.Value = item;
        }

        private void HasItem(Item item)
        {
            SwitchItem(item);
            item.Init(_player);
            item.transform.SetParent(rightHandTransform, false);
            item.transform.localPosition = Vector3.zero;
            item.transform.rotation = quaternion.identity;
        }
    }
}