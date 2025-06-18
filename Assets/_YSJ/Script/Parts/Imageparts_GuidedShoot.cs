using CKT;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Imageparts_GuidedShoot : ImageParts, ISkillable
{
    public override Define.SkillType SkillType => Define.SkillType.Cast;

    public override string SkillName => "GuidedShoot";

    float guideAngle = 15.0f; // 유도 각도

    private void Start()
    {
    }

    public IEnumerator SkillCoroutine(Vector3 position, Vector3 directoin, int level, SkillManager skillManager)
    {
        //YSJ.Managers.Sound.PlaySFX(Define.SFX.GuidedShoot);

        // for문으로 각도 별로 레이케스트쏘려고 함
        float guideAngle = this.guideAngle * level;
        float DirectionAngle = Mathf.Atan2(directoin.y, directoin.x) * Mathf.Rad2Deg; // 방향 벡터의 각도`

        //for (float f = Mathf.Atan2(directoin.y, directoin.x); f)
        Physics2D.RaycastAll(position, Quaternion.Euler(0, 0, guideAngle) * directoin, 10f);
        yield return null;
    }
}
