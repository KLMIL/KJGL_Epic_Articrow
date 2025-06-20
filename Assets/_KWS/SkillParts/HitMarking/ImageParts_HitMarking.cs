using Game.Enemy;
using System.Collections;
using System.Linq;
using UnityEngine;

/*
 * 적중시 적중 대상에게 추가 대미지를 부여하는 파츠
 * 작성자: KWS
 * 작성일자: 2025-06-20
 */

namespace CKT
{
    public class ImageParts_HitMarking: ImageParts, ISkillable
    {
        float hitRadius = 1f;
        float damageMultiply = 2f;
        float damageDuration = 2f;

        public override Define.SkillType SkillType => Define.SkillType.Hit;
        public override string SkillName => "HitMarking";

        public IEnumerator SkillCoroutine(Vector3 position, Vector3 direction, int level, SkillManager skillManager)
        {
            Debug.Log($"{SkillName}, Level+{level}");

            Debug.LogError("Here?");

            Collider2D[] hits = Physics2D.OverlapCircleAll(position, hitRadius, LayerMask.GetMask("Monster"));


            var filterHits = hits
                .Where(h => h != null && h.isTrigger)
                .ToList();

            foreach (Collider2D hit in filterHits)
            {
                if (hit == null) continue;

                Debug.LogError("Here good");
                EnemyController enemyController = hit.GetComponent<EnemyController>();
                enemyController.StartMarkingCoroutine(damageMultiply, damageDuration * level);

                hit.GetComponent<EnemyController>().StartTintCoroutine(new Color(1, 0.5f, 0), damageDuration * level);
            }

            yield return null;
        }
    }

}
