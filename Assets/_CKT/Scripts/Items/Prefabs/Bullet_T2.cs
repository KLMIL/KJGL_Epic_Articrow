using CKT;
using UnityEngine;

public class Bullet_T2 : Projectile
{
    protected override int BasePenetration => 0;
    protected override float MoveSpeed => 5f;
    protected override float Damage => 4f;
    protected override float ExistTime => 0.15f;

    protected override void CreateHitSkillObject()
    {
        GameObject hitSkillObject = YSJ.Managers.Pool.InstPrefab("HitSkillObject");
        hitSkillObject.transform.position = this.transform.position;
        hitSkillObject.transform.up = this.transform.up;
        hitSkillObject.transform.localScale = this.transform.localScale;
        hitSkillObject.GetComponent<HitSkillObject>().HitSkill(SkillManager);
    }
}
