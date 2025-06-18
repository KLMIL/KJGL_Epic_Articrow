using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace CKT
{
    public class Bullet_T4 : Projectile
    {
        protected override int BasePenetration => 0;
        protected override float MoveSpeed => 15;
        protected override float Damage => 1f;
        protected override float ExistTime => 0.2f;
        protected override Define.PoolID PoolID => Define.PoolID.Bullet_T4;

        Pellet[] _pellets;
        float _moveXSpeed = 3f;

        private new void OnEnable()
        {
            base.OnEnable();

            StartCoroutine(PelletEnableCoroutine());
        }

        private new void OnDisable()
        {
            base.OnDisable();

            _pellets = _pellets ?? GetComponentsInChildren<Pellet>();
            for (int i = 0; i < _pellets.Length; i++)
            {
                _pellets[i].transform.localPosition = Vector3.zero;
            }
        }

        IEnumerator PelletEnableCoroutine()
        {
            yield return null;

            _pellets = _pellets ?? GetComponentsInChildren<Pellet>();
            float _deltaSpeed = (_moveXSpeed * 2) / (_pellets.Length - 1);

            for (int i = 0; i < _pellets.Length; i++)
            {
                _pellets[i].gameObject.SetActive(true);
                _pellets[i].ScatterSpeed = _moveXSpeed - (_deltaSpeed * i);
                _pellets[i].SkillManager = base.SkillManager;
            }
        }
    }
}