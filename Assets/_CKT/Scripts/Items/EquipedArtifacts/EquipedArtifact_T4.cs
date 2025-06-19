using UnityEngine;

namespace CKT
{
    public class EquipedArtifact_T4 : EquipedArtifact
    {
        protected override GameObject FieldArtifact => Resources.Load<GameObject>("FieldArtifacts/FieldArtifact_T4");
        protected override Define.PoolID PoolID => Define.PoolID.Bullet_T4;
        protected override float AttackSpeed => 0.5f;
        protected override float ManaCost => 20f;
        protected override float MoveSpeed => 15f;
        protected override float ExistTime => 0.2f;
        protected override float Damage => 1;
        protected override int Penetration => 0;
    }
}