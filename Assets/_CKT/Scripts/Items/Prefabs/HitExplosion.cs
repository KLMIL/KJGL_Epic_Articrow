using UnityEngine;

namespace CKT
{
    public class HitExplosion : Explosion
    {
        protected override Define.PoolID PoolID => Define.PoolID.HitExplosion;
    }
}