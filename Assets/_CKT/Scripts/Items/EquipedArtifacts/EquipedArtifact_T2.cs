using UnityEngine;

namespace CKT
{
    public class EquipedArtifact_T2 : EquipedArtifact
    {
        protected override GameObject FieldArtifact => Resources.Load<GameObject>("FieldArtifacts/FieldArtifact_T2");
        protected override Define.PoolID PoolID => Define.PoolID.Bullet_T2;
        protected override float ManaCost => 20f;
        protected override float MoveSpeed => 10f;
        protected override float ExistTime => 0.15f;
        protected override float Damage => 40f;
        protected override float AttackSpeed => 0.5f;
        protected override int Penetration => 0;
    }
}