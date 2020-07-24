using System;
using ToBeScrap.Game.Players;
using UniRx;
using UnityEngine;
using Zenject;

namespace ToBeScrap.Game.Items
{
    public abstract class Item : MonoBehaviour, IGrabbable, IOwnable
    {
        [Inject] protected Rigidbody2D Rb2D;
        [SerializeField] protected ItemType itemType;
        protected readonly float ItemSpeed = 8f;
        
        private void Awake()
        {
            Owner
                .Where(x => x != PlayerId.None)
                .Subscribe(_ => Rb2D.simulated = false);
        }

        public abstract void OnThrow(Vector2 direction);

        private BoolReactiveProperty _isGrabbed = new BoolReactiveProperty(false);
        public IReadOnlyReactiveProperty<bool> IsGrabbed => _isGrabbed.ToReadOnlyReactiveProperty();
        public void Grab() => _isGrabbed.Value = true;
        public void UnGrab() => _isGrabbed.Value = false;

        private ReactiveProperty<PlayerId> _owner = new ReactiveProperty<PlayerId>(PlayerId.None);
        public IReadOnlyReactiveProperty<PlayerId> Owner => _owner.ToReadOnlyReactiveProperty();
        public void Own(PlayerId playerId) => _owner.Value = playerId;
        public void UnOwn() => _owner.Value = PlayerId.None;
    }
}