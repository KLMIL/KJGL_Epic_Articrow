using UnityEngine;

namespace CKT
{
    public class EquipedArtifact_T1 : EquipedArtifact
    {
        protected override GameObject FieldArtifact => Resources.Load<GameObject>("FieldArtifacts/FieldArtifact_T1");
        protected override Define.PoolID PoolID => Define.PoolID.Bullet_T1;
        protected override float ManaCost => 20f;
        protected override float MoveSpeed => 15f;
        protected override float ExistTime => 0.5f;
        protected override float Damage => 30f;
        protected override float AttackSpeed => 0.5f;
        protected override int Penetration => 0;
    }
}