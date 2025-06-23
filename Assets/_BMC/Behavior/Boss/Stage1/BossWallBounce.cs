using Unity.Behavior;
using UnityEngine;

namespace BMC
{
    public class BossWallBounce : MonoBehaviour
    {
        BossFSM _fsm;

        bool _isReflect = false;
        LayerMask _stopLayerMask;
        [SerializeField] BehaviorGraphAgent _behaviorGraphAgent;

        void Awake()
        {
            _fsm = GetComponent<BossFSM>();
            _stopLayerMask = LayerMask.GetMask("Obstacle");
            _behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if ((_stopLayerMask.value & (1 << collision.gameObject.layer)) != 0 && _isReflect == false)
            {
                _isReflect = true;
                Debug.Log("벽 닿음");
                GameManager.Instance.CameraController.ShakeCamera(5f, 0.1f);
                _behaviorGraphAgent.SetVariableValue("IsCollisionWithObstacle", true);

                //Debug.Log("Normal of the first point: " + collision.contacts[0].normal);

                // 입사각
                _behaviorGraphAgent.GetVariable<Vector2>("RushDirection", out BlackboardVariable<Vector2> rushDirection);
                Vector2 inDirection = rushDirection.Value;
                Debug.DrawRay(collision.contacts[0].point, inDirection, Color.blue, 1f);

                // 반사각
                Vector2 reflectDirection = Vector2.Reflect(inDirection, collision.contacts[0].normal);
                Debug.DrawRay(collision.contacts[0].point, reflectDirection, Color.green, 1f);
                _behaviorGraphAgent.SetVariableValue("RushDirection", reflectDirection);

                _fsm.FlipX(reflectDirection.x);

                // 법선 벡터
                Debug.DrawRay(collision.contacts[0].point, collision.contacts[0].normal, Color.red, 1f);

                //Time.timeScale = 0f;
            }
            else
            {
                Debug.Log("벽 닿지 않음");
                //_behaviorGraphAgent.SetVariableValue("IsCollisionWithObstacle", false);
            }
        }

        void OnCollisionExit2D(Collision2D collision)
        {
            if ((_stopLayerMask.value & (1 << collision.gameObject.layer)) != 0)
            {
                _isReflect = false;
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