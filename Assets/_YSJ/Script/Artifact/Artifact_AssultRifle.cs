using System.Collections;
using UnityEngine;
using YSJ;

public class Artifact_AssultRifle : Artifact_YSJ
{
    private void Start()
    {
        ArtifactInitialize();   
    }

    public override void SkillAttackClicked()
    {
        isCanLeftClick=false;
        base.SkillAttackClicked();
    }

    public override void SkillAttackCancled()
    {
        isCanLeftClick =true;
        base.SkillAttackCancled();
    }
}
