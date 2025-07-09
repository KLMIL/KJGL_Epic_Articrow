using UnityEngine;
using YSJ;

public class Artifact_Flute : Artifact_YSJ
{
    [Header("발사 이펙트")]
    public GameObject FireEffect;
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
        normalStatus.AfterFire += CreateFireEffect;
    }

    protected override void ResetSkillAttack()
    {
        base.ResetSkillAttack();
        skillStatus.AfterFire += SpawnedAttackSetChild;
        skillStatus.AfterFire += CreateFireEffect;
    }

    void PlayNormalSFX(Artifact_YSJ artifact = null, GameObject go = null)
    {
        Managers.Sound.PlaySFX(Define.SFX.FluteNormalAttack);
    }

    // 발사 이펙트 소환
    void CreateFireEffect(Artifact_YSJ me, GameObject bullet)
    {
        GameObject spawnedEffect = Instantiate(FireEffect, transform);
        spawnedEffect.transform.position += transform.right * 0.4f;
        //spawnedEffect.transform.position += -transform.up * 0.1f;
    }
}