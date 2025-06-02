using UnityEngine;

namespace CKT
{
    public class ImageParts_T1 : ImageParts, ICastEffectable
    {
        private void Awake()
        {
            base.Init("FieldParts/FieldParts_T1");
        }

        #region [Method]
        public void CastEffect(GameObject origin)
        {
            Vector2 originalVector = origin.GetComponent<Rigidbody2D>().linearVelocity;

            GameObject copyLeft = Instantiate(origin, origin.transform.position, origin.transform.rotation);
            copyLeft.GetComponent<Rigidbody2D>().linearVelocity = RotateVector(originalVector, -15f);

            GameObject copyRight = Instantiate(origin, origin.transform.position, origin.transform.rotation);
            copyRight.GetComponent<Rigidbody2D>().linearVelocity = RotateVector(originalVector, 15f);
        }

        //TODO : Utils에 해당 메소드 넣기
        Vector2 RotateVector(Vector2 vector, float angleDegrees)
        {
            float angleRad = angleDegrees * Mathf.Deg2Rad; // 도를 라디안으로 변환
            float cos = Mathf.Cos(angleRad);
            float sin = Mathf.Sin(angleRad);

            // 2D 벡터 회전 공식 적용
            float newX = vector.x * cos - vector.y * sin;
            float newY = vector.x * sin + vector.y * cos;

            return new Vector2(newX, newY);
        }
        #endregion
    }
}