using UnityEngine;

namespace CKT
{
    public class Bullet_T2 : Projectile
    {
        protected override int BasePenetration => 0;
        protected override float MoveSpeed => 5f;
        protected override float Damage => 4f;
        protected override float ExistTime => 0.15f;

        protected override void CreateHitSkillObject()
        {
            GameObject hitSkillObject = YSJ.Managers.Pool.InstPrefab("HitSkillObject");
            hitSkillObject.transform.position = transform.position;
            hitSkillObject.transform.up = transform.up;
            hitSkillObject.transform.localScale = transform.localScale;
            hitSkillObject.GetComponent<HitSkillObject>().HitSkill(SkillManager);
        }
    }
}