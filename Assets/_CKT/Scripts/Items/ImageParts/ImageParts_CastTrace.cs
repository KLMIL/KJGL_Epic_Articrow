using CKT;
using UnityEngine;

namespace CKT
{
    public class ImageParts_CastTrace : ImageParts, ICastEffectable
    {
        private void Awake()
        {
            base.Init("FieldParts/FieldParts_CastTrace");
        }

        #region [ICastEffectable]
        public void CastEffect(int handID)
        {

        }
        #endregion
    }
}