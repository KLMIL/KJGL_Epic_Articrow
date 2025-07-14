using System.Collections;
using UnityEngine;

/// <summary>
/// 일반 공격 적중 시 스킬 공격 피해량 10% 증가 (최대 4중첩, 3초 지속, 효과 발동 시 지속 시간 갱신)
/// </summary>
public class ImageParts_HitNormal_SkillAttackPower_1 : ImagePartsRoot_YSJ, IImagePartsToNormalAttack_YSJ, IImagePartsToSkillAttack_YSJ
{
    Coroutine _stackCoroutine = null;
    float _increasePercent = 10f;
    int _curStack = 0;
    int _maxStack = 4;
    float _duration = 3f;
    
    public override string partsName => "HitNormal_SkillAttackPower_1";

    #region [Normal]
    public void NormalAttackPessive(Artifact_YSJ fireArtifact)
    {
    }

    public void NormalAttackBeforeFire(Artifact_YSJ fireArtifact)
    {
    }

    public void NormalAttackAfterFire(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
    }

    public void NormalAttackFlying(Artifact_YSJ fireArtifact, GameObject spawnedAttack)
    {
    }

    public void NormalAttackOnHit(Artifact_YSJ fireArtifact, GameObject spawnedAttack, GameObject hitObject)
    {
        if (_stackCoroutine != null)
        {
            StopBuff();
        }
        _stackCoroutine = StartCoroutine(StackCoroutine(fireArtifact));
    }
    #endregion

    #region [Skill]
    public void SkillAttackPessive(Artifact_YSJ fireArtifact)
    {
        float add = _curStack * _increasePercent * fireArtifact.skillStatus.Default_AttackPower;
        add = Mathf.RoundToInt(add) / 100f;

        fireArtifact.skillStatus.Added_AttackPower += add;
        Debug.Log($"[ckt] {partsName} StartBuff {_curStack}_{add}");
    }

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
    }
    #endregion

    #region [상세]
    void OnDisable()
    {
        if (_stackCoroutine != null)
        {
            EndBuff();
            StopBuff();
        }
    }

    IEnumerator StackCoroutine(Artifact_YSJ fireArtifact)
    {
        StartBuff();

        yield return new WaitForSeconds(_duration);

        EndBuff();
        _stackCoroutine = null;
    }

    void StartBuff()
    {
        _curStack++;
        _curStack = Mathf.Clamp(_curStack, 0, _maxStack);
        Debug.Log($"[ckt] {partsName} StartBuff stack_{_curStack}");
    }

    void EndBuff()
    {
        _curStack = 0;
        Debug.Log($"[ckt] {partsName} EndBuff stack_{_curStack}");
    }

    void StopBuff()
    {
        StopCoroutine(_stackCoroutine);
        _stackCoroutine = null;
    }
    #endregion
}
