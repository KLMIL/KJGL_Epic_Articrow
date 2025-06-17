using BMC;
using System.Collections;
using UnityEngine;

namespace CKT
{
    public class ImageParts_HitGrab : ImageParts, ISkillable
    {
        private void Awake()
        {
            base.Init("FieldParts/FieldParts_HitGrab", 0f);
        }

        public SkillType SkillType => SkillType.Hit;

        public string SkillName => "HitGrab";

        public IEnumerator SkillCoroutine(GameObject origin, int level, SkillManager skillManager)
        {
            Debug.Log($"{SkillName}, Level+{level}");

            if (level > 0)
            {
                GameObject grabObject = YSJ.Managers.Pool.InstPrefab("GrabObject");
                grabObject.transform.position = origin.transform.position;
                //grabObject.transform.localScale = origin.transform.localScale;
                grabObject.GetComponent<GrabObject>().Level = level;
            }

            PlayerManager.Instance.PlayerStatus.SpendMana(base._manaCost * level);
            yield return null;
        }
    }
}