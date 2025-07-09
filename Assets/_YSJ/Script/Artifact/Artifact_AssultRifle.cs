using System.Collections;
using UnityEngine;
using YSJ;

public class Artifact_AssultRifle : Artifact_YSJ
{
    public GameObject FireEffect;

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
        normalStatus.AfterFire += CreateFireEffect;
    }

    protected override void ResetSkillAttack()
    {
        base.ResetSkillAttack();
        skillStatus.AfterFire += PlaySkillSFX; // 일반 공격 생성 한 직후 사운드 재생
        skillStatus.AfterFire += CreateFireEffect;
    }

    void PlayNormalSFX(Artifact_YSJ artifact = null, GameObject go = null)
    {
        Managers.Sound.PlaySFX(Define.SFX.AssultRifleNormalAttack);
    }

    void PlaySkillSFX(Artifact_YSJ artifact = null, GameObject go = null)
    {
        Managers.Sound.PlaySFX(Define.SFX.AssultRifleSkillAttack);
    }

    // 발사 이펙트 소환
    void CreateFireEffect(Artifact_YSJ me, GameObject bullet)
    {
        GameObject spawnedEffect = Instantiate(FireEffect, transform);
        spawnedEffect.transform.position += transform.right * 0.3f;
        spawnedEffect.transform.position += -transform.up * 0.1f;
    }
}