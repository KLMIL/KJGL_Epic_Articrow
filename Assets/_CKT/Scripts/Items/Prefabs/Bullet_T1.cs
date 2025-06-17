using UnityEngine;


namespace CKT
{
    public class Bullet_T1 : Projectile
    {
        protected override int BasePenetration => 0;
        protected override float MoveSpeed => 15f;
        protected override float Damage => 30f;
        protected override float ExistTime => 0.5f;
        protected override Define.PoolID PoolID => Define.PoolID.Bullet_T1;
    }
}