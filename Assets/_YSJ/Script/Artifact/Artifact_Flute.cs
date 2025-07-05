using UnityEngine;

public class Artifact_Flute : Artifact_YSJ
{
    private void Start()
    {
        ArtifactInitialize();
    }

    void SpawnedAttackSetChild(Artifact_YSJ me, GameObject spawnedAttack)
    {
        spawnedAttack.transform.SetParent(transform);
    }

    protected override void ResetSkillAttack()
    {
        base.ResetSkillAttack();
        skillStatus.AfterFire += SpawnedAttackSetChild;
    }
}