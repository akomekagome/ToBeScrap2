using UnityEngine;

namespace ToBeScrap.Damages
{
    public struct Damage
    {
        public Vector2 BlowingDirection { get; }
        public IAttacker Attacker { get; }
        
        public Damage(Vector2 blowingDirection, IAttacker attacker)
        {
            BlowingDirection = blowingDirection;
            Attacker = attacker;
        }
    }
}