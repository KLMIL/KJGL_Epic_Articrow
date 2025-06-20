using Game.Enemy;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/*
 * 적중시 적중 대상을 일시적으로 느리게 만드는 파츠
 * 작성자: KWS
 * 작성일자: 2025-06-19
 */

namespace CKT
{
    public class ImageParts_HitSlow : ImageParts, ISkillable
    {
        public float slowMultiply = 0.3f;
        public float slowDuration = 1f;
        public float slowRadius = 1f;

        public GameObject hitSlowPrefab;

        public override Define.SkillType SkillType => Define.SkillType.Hit;
        public override string SkillName => "HitSlow";

        public IEnumerator SkillCoroutine(Vector3 position, Vector3 direction, int level, SkillManager manager)
        {
            Debug.Log($"{SkillName}, Level+{level}");

            Collider2D[] hits = Physics2D.OverlapCircleAll(position, slowRadius, LayerMask.GetMask("Monster"));

            var filterHits = hits
                .Where(h => h != null && h.isTrigger)
                .ToList();

            foreach (Collider2D hit in filterHits)
            {
                if (hit == null) continue;

                float originSpeed = hit.GetComponent<EnemyController>().Status.moveSpeed;

                GameObject hitSlow = Instantiate(hitSlowPrefab);
                hitSlow.GetComponent<HitSlow>().StartSlowCoroutine(slowDuration * level, slowMultiply, hit);

                hit.GetComponent<EnemyController>().StartTintCoroutine(Color.blue, slowDuration * level);
            }

            yield return null;
        }
    }
}
