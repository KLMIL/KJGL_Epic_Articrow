using UnityEngine;

namespace CKT
{
    public class ImageParts_HitGrab : ImageParts, IHitEffectable
    {
        private void Awake()
        {
            base.Init("FieldParts/FieldParts_HitGrab");
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
                skillManager.HitGrabLevelUp(1);
            }
        }
        #endregion
    }
}