using UnityEngine;

namespace CKT
{
    public class HitScatter : Projectile
    {
        protected override int BasePenetration => 1;
        protected override float MoveSpeed => 25f;
        protected override float Damage => 5f;
    }
}