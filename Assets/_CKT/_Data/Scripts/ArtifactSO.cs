using UnityEngine;

namespace CKT
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Artifact")]
    public class ArtifactSO : ScriptableObject
    {
        [SerializeField] string         _artifactName;
        [SerializeField] Define.PoolID  _projectilePoolID;
        [SerializeField] float          _manaCost;
        [SerializeField] float          _moveSpeed;
        [SerializeField] float          _existTime;
        [SerializeField] float          _attackSpeed;
        [SerializeField] float          _damage;
        [SerializeField] int            _penetration;

        public string        ArtifactName { get; }
        public Define.PoolID ProjectilePoolID { get; }
        public float         ManaCost { get; }
        public float         MoveSpeed { get; }
        public float         ExistTime { get; }
        public float         AttackSpeed { get; }
        public float         Damage { get; }
        public int           Penetration { get; }
    }
}