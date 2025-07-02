using System.Collections;
using UnityEngine;

public class ImageParts_HitNormal_SkillAttackPower_1 : ImagePartsRoot_YSJ, IImagePartsToNormalAttack_YSJ
{
    public override string partsName => "HitNormal_SkillAttackPower_1";

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

    #region [일반 공격 적중 시 스킬 공격 피해 증가 (5%, 최대 6스택, 5초 지속)]
    void OnDisable()
    {
        StopBuff();
    }

    Artifact_YSJ _fireArtifact = null;
    Coroutine _stackCoroutine = null;
    float _power = 5f;
    int _curStack = 0;
    int _maxStack = 6;
    float _duration = 5f;

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
        float add = _power * _curStack * _fireArtifact.Default_SkillAttackPower;

        _fireArtifact.Added_SkillAttackPower += add;
        Debug.Log($"[ckt] {partsName} StartBuff {_curStack}_{add}");
    }

    void EndBuff()
    {
        float add = _power * _curStack * _fireArtifact.Default_SkillAttackPower;

        _fireArtifact.Added_SkillAttackPower -= add;
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
