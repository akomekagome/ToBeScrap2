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
            _itemGetter.TouchingItemObservable
                .Where(x => _input.HasPressingItemButton.Value && x.Owner.Value == PlayerId.None) 
                .ThrottleFirstFrame(0)
                .Subscribe(HasItem);

            _input.HasPressingItemButton
                .Where(x => x)
                .AsUnitObservable()
                .BatchFrame(0, FrameCountType.FixedUpdate)
                .Select(_ => _input.ThrowDirection.Value)
                .Where(_ => CurrentHasItem.Value != null)
                .Subscribe(v =>
                {
                    CurrentHasItem.Value.UnOwn();
                    CurrentHasItem.Value.OnThrow(v);
                });
        }

        private void SwitchItem(Item item)
        {
            if (CurrentHasItem.Value != null)
                CurrentHasItem.Value.UnOwn();
            item.Own(_player.PlayerId);
            _currentHasItem.Value = item;
        }

        private void HasItem(Item item)
        {
            SwitchItem(item);
            item.transform.rotation = quaternion.identity;
            item.transform.SetParent(transform, false);
            item.transform.localPosition = Vector3.zero;
        }
    }
}