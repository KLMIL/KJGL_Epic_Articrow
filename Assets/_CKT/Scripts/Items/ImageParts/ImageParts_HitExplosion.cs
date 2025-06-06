using UnityEngine;

namespace CKT
{
    public class ImageParts_HitExplosion : ImageParts, IHitEffectable
    {
        private void Awake()
        {
            base.Init("FieldParts/FieldParts_HitExplosion");
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
                skillManager.HitExplosionLevelUp(1);
            }
        }
        #endregion
    }
}