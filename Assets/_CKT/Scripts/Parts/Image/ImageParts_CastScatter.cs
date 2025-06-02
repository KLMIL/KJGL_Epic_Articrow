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
        public void CastEffect(GameObject origin)
        {
            /*Vector2 originalVector = origin.GetComponent<Rigidbody2D>().linearVelocity;

            GameObject copyLeft = Instantiate(origin, origin.transform.position, origin.transform.rotation);
            copyLeft.GetComponent<Rigidbody2D>().linearVelocity = RotateVector(originalVector, -15f);

            GameObject copyRight = Instantiate(origin, origin.transform.position, origin.transform.rotation);
            copyRight.GetComponent<Rigidbody2D>().linearVelocity = RotateVector(originalVector, 15f);*/
        }
        #endregion
    }
}