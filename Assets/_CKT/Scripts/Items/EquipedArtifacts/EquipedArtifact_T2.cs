using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CKT
{
    public class EquipedArtifact_T2 : EquipedArtifact
    {
        protected override GameObject _fieldArtifact => Resources.Load<GameObject>("FieldArtifacts/FieldArtifact_T2");
        protected override string _prefabName => "Bullet_T2";
        protected override float _attackSpeed => 0.5f;

        protected override void Attack(List<GameObject> list)
        {
            _attackCoroutine = _attackCoroutine ?? StartCoroutine(AttackCoroutine(list));
        }

        protected override IEnumerator AttackCoroutine(List<GameObject> list)
        {
            //TODO : 사운드_투사체 발사
            YSJ.Managers.Sound.PlaySFX(Define.SFX.DefaultAttack);

            //애니메이션 재생
            base._animator.Play("Attack", -1, 0);

            //총알 생성
            //총알 생성
            GameObject bullet = YSJ.Managers.Pool.InstPrefab(_prefabName);
            bullet.transform.position = this.transform.position + this.transform.up;
            //이동 방향
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseDir = (mousePos - this.transform.position).normalized;
            bullet.transform.up = mouseDir;
            //이름 설정 (복사본 만들 때 이름을 받아서 생성하는 용도)
            bullet.name = _prefabName;
            //왼손||오른손 SkillManager 설정
            bullet.GetComponent<Projectile>().SkillManager = base._skillManager;
            //좌우반전
            bullet.GetComponentInChildren<SpriteRenderer>().flipY = base._renderer.flipY;

            //CastSkill
            foreach (Func<GameObject, IEnumerator> castSkill in _skillManager.CastSkillDict.Values)
            {
                StartCoroutine(castSkill(bullet));
            }

            yield return new WaitForSeconds(_attackSpeed);
            base._attackCoroutine = null;
        }
    }
}