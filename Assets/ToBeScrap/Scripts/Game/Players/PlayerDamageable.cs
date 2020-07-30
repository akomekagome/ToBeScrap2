using System;
using ToBeScrap.Damages;
using UniRx;
using UnityEngine;

namespace ToBeScrap.Game.Players
{
    public class PlayerDamageable : MonoBehaviour, IDamageable
    {
        private Subject<Damage> _applyDamageSubject = new Subject<Damage>();
        public IObservable<Damage> ApplyDamageObservable => _applyDamageSubject.AsObservable();
        
        public void ApplyDamage(Damage damage)
        {
            _applyDamageSubject.OnNext(damage);
        }
    }
}