using System.Collections;
using UnityEngine;

namespace CKT
{
    public class HitDamageArea : DamageArea
    {
        protected override float BaseDamage => 2f;
        protected override float DamageGap => 0.5f;
        protected override int DamageCount => 4;
        protected override Define.PoolID PoolID => Define.PoolID.HitDamageArea;
    }
}