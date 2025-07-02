using System.Collections;
using UnityEngine;

namespace CKT
{
    public class CastDamageArea : DamageArea
    {
        protected override float BaseDamage => 3f;
        protected override float DamageGap => 0.5f;
        protected override int DamageCount => 4;
        protected override Define.PoolID PoolID => Define.PoolID.None;
    }
}