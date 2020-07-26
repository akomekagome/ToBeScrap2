using System;
using System.Collections.Generic;
using System.Linq;
using ToBeScrap.Game.Items;
using ToBeScrap.Scripts.Common;
using ToBeScrap.Utils;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace ToBeScrap.Game.Players
{
    public class PlayerItemGetter : MonoBehaviour
    {
        [Inject] private CapsuleCollider2D _capsuleCollider2D;
        private Subject<Item> _touchingItemSubject = new Subject<Item>();
        public IObservable<Item> TouchingItemObservable => _touchingItemSubject.AsObservable();

        private void Start()
        {
            var itemMask = 1 << (int) LayerName.Item;
            // this.OnCollisionStay2DAsObservable()
            //     .Select(x => x.collider.GetComponent<Item>())
            //     .Subscribe(_touchingItemSubject);

            this.UpdateAsObservable()
                .Select(_ => Physics2DExtenstion.OverlapCapsuleColliderAll(_capsuleCollider2D, transform.position, itemMask))
                .Select(x => x.Select(c => c.GetComponent<Item>()).Where(i => i != null).ToList())
                .Where(x => x.Any())
                .Subscribe(x => x.ForEach(_touchingItemSubject.OnNext));
        }
    }
}