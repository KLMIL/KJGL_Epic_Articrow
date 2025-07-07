using System.Collections;
using UnityEngine;
using YSJ;

public class Artifact_AssultRifle : Artifact_YSJ
{
    private void Start()
    {
        ArtifactInitialize();
    }

    public override void SkillAttackClicked()
    {
        isCanLeftClick = false;
        base.SkillAttackClicked();
    }

    public override void SkillAttackCancled()
    {
        isCanLeftClick = true;
        base.SkillAttackCancled();
    }

    protected override void ResetNormalAttack()
    {
        base.ResetNormalAttack();
        normalStatus.AfterFire += PlayNormalSFX; // 일반 공격 생성 한 직후 사운드 재생
    }

    protected override void ResetSkillAttack()
    {
        base.ResetSkillAttack();
        skillStatus.AfterFire += PlaySkillSFX; // 일반 공격 생성 한 직후 사운드 재생
    }

    void PlayNormalSFX(Artifact_YSJ artifact = null, GameObject go = null)
    {
        Managers.Sound.PlaySFX(Define.SFX.AssultRifleNormalAttack);
    }

    void PlaySkillSFX(Artifact_YSJ artifact = null, GameObject go = null)
    {
        Managers.Sound.PlaySFX(Define.SFX.AssultRifleSkillAttack);
    }
}