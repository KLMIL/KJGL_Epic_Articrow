using CKT;
using UnityEngine;

public class ImageParts_CastExplosion : ImageParts, ICastEffectable
{
    private void Awake()
    {
        base.Init("FieldParts/FieldParts_CastExplosion");
    }

    #region [ICastEffectable]
    public void CastEffect(EquipedArtifact artifact)
    {
        GameObject explosion = YSJ.Managers.Pool.InstPrefab("Explosion", null, artifact.transform.position);
    }
    #endregion
}
