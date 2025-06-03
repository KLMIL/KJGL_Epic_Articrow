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
        public void CastEffect(GameObject artifact)
        {
            artifact.GetComponent<EquipedArtifact>().AddScatterCount(2);
        }
        #endregion
    }
}