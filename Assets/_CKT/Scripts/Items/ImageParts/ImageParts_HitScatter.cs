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
            if (handID == 1)
            {
                GameManager.Instance.LeftSkillManager.HitScatterLevelUp(1);
            }
            else if (handID == 2)
            {
                GameManager.Instance.RightSkillManager.HitScatterLevelUp(1);
            } 
        }
        #endregion
    }
}