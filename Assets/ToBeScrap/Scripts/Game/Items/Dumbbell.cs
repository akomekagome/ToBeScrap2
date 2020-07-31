using System;
using ToBeScrap.Utils;
using UniRx.Triggers;
using UnityEngine;

namespace ToBeScrap.Game.Items
{
    public class Dumbbell : Item
    {
        public override void OnThrow(Vector2 direction)
        {
            Rb2D.simulated = true;
            Rb2D.velocity = direction * _itemStatusData.itemSpeed;
        }
    }
}