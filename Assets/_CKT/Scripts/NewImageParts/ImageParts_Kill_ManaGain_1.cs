using Game.Enemy;
using System.Collections;
using UnityEngine;

/// <summary>
/// 대상을 처치했을 때 마나 1 회복
/// </summary>
public class ImageParts_Kill_ManaGain_1 : ImagePartsRoot_YSJ, IImagePartsToNormalAttack_YSJ, IImagePartsToSkillAttack_YSJ
{
    int _increaseValue = 1;

    public override string partsName => "Kill_ManaGain_1";

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
        StartCoroutine(ManaGainCoroutine(fireArtifact, spawnedAttack, hitObject));
    }
    #endregion

    #region [Skill]
    public void SkillAttackPessive(Artifact_YSJ fireArtifact)
    {
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
        StartCoroutine(ManaGainCoroutine(fireArtifact, spawnedAttack, hitObject));
    }
    #endregion

    #region [상세]
    IEnumerator ManaGainCoroutine(Artifact_YSJ fireArtifact, GameObject spawnedAttack, GameObject hitObject)
    {
        yield return null;

        float enemyHealth = hitObject.GetComponent<EnemyController>().Status.healthPoint;
        if (enemyHealth <= 0)
        {
            int add = _increaseValue * 2;
            
            fireArtifact.playerStatus.RegenerateMana(add);
            Debug.Log($"[ckt] {partsName} RegenerateMana({add})");
        }
    }
    #endregion
}
