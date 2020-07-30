using System;
using System.Net.NetworkInformation;
using ToBeScrap.Damages;
using UnityEngine;

namespace ToBeScrap.Game.Players
{
    public class Player : MonoBehaviour, IAttacker
    {
        [SerializeField] private PlayerId playerId;
        public PlayerId AttackerId => playerId;

        public void Init(PlayerId playerId)
        {
            this.playerId = playerId;
        }
    }
}