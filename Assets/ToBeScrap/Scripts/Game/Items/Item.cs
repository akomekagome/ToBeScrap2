using System;
using ToBeScrap.Game.Players;
using ToBeScrap.Scripts.Common;
using ToBeScrap.Utils;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace ToBeScrap.Game.Items
{
    public abstract class Item : MonoBehaviour, IGrabbable, IOwnable
    {
        [Inject] protected Rigidbody2D Rb2D;
        [Inject] protected BoxCollider2D BoxCollider2D;
        [SerializeField] protected ItemType itemType;
        protected readonly float ItemSpeed = 8f;
        private BoolReactiveProperty _isGrounded = new BoolReactiveProperty(false);
        public IReadOnlyReactiveProperty<bool> IsGrounded => _isGrounded.ToReadOnlyReactiveProperty();
        
        private void Awake()
        { 
            var groundMask = 1 << (int)LayerName.Ground;
            
            Owner
                .Do(x => Debug.Log(x))
                .Where(x => x != PlayerId.None)
                .Subscribe(_ => Rb2D.simulated = false);

            this.UpdateAsObservable()
                .Select(_ => Physics2DExtenstion.OverlapBoxCollider(BoxCollider2D, transform.position, groundMask))
                .Subscribe(x => _isGrounded.Value = x != null);
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