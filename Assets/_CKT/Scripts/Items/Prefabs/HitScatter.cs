using UnityEngine;

namespace CKT
{
    public class HitScatter : Projectile
    {
        protected override int BasePenetration => 1;
        protected override float MoveSpeed => 15f;
        protected override float Damage => 2f;
        protected override float ExistTime => 0.3f;
    }
}