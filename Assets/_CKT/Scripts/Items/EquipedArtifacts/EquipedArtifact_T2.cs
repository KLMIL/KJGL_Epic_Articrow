using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CKT
{
    public class EquipedArtifact_T2 : EquipedArtifact
    {
        protected override GameObject FieldArtifact => Resources.Load<GameObject>("FieldArtifacts/FieldArtifact_T2");
        protected override Define.PoolID PoolID => Define.PoolID.Bullet_T2;
        protected override float AttackSpeed => 0.5f;
        protected override float ManaCost => 20f;

        #region [Attack]
        protected override IEnumerator AttackCoroutine(List<GameObject> list)
        {
            YSJ.Managers.Sound.PlaySFX(Define.SFX.DefaultAttack);

            //애니메이션 재생
            base._animator.Play("Attack", -1, 0);

            //총알 생성
            GameObject bullet = YSJ.Managers.TestPool.Get<GameObject>(Define.PoolID.Bullet_T2);
            bullet.transform.position = _firePoint.position;
            //이동 방향
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseDir = (mousePos - this.transform.position).normalized;
            bullet.transform.up = mouseDir;
            //이름 설정 (복사본 만들 때 이름을 받아서 생성하는 용도)
            bullet.name = PoolID.ToString();
            //왼손||오른손 SkillManager 설정
            bullet.GetComponent<Projectile>().SkillManager = base._skillManager;

            //CastSkill
            foreach (Func<Vector3, Vector3, IEnumerator> castSkill in _skillManager.CastSkillDict.Values)
            {
                StartCoroutine(castSkill(bullet.transform.position, bullet.transform.up));
            }

            yield return new WaitForSeconds(AttackSpeed);
            base._attackCoroutine = null;
        }
        #endregion

        #region [Attack Cancel]
        protected override void AttackCancel()
        {
            Debug.Log("Attack Cancel");
        }
        #endregion
    }
}