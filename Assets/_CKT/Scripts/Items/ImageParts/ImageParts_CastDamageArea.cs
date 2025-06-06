using UnityEngine;

namespace CKT
{
    public class ImageParts_CastDamageArea : ImageParts, ICastEffectable
    {
        private void Awake()
        {
            base.Init("FieldParts/FieldParts_CastDamageArea");
        }

        #region [ICastEffectable]
        public void CastEffect(int handID)
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
                skillManager.CastDamageAreaLevelUp(1);
            }
        }
        #endregion
    }
}