using System.Collections;
using System.Linq;
using UnityEngine;

/*
 * 적중시 적중 대상 주위의 n마리 적에게 추가 공격을 가하는 파츠
 * 작성자: KWS
 * 작성일자: 2025-06-19
 */

namespace CKT
{
    public class ImageParts_HitSpark : ImageParts, ISkillable
    {
        public float baseDamage = 20f;
        public float radius = 4f;
        public int baseTarget = 3;

        public override Define.SkillType SkillType => Define.SkillType.Hit;
        public override string SkillName => "HitSpark";

        public IEnumerator SkillCoroutine(Vector3 position, Vector3 direction, int level, SkillManager skillManager)
        {
            Debug.Log($"{SkillName}, Level+{level}");

            Vector3 startPos = position;

            // TODO: 사운드 적절한 파지직 사운드로 바꾸기
            YSJ.Managers.Sound.PlaySFX(Define.SFX.HitExplosion);

            // CircleCast로 Enemy 감지
            Collider2D[] hits = Physics2D.OverlapCircleAll(position, radius, LayerMask.GetMask("Monster"));

            // 거리 순으로 정렬
            var sorted = hits.OrderBy(h => Vector2.Distance(h.transform.position, position)).ToList();

            // 타격할 최대 수 
            int maxCount = baseTarget * level;

            // 가까운 순서로 n개 공격 -> level당 갯수 추가
            int count = 0;
            foreach (var enemy in sorted)
            {
                if (enemy == null) continue;
                // TODO: 자기자신, 이미 피격된 적 거르기

                enemy.GetComponent<IDamagable>().TakeDamage(baseDamage);
                Debug.LogError($"Hit Enemy{enemy.GetInstanceID()}");

                count++;
                if (count >= maxCount) break;
            }

            yield return null;
        }
    }
}
