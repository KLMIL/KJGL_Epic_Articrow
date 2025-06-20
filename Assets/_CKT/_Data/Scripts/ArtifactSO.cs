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
        [SerializeField] float          _attackDelay;
        [SerializeField] float          _attackCoolTime;
        [SerializeField] float          _damage;
        [SerializeField] int            _penetration;

        public string        ArtifactName { get => _artifactName; }
        public Define.PoolID ProjectilePoolID { get => _projectilePoolID; }
        public float         ManaCost { get => _manaCost; }
        public float         MoveSpeed { get => _moveSpeed; }
        public float         ExistTime { get => _existTime; }
        public float         AttackDelay { get => _attackDelay; }
        public float         AttackCoolTime { get => _attackCoolTime; }
        public float         Damage { get => _damage; }
        public int           Penetration { get => _penetration; }
    }
}