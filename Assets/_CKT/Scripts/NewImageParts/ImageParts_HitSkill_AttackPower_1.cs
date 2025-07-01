using System.Collections;
using UnityEngine;

public class ImageParts_HitSkill_AttackPower_1 : ImagePartsRoot_YSJ, IImagePartsToSkillAttack_YSJ
{
    public override string partsName => "HitSkill_AttackPower_1";

    public void SkillAttackBeforeFire(Artifact_YSJ fireArtifact)
    {
    }

    public void SkillAttackAfterFire(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
    }

    public void SkillAttackFlying(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
    }

    public void SKillAttackOnHit(Artifact_YSJ fireArtifact, GameObject spawnedAttack, GameObject hitObject)
    {
        StopBuff();
        _attackPowerCoroutine = StartCoroutine(AttackPowerCoroutine(fireArtifact));
    }

    public void SkillAttackPessive(Artifact_YSJ fireArtifact)
    {
    }

    #region [n초 동안 피해량 증가]
    void OnDisable()
    {
        StopBuff();
    }

    Artifact_YSJ _fireArtifact;
    Coroutine _attackPowerCoroutine = null;
    float _duration = 5f;

    IEnumerator AttackPowerCoroutine(Artifact_YSJ fireArtifact)
    {
        _fireArtifact = fireArtifact;
        StartBuff();

        yield return new WaitForSeconds(_duration);

        EndBuff();
        _fireArtifact = null;

        _attackPowerCoroutine = null;
    }

    void StartBuff()
    {
        float addNormal = 0.15f * _fireArtifact.Default_NormalAttackPower;
        float addSkill = 0.15f * _fireArtifact.Default_SkillAttackPower;
        
        _fireArtifact.Added_NormalAttackPower += addNormal;
        _fireArtifact.Added_SkillAttackPower += addSkill;
        Debug.Log($"[ckt] {partsName} StartBuff {addNormal}_{addSkill}");
    }

    void EndBuff()
    {
        float addNormal = 0.15f * _fireArtifact.Default_NormalAttackPower;
        float addSkill = 0.15f * _fireArtifact.Default_SkillAttackPower;
        
        _fireArtifact.Added_NormalAttackPower -= addNormal;
        _fireArtifact.Added_SkillAttackPower -= addSkill;
        Debug.Log($"[ckt] {partsName} EndBuff {addNormal}_{addSkill}");
    }

    void StopBuff()
    {
        if (_attackPowerCoroutine != null)
        {
            StopCoroutine(_attackPowerCoroutine);
            EndBuff();
            _fireArtifact = null;
            _attackPowerCoroutine = null;
        }
    }
    #endregion
}
