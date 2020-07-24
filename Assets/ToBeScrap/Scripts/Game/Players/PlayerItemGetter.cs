using System;
using System.Collections.Generic;
using ToBeScrap.Game.Items;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace ToBeScrap.Game.Players
{
    public class PlayerItemGetter : MonoBehaviour
    {
        private Subject<Item> _touchingItemSubject = new Subject<Item>();
        public IObservable<Item> TouchingItemObservable => _touchingItemSubject.AsObservable();
        
        private void Start()
        {
            this.OnCollisionStay2DAsObservable()
                .Select(x => x.collider.GetComponent<Item>())
                .Subscribe(_touchingItemSubject);
        }
    }
}