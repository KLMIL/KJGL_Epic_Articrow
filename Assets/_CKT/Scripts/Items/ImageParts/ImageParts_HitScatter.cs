using CKT;
using UnityEngine;

namespace CKT
{
    public class ImageParts_HitScatter : ImageParts, IHitEffectable
    {
        private void Awake()
        {
            base.Init("FieldParts/FieldParts_HitScatter");
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
                skillManager.HitScatterLevelUp(1);
            }
        }
        #endregion
    }
}