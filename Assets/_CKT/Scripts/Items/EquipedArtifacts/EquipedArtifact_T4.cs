using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CKT
{
    public class EquipedArtifact_T4 : EquipedArtifact
    {
        protected override GameObject FieldArtifact => Resources.Load<GameObject>("FieldArtifacts/FieldArtifact_T4");
        protected override Define.PoolID PoolID => Define.PoolID.Bullet_T4;
        protected override float AttackSpeed => 0.5f;
        protected override float ManaCost => 20f;

        protected override IEnumerator AttackCoroutine(List<GameObject> list)
        {
            YSJ.Managers.Sound.PlaySFX(Define.SFX.DefaultAttack);

            //애니메이션 재생
            _animator.Play("Attack", -1, 0);

            //총알 생성
            GameObject bullet = YSJ.Managers.TestPool.Get<GameObject>(PoolID);

            bullet.transform.position = base._firePoint.position;
            bullet.transform.up = this.transform.up;
            bullet.GetComponent<Projectile>().SkillManager = base._skillManager;

            //CastSkill
            foreach (Func<Vector3, Vector3, IEnumerator> castSkill in _skillManager.CastSkillDict.Values)
            {
                StartCoroutine(castSkill(bullet.transform.position, bullet.transform.up));
            }

            yield return new WaitForSeconds(AttackSpeed);
            _attackCoroutine = null;
        }

        protected override void AttackCancel()
        {

        }
    }
}