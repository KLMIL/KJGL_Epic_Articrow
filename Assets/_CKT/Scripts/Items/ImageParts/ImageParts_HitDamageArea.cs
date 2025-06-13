using CKT;
using System.Collections;
using UnityEngine;

public class ImageParts_HitDamageArea : ImageParts, ISkillable
{
    private void Awake()
    {
        base.Init("FieldParts/FieldParts_HitDamageArea");
    }

    public SkillType SkillType => SkillType.Hit;

    public string SkillName => "HitDamageArea";

    public IEnumerator SkillCoroutine(GameObject origin, int level, SkillManager skillManager)
    {
        Debug.Log($"{SkillName}, Level+{level}");

        Vector3 startPos = origin.transform.position;

        for (int i = 0; i < level; i++)
        {
            YSJ.Managers.Sound.PlaySFX(Define.SFX.HitDamageArea);

            GameObject hitDamageArea = YSJ.Managers.Pool.InstPrefab("HitDamageArea");
            hitDamageArea.transform.position = startPos;
            yield return new WaitForSeconds(0.05f);
        }
        yield return null;
    }
}
