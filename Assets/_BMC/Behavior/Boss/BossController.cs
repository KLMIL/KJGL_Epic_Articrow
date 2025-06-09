using System.Reflection;
using Unity.Behavior;
using UnityEditor;
using UnityEngine;

namespace BMC
{
    public class BossController : MonoBehaviour
    {
        Transform _visual;
        Rigidbody2D _rb;

        [SerializeField] BehaviorGraphAgent _behaviorGraphAgent;
        [SerializeField] Transform _target;
        [SerializeField] LayerMask _stopLayerMask;

        public Vector2 RushDirection { get; set; } // 돌진 방향

        bool _isReflect = false;

        void Awake()
        {
            _visual = transform.Find("Visual");
            _rb = GetComponent<Rigidbody2D>();
            _stopLayerMask = LayerMask.GetMask("Player") | LayerMask.GetMask("Obstacle");
            _behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
        }

        void Start()
        {
            _target = GameObject.FindWithTag("Player").transform;
            _behaviorGraphAgent.SetVariableValue("Target", _target.gameObject);
        }

        void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Q))
            //    Flip(1);
            //else if (Input.GetKeyDown(KeyCode.E))
            //    Flip(-1);
        }

        // x 방향으로 비주얼 회전
        public void Flip(float x)
        {
            float angle = (x>=0) ? 0 : 180;
            _visual.rotation = Quaternion.Euler(0, angle, 0);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if ((_stopLayerMask.value & (1 << collision.gameObject.layer)) != 0 && _isReflect == false)
            {
                _isReflect = true;

                Debug.Log("벽 닿음");
                _behaviorGraphAgent.SetVariableValue("IsCollisionWithObstacle", true);

                Debug.Log("Normal of the first point: " + collision.contacts[0].normal);

                // 입사각
                _behaviorGraphAgent.GetVariable<Vector2>("RushDirection", out BlackboardVariable<Vector2> rushDirection);
                Vector2 inDirection = rushDirection.Value;
                Debug.DrawRay(collision.contacts[0].point, inDirection, Color.blue, 1f);

                // 반사각
                Vector2 reflectDirection = Vector2.Reflect(inDirection, collision.contacts[0].normal);
                Debug.DrawRay(collision.contacts[0].point, reflectDirection, Color.green, 1f);
                _behaviorGraphAgent.SetVariableValue("RushDirection", reflectDirection);

                // 법선 벡터
                Debug.DrawRay(collision.contacts[0].point, collision.contacts[0].normal, Color.red, 1f);

                //Time.timeScale = 0f;
            }
            else
            {
                Debug.Log("벽 닿지 않음");
                _behaviorGraphAgent.SetVariableValue("IsCollisionWithObstacle", false);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if ((_stopLayerMask.value & (1 << collision.gameObject.layer)) != 0)
            {
                _isReflect = false;
                Debug.Log("벽에서 벗어남");
                _behaviorGraphAgent.SetVariableValue("IsCollisionWithObstacle", false);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 1f);
            //Collider2D hit = Physics2D.OverlapCircle(Agent.Value.transform.position, 0.5f, LayerMask.GetMask("Obstacle"));
        }
    }
}