using System.Collections.Generic;
using ToBeScrap.Game.Items;
using UnityEngine;

namespace ToBeScrap.Utils
{
    public static class Physics2DExtenstion
    { 
        public static int OverlapCapsuleColliderNonAlloc(CapsuleCollider2D capsuleCollider2D, Vector2 origin, Collider2D[] results)
        {
            return Physics2D.OverlapCapsuleNonAlloc(
                origin + capsuleCollider2D.offset,
                capsuleCollider2D.size,
                capsuleCollider2D.direction,
                0f,
                results);
        }

        public static Collider2D OverlapCapsuleCollider(CapsuleCollider2D capsuleCollider2D, Vector2 origin, int layerMask)
        {
            return Physics2D.OverlapCapsule(
                origin + capsuleCollider2D.offset,
                capsuleCollider2D.size,
                capsuleCollider2D.direction,
                0f,
                layerMask);
        }

        public static Collider2D[] OverlapCapsuleColliderAll(CapsuleCollider2D capsuleCollider2D, Vector2 origin, int layerMask)
        {
            return Physics2D.OverlapCapsuleAll(
                origin + capsuleCollider2D.offset,
                capsuleCollider2D.size,
                capsuleCollider2D.direction,
                0f,
                layerMask);
        }

        public static Collider2D OverlapBoxCollider(BoxCollider2D boxCollider2D, Vector2 origin, int layerMask)
        {
            return Physics2D.OverlapBox(
                origin + boxCollider2D.offset,
                boxCollider2D.size,
                0f,
                layerMask
            );
        }
        
        public static Collider2D[] OverlapBoxColliderALL(BoxCollider2D boxCollider2D, Vector2 origin, int layerMask)
        {
            return Physics2D.OverlapBoxAll(
                origin + boxCollider2D.offset,
                boxCollider2D.size,
                0f,
                layerMask
            );
        }
    }
}