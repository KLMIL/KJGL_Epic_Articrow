using UnityEngine;

namespace CKT
{
    public class HitExplosion : Explosion
    {
        protected override float BaseDamage => 12f;

        protected override float DisableTime => 1f;

        protected override Define.PoolID PoolID => Define.PoolID.HitExplosion;
    }
}