using System;
using UnityEngine;

namespace CKT
{
    public class Bullet : Projectile
    {
        protected override void CreateHitSkillObject()
        {
            GameObject hitSkillObject = YSJ.Managers.Pool.InstPrefab("HitSkillObject");
            hitSkillObject.transform.position = this.transform.position;
            hitSkillObject.transform.up = this.transform.up;
            hitSkillObject.GetComponent<HitSkillObject>().HitSkill(SkillManager);
        }
    }
}