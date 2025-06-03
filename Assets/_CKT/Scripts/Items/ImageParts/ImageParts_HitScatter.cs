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
        public void HitEffect(GameObject hitBox)
        {
            Debug.Log("HitScatter");
        }
        #endregion
    }
}