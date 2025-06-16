using System.Collections;
using UnityEngine;

namespace CKT
{
    public class Bullet_T2 : Projectile
    {
        protected override int BasePenetration => 0;
        protected override float MoveSpeed => 10f;
        protected override float Damage => 40f;
        protected override float ExistTime => 0.15f;
    }
}