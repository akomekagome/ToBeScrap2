using System;
using System.Net.NetworkInformation;
using UnityEngine;

namespace ToBeScrap.Game.Players
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerId playerId;
        public PlayerId PlayerId => playerId;

        public void Init(PlayerId playerId)
        {
            this.playerId = playerId;
        }
    }
}