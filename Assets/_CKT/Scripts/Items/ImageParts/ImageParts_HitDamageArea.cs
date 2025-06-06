using CKT;
using UnityEngine;

public class ImageParts_HitDamageArea : ImageParts, IHitEffectable
{
    private void Awake()
    {
        base.Init("FieldParts/FieldParts_HitDamageArea");
    }

    #region [IHitEffectable]
    public void HitEffect(int handID)
    {
        SkillManager skillManager = null;
        if (handID == 1)
        {
            skillManager = GameManager.Instance.LeftSkillManager;
        }
        else if (handID == 2)
        {
            skillManager = GameManager.Instance.RightSkillManager;
        }

        if (skillManager != null)
        {
            skillManager.HitDamageAreaLevelUp(1);
        }
    }
    #endregion
}
