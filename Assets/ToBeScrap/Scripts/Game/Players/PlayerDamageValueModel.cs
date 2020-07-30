using System;
using UnityEngine;
using Zenject;
using UniRx;

namespace ToBeScrap.Game.Players
{
    public class PlayerDamageValueModel : MonoBehaviour
    {
        [Inject] private PlayerDamageable _playerDamageable;
        [Inject] private Player _player;
        private void Start()
        {
            // _playerDamageable.ApplyDamageObservable
            //     .Where(x => x.Attacker.AttackerId != _player.AttackerId)
            //     .Subscribe(x => )
        }
    }
}