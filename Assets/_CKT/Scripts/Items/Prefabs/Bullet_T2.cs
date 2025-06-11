using UnityEngine;

namespace CKT
{
    public class Bullet_T2 : Projectile
    {
        protected override int BasePenetration => 0;
        protected override float MoveSpeed => 5f;
        protected override float Damage => 80f;
        protected override float ExistTime => 0.15f;
    }
}