using UnityEngine;
using YSJ;

public class Artifact_Flute : Artifact_YSJ
{
    void Start()
    {
        ArtifactInitialize();
    }

    void SpawnedAttackSetChild(Artifact_YSJ me, GameObject spawnedAttack)
    {
        spawnedAttack.transform.SetParent(transform);
    }

    protected override void ResetNormalAttack()
    {
        base.ResetNormalAttack();
        normalStatus.AfterFire += PlayNormalSFX; // 일반 공격 생성 한 직후 사운드 재생
    }

    protected override void ResetSkillAttack()
    {
        base.ResetSkillAttack();
        skillStatus.AfterFire += SpawnedAttackSetChild;
    }

    void PlayNormalSFX(Artifact_YSJ artifact = null, GameObject go = null)
    {
        Managers.Sound.PlaySFX(Define.SFX.FluteNormalAttack);
    }
}