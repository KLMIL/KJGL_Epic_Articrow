using BMC;
using System.Collections;
using UnityEngine;
using YSJ;

namespace CKT
{
    public class ImageParts_CastDamageArea : ImageParts, ISkillable
    {
        private void Awake()
        {
            base.Init("FieldParts/FieldParts_CastDamageArea", 0f);
        }

        public SkillType SkillType => SkillType.Cast;

        public string SkillName => "CastDamageArea";

        public IEnumerator SkillCoroutine(GameObject origin, int level, SkillManager skillManager)
        {
            Debug.Log($"{SkillName}, Level+{level}");

            Vector3 startPos = origin.transform.position;

            /*for (int i = 0; i < level; i++)
            {
                GameObject castDamageArea = YSJ.Managers.Pool.InstPrefab("CastDamageArea");
                castDamageArea.transform.position = startPos;
                yield return new WaitForSeconds(0.05f);
            }*/

            Managers.Sound.PlaySFX(Define.SFX.CastDamageArea);
            GameObject castDamageArea = YSJ.Managers.TestPool.Get<GameObject>(Define.PoolID.CastDamageArea);
            castDamageArea.transform.position = startPos;
            castDamageArea.GetComponent<DamageArea>().Init(level);

            PlayerManager.Instance.PlayerStatus.SpendMana(base._manaCost * level);
            yield return null;
        }
    }
}