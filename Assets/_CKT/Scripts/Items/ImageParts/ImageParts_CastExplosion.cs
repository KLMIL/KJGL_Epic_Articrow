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
            if (handID == 1)
            {
                GameManager.Instance.LeftSkillManager.CastExplosionLevelUp(1);
            }
            else if (handID == 2)
            {
                GameManager.Instance.RightSkillManager.CastExplosionLevelUp(1);
            }
        }
        #endregion
    }
}