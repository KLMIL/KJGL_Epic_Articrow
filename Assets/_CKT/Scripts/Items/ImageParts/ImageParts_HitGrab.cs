using BMC;
using System.Collections;
using UnityEngine;

namespace CKT
{
    public class ImageParts_HitGrab : ImageParts, ISkillable
    {
        public override Define.SkillType SkillType => Define.SkillType.Hit;

        public override string SkillName => "HitGrab";

        public IEnumerator SkillCoroutine(Vector3 position, Vector3 direction, int level, SkillManager skillManager)
        {
            Debug.Log($"{SkillName}, Level+{level}");

            GameObject grabObject = YSJ.Managers.TestPool.Get<GameObject>(Define.PoolID.GrabObject);
            grabObject.transform.position = position;
            //grabObject.transform.localScale = origin.transform.localScale;
            grabObject.GetComponent<GrabObject>().Init(level);

            yield return null;
        }
    }
}