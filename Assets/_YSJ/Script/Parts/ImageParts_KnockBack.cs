using CKT;
using Game.Enemy;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ImageParts_KnockBack : ImageParts, ISkillable
{
    public override Define.SkillType SkillType => Define.SkillType.Hit;

    public override string SkillName => "KnockBack";

    public IEnumerator SkillCoroutine(Vector3 position, Vector3 directoin, int level, SkillManager skillManager)
    {
        // 쏜 방향으로 레이캐스트 쏴서 몬스터가 맞으면 그친구 넉백
        RaycastHit2D hit = Physics2D.Raycast(position, (Vector2)directoin, .5f, LayerMask.GetMask("Monster"));
        if (hit.collider != null && hit.collider.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb2d))
        {
            hit.collider.GetComponent<EnemyController>().StopAllCoroutines();
            hit.collider.GetComponent<EnemyController>().StartKnockbackCoroutine(directoin, 3f * level, 0.1f, 10);
        }

        yield return null;
    }
}
