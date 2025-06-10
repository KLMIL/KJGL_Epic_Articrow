using UnityEngine;

namespace CKT
{
    public class EquipedArtifact_T1 : EquipedArtifact
    {
        private void Awake()
        {
            base.Init("FieldArtifacts/FieldArtifact_T1", "Bullet_T1");
        }

        protected override GameObject CreateBullet(string prefabName, SkillManager skillManager)
        {
            //총알 생성
            GameObject bullet = YSJ.Managers.Pool.InstPrefab(prefabName);
            bullet.transform.position = this.transform.position + this.transform.up;
            //이동 방향
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseDir = (mousePos - this.transform.position).normalized;
            bullet.transform.up = mouseDir;
            //이름 설정 (복사본 만들 때 이름을 받아서 생성하는 용도)
            bullet.name = prefabName;
            //왼손||오른손 SkillManager 설정
            bullet.GetComponent<Projectile>().SkillManager = skillManager;

            return bullet;
        }
    }
}