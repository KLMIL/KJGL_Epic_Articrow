using CKT;
using UnityEngine;

public class ImageParts_CastExplosion : ImageParts, ICastEffectable
{
    private void Awake()
    {
        base.Init("FieldParts/FieldParts_CastExplosion");
    }

    #region [ICastEffectable]
    public void CastEffect(GameObject origin)
    {
        //TODO : 폭발
        Debug.Log("폭발");
        GameObject explosion = YSJ.Managers.Pool.InstPrefab("Explosion", null, origin.transform.position);
    }
    #endregion
}
