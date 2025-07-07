using System.Collections;
using UnityEngine;

namespace CKT
{
    public class ImageParts_GravitySurge : ImageParts, ISkillable
    {
        [SerializeField] GameObject HitKnockbackFieldPrefab;

        public override Define.SkillType SkillType => Define.SkillType.Hit;
        public override string SkillName => "HitGravitySurge";

        public IEnumerator SkillCoroutine(Vector3 position, Vector3 direction, int level, SkillManager skillManager)
        {
            Debug.Log($"{SkillName}, Level+{level}");

            Vector3 startPos = position;

            // TODO: 사운드 적절한 넉백 사운드로 바꾸기
            //YSJ.Managers.Sound.PlaySFX(Define.SFX.HitExplosion);
            
            GameObject obj = Instantiate(HitKnockbackFieldPrefab, startPos, Quaternion.identity);
            obj.transform.position = startPos;
            obj.GetComponent<HitGravitySurge>().Init(level);

            // 프리펩에서 직접 제거하도록 변경
            Destroy(obj, 0.5f * level + 0.1f);

            yield return null;
        }
    }

}
