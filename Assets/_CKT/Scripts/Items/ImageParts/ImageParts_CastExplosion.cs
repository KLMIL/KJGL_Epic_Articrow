using CKT;
using UnityEngine;

namespace CKT
{
    public class ImageParts_CastExplosion : ImageParts, ICastEffectable
    {
        private void Awake()
        {
            base.Init("FieldParts/FieldParts_CastExplosion");
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
                skillManager.CastExplosionLevelUp(1);
            }
        }
        #endregion
    }
}