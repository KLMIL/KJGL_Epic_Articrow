using Unity.Behavior;
using UnityEngine;

namespace BMC
{
    public class BossWallBounce : MonoBehaviour
    {
        BossFSM _fsm;
        [SerializeField] BehaviorGraphAgent _behaviorGraphAgent;

        LayerMask _stopLayerMask;

        void Awake()
        {
            _fsm = GetComponent<BossFSM>();
            _behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
            _stopLayerMask = LayerMask.GetMask("Obstacle") | LayerMask.GetMask("Corner");
        }

        bool IsInLayerMask(int layer, LayerMask mask)
        {
            return (mask.value & (1 << layer)) != 0;
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (!IsInLayerMask(collision.gameObject.layer, _stopLayerMask))
                return;

            GameManager.Instance.CameraController.ShakeCamera(5f, 0.1f);
            _behaviorGraphAgent.SetVariableValue("IsCollisionWithObstacle", true);

            // RushDirection 가져오기
            _behaviorGraphAgent.GetVariable<Vector2>("RushDirection", out var rushDirectionVar);
            Vector2 inDirection = rushDirectionVar.Value.normalized;

            // 벽을 향하지 않고 튕길 수도 있으니 보정
            if (Vector2.Dot(inDirection, collision.contacts[0].normal) > 0)
                inDirection = -inDirection;

            // 입사 벡터와 가장 반대 방향(내적이 가장 작은) 접점 노멀 선택
            Vector2 bestNormal = collision.contacts[0].normal;
            float bestDot = Vector2.Dot(inDirection, bestNormal);
            foreach (var contact in collision.contacts)
            {
                float dot = Vector2.Dot(inDirection, contact.normal);
                if (dot < bestDot)
                {
                    bestDot = dot;
                    bestNormal = contact.normal;
                }
            }

            // 반사각 계산
            Vector2 reflectDirection = Vector2.Reflect(inDirection, bestNormal).normalized;

            // 결과 반영
            _behaviorGraphAgent.SetVariableValue("RushDirection", reflectDirection);
            _fsm.FlipX(reflectDirection.x);

            // 디버그
            Vector2 contactPoint = collision.contacts[0].point;
            Debug.DrawRay(contactPoint, inDirection, Color.blue, 1f);       // 입사각
            Debug.DrawRay(contactPoint, bestNormal, Color.red, 1f);         // 법선
            Debug.DrawRay(contactPoint, reflectDirection, Color.green, 1f); // 반사각
        }

        void OnCollisionExit2D(Collision2D collision)
        {
            if(IsInLayerMask(collision.gameObject.layer, _stopLayerMask))
            {
                Debug.Log("벽에서 벗어남");
                _behaviorGraphAgent.SetVariableValue("IsCollisionWithObstacle", false);
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 1f);
            //Collider2D hit = Physics2D.OverlapCircle(Agent.Value.transform.position, 0.5f, LayerMask.GetMask("Obstacle"));
        }
    }
}