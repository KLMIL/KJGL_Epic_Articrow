using CKT;
using UnityEngine;

public class ImageParts_CastAdditional : ImageParts, ICastEffectable
{
    private void Awake()
    {
        base.Init("FieldParts/FieldParts_CastAdditional");
    }

    #region [ICastEffectable]
    public void CastEffect(EquipedArtifact artifact)
    {
        artifact.AddAttackCount(1);
    }
    #endregion
}
