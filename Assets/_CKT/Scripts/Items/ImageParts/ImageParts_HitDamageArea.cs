using BMC;
using System.Collections;
using UnityEngine;

namespace CKT
{
    public class ImageParts_HitDamageArea : ImageParts, ISkillable
    {
        private void Awake()
        {
            base.Init("FieldParts/FieldParts_HitDamageArea", 0f);
        }

        public SkillType SkillType => SkillType.Hit;

        public string SkillName => "HitDamageArea";

        public IEnumerator SkillCoroutine(GameObject origin, int level, SkillManager skillManager)
        {
            Debug.Log($"{SkillName}, Level+{level}");

            Vector3 startPos = origin.transform.position;

            /*for (int i = 0; i < level; i++)
            {

                GameObject hitDamageArea = YSJ.Managers.Pool.InstPrefab("HitDamageArea");
                hitDamageArea.transform.position = startPos;
                yield return new WaitForSeconds(0.05f);
            }*/

            YSJ.Managers.Sound.PlaySFX(Define.SFX.HitDamageArea);
            GameObject hitDamageArea = YSJ.Managers.TestPool.Get<GameObject>(Define.PoolID.HitDamageArea);
            hitDamageArea.transform.position = startPos;
            hitDamageArea.GetComponent<DamageArea>().Init(level);

            PlayerManager.Instance.PlayerStatus.SpendMana(base._manaCost * level);
            yield return null;
        }
    }
}