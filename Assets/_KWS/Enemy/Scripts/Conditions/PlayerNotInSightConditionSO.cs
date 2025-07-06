using UnityEngine;

namespace Game.Enemy
{
    [CreateAssetMenu(fileName = "PlayerNotInSightCondition", menuName = "Enemy/Condition/Player Not In Sight")]
    public class PlayerNotInSightConditionSO : EnemyConditionSO
    {
        public float detectionRange = 5f;
        public float boxWidth = 0.5f;

        public override bool IsMet(EnemyController controller)
        {
            if (controller.Player == null) return false;

            Vector2 from = controller.transform.position;
            Vector2 to = controller.Player.position;
            Vector2 direction = (to - from).normalized;
            float distance = Vector2.Distance(from, to);

            // 거리가 모자라면 false
            if (distance > detectionRange)
            {
                return true;
            }

            // BoxCast로 장애물 감지
            RaycastHit2D hit = Physics2D.BoxCast(
                    from,
                    new Vector2(boxWidth, 0.1f),
                    0f,
                    direction,
                    distance,
                    LayerMask.GetMask("Obstacle")
            );

            // ----[디버그: BoxCast 경로 시각화]----
#if UNITY_EDITOR
            // 박스의 4개 꼭짓점 계산 (2D에서만, 박스 회전X 가정)
            Vector2 perp = Vector2.Perpendicular(direction) * (new Vector2(boxWidth, 0.1f) * 0.5f);
            Vector2 startLeft = from + perp;
            Vector2 startRight = from - perp;
            Vector2 endLeft = startLeft + direction * distance;
            Vector2 endRight = startRight + direction * distance;

            Debug.DrawLine(startLeft, startRight, Color.cyan, 0.1f);
            Debug.DrawLine(endLeft, endRight, Color.cyan, 0.1f);
            Debug.DrawLine(startLeft, endLeft, Color.cyan, 0.1f);
            Debug.DrawLine(startRight, endRight, Color.cyan, 0.1f);
#endif
            // ------------------------------------

            return !(hit.collider == null);
        }
    }
}
