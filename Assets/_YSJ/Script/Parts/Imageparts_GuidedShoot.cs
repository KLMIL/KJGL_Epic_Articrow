using CKT;
using Game.Enemy;
using System.Collections;
using System.Collections.Generic;
using Unity.AppUI.UI;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Imageparts_GuidedShoot : ImageParts, ISkillable
{
    public override Define.SkillType SkillType => Define.SkillType.Cast;

    public override string SkillName => "GuidedShoot";

    float guideAngle = 15.0f; // 유도 각도
    float distance = 10.0f; // 유도 거리

    private void Start()
    {
    }

    public IEnumerator SkillCoroutine(Vector3 position, Vector3 directoin, int level, SkillManager skillManager)
    {
        //YSJ.Managers.Sound.PlaySFX(Define.SFX.GuidedShoot);

        // for문으로 각도 별로 레이케스트쏘려고 함
        float guideAngle = this.guideAngle * level;
        float DirectionAngle = Mathf.Atan2(directoin.y, directoin.x) * Mathf.Rad2Deg; // 방향 벡터의 각도

        List<Collider2D> hitObjects = new();

        // 2도 단위로 레이캐스트쏘고 몬스터를 찾으면 리스트에 추가
        for (float f = DirectionAngle - guideAngle / 2; f < DirectionAngle + guideAngle / 2; f += 2f)
        {
            RaycastHit2D hit = Physics2D.Raycast(position, Quaternion.Euler(0, 0, f) * Vector2.right, distance, LayerMask.GetMask("Monster"));
            Debug.DrawLine(position, position + Quaternion.Euler(0, 0, f) * Vector2.right * distance, Color.red, 1.5f);
            if (hit.collider && hit.collider.GetComponent<EnemyController>() && !hitObjects.Contains(hit.collider))
            {
                // 히트된 오브젝트에 대한 처리
                hitObjects.Add(hit.collider);
            }
        }

        // 찾은 몬스터 중 가장 가까운 몬스터를 찾음
        Transform nearestTarget = null;
        foreach (Collider2D hit in hitObjects)
        {
            if (nearestTarget == null || Vector2.Distance(position, nearestTarget.position) > Vector2.Distance(position, hit.transform.position)) 
            {
                nearestTarget = hit.transform;
            }
        }

        Debug.Log(nearestTarget != null ? $"Nearest Target: {nearestTarget.name}" : "No target found.");

        // 가장 가까운 몬스터를 향해 발사 -> 원본의 정보를 알 수 있게 되면 그때 작성
        if (nearestTarget != null)
        {
        }

        yield return null;
    }
}
