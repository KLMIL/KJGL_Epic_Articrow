using CKT;
using UnityEngine;

namespace CKT
{
    public class ImageParts_CastAdditional : ImageParts, ICastEffectable
    {
        private void Awake()
        {
            base.Init("FieldParts/FieldParts_CastAdditional");
        }

        #region [ICastEffectable]
        public void CastEffect(int handID)
        {
            if (handID == 1)
            {
                GameManager.Instance.LeftSkillManager.CastAdditionalLevelUp(1);
            }
            else if (handID == 2)
            {
                GameManager.Instance.RightSkillManager.CastAdditionalLevelUp(1);
            }
        }
        #endregion
    }
}