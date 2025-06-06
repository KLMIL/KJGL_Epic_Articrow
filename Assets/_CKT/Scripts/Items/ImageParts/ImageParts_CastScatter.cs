using UnityEngine;

namespace CKT
{
    public class ImageParts_CastScatter : ImageParts, ICastEffectable
    {
        private void Awake()
        {
            base.Init("FieldParts/FieldParts_CastScatter");
        }

        #region [ICastEffectable]
        public void CastEffect(int handID)
        {
            if (handID == 1)
            {
                GameManager.Instance.LeftSkillManager.CastScatterLevelUp(1);
            }
            else if (handID == 2)
            {
                GameManager.Instance.RightSkillManager.CastScatterLevelUp(1);
            }
        }
        #endregion
    }
}