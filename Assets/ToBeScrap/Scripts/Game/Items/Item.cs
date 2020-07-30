using System;
using System.Collections.Generic;
using System.Linq;
using ToBeScrap.Damages;
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
        protected readonly float ItemSpeed = 100f;
        protected IAttacker _attacker;
        private BoolReactiveProperty _isGrounded = new BoolReactiveProperty(false);
        public IReadOnlyReactiveProperty<bool> IsGrounded => _isGrounded.ToReadOnlyReactiveProperty();

        public void Init(IAttacker attacker)
        {
            _attacker = attacker;
        }
        
        private void Start()
        { 
            var groundMask = 1 << (int)LayerName.Ground;
            var playerMask = 1 << (int)LayerName.Player;

            Owner
                .Do(x => Debug.Log(x))
                .Where(x => x != PlayerId.None)
                .Subscribe(_ => Rb2D.simulated = false);

            this.UpdateAsObservable()
                .Select(_ => Physics2DExtenstion.OverlapBoxCollider(BoxCollider2D, transform.position, groundMask))
                .Subscribe(x => _isGrounded.Value = x != null);
            
            this.UpdateAsObservable()
                .Select(_ => Physics2DExtenstion.OverlapBoxColliderALL(BoxCollider2D, transform.position, playerMask))
                .Select(x => x.Select(c => c.GetComponent<Player>()).Where(p => p != null && p.AttackerId != _owner.Value && _owner.Value != PlayerId.None).ToList())
                .Where(x => x.Any())
                .Subscribe(x => OnDamage(x));
        }

        private void OnDamage(List<Player> players)
        {
            foreach (var damageable in players.Select(x => x.GetComponent<IDamageable>()).Where(x => x != null))
            {
                var damage = new Damage(new Vector2(1, 1) * 50f, _attacker);
                damageable.ApplyDamage(damage);
            }
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