using Game.Enemy;
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
        ManaGain(fireArtifact, spawnedAttack, hitObject);
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
        ManaGain(fireArtifact, spawnedAttack, hitObject);
    }
    #endregion

    #region [상세]
    void ManaGain(Artifact_YSJ fireArtifact, GameObject spawnedAttack, GameObject hitObject)
    {
        EnemyController enemy = hitObject.transform.GetComponentInParent<EnemyController>();
        MagicRoot_YSJ magic = spawnedAttack.GetComponent<MagicRoot_YSJ>();

        if ((enemy != null) && (magic != null))
        {
            float enemyHealth = enemy.Status.healthPoint;
            float attackPower = magic.AttackPower;
            //Debug.Log($"[ckt] {partsName} enemyHealth:{enemyHealth}, attackPower:{attackPower}");
            if (enemyHealth <= attackPower)
            {
                int add = _increaseValue * 2;

                fireArtifact.playerStatus.RegenerateMana(add);
                //Debug.Log($"[ckt] {partsName} RegenerateMana({add})");
            }
        }
        else
        {
            //Debug.LogWarning($"[ckt] {partsName} EnemyController or MagicRoot_YSJ is null");
        }
    }
    #endregion
}
