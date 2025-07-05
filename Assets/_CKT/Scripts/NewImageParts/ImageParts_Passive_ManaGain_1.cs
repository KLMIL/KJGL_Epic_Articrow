using UnityEngine;

public class ImageParts_Passive_ManaGain_1 : ImagePartsRoot_YSJ, IImagePartsToEnhance_YSJ, IImagePartsToNormalAttack_YSJ
{
    public override string partsName => "Passive_ManaGain_1";

    public void Equip(Artifact_YSJ currentArtifact)
    {
        float add = 0.1f * currentArtifact.normalStatus.Default_AttackCoolTime;

        currentArtifact.normalStatus.Added_AttackCoolTime += add;
        Debug.Log($"[ckt] {partsName} AddCoolTime({add})");
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
        int add = 1;

        fireArtifact.playerStatus.RegenerateMana(add);
        Debug.Log($"[ckt] {partsName} RegenerateMana({add})");
    }

    public void NormalAttackPessive(Artifact_YSJ fireArtifact)
    {
    }
}
