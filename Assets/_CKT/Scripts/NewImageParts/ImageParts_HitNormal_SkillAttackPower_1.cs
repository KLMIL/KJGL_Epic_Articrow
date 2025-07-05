using System.Collections;
using UnityEngine;

/// <summary>
/// 일반 공격 적중 시 스킬 공격 피해량 5% 증가 (최대 6중첩, 5초 지속, 효과 발동 시 지속 시간 갱신)
/// </summary>
public class ImageParts_HitNormal_SkillAttackPower_1 : ImagePartsRoot_YSJ, IImagePartsToNormalAttack_YSJ
{
    Artifact_YSJ _fireArtifact = null;
    Coroutine _stackCoroutine = null;
    float _increasePercent = 5f;
    int _curStack = 0;
    int _maxStack = 6;
    float _duration = 5f;
    
    public override string partsName => "HitNormal_SkillAttackPower_1";

    #region [Normal]
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
        StopBuff();
        _stackCoroutine = StartCoroutine(StackCoroutine(fireArtifact));
    }

    public void NormalAttackPessive(Artifact_YSJ fireArtifact)
    {
    }
    #endregion

    #region [상세]
    void OnDisable()
    {
        StopBuff();
    }

    IEnumerator StackCoroutine(Artifact_YSJ fireArtifact)
    {
        _fireArtifact = fireArtifact;
        StartBuff();

        yield return new WaitForSeconds(_duration);

        EndBuff();
        _fireArtifact = null;
        _stackCoroutine = null;
    }

    void StartBuff()
    {
        _curStack++;
        _curStack = Mathf.Clamp(_curStack, 0, _maxStack);
        float add = (_increasePercent * 0.01f) * _curStack * _fireArtifact.skillStatus.Default_AttackPower;

        _fireArtifact.skillStatus.Added_AttackPower += add;
        Debug.Log($"[ckt] {partsName} StartBuff {_curStack}_{add}");
    }

    void EndBuff()
    {
        float add = (_increasePercent * 0.01f) * _curStack * _fireArtifact.skillStatus.Default_AttackPower;

        _fireArtifact.skillStatus.Added_AttackPower -= add;
        _curStack = 0;
        Debug.Log($"[ckt] {partsName} StartBuff {_curStack}_{add}");
    }

    void StopBuff()
    {
        if (_stackCoroutine != null)
        {
            StopCoroutine(_stackCoroutine);
            EndBuff();
            _fireArtifact = null;
            _stackCoroutine = null;
        }
    }
    #endregion
}
