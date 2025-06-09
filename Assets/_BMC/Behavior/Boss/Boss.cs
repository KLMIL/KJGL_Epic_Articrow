using Unity.Behavior;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] BehaviorGraphAgent _behaviorGraphAgent;
    [SerializeField] Transform _target;
    [SerializeField] LayerMask _stopLayerMask;

    void Start()
    {
        _stopLayerMask = LayerMask.GetMask("Player") | LayerMask.GetMask("Obstacle");
        _target = GameObject.FindWithTag("Player").transform;
        _behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
        _behaviorGraphAgent.SetVariableValue("Target", _target.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if ((_stopLayerMask.value & (1 << collision.gameObject.layer)) != 0)
        {
            Debug.Log("벽 닿음");
            _behaviorGraphAgent.SetVariableValue("IsCanRush", false);
        }
        else
        {
            Debug.Log("벽 닿지 않음");
            _behaviorGraphAgent.SetVariableValue("IsCanRush", true);
        }

        //if (LayerMask.NameToLayer("Obstacle") == collision.gameObject.layer)
        //{
        //    Debug.Log("벽 닿음");
        //    _behaviorGraphAgent.SetVariableValue("IsCanRush", false);
        //}
        //else
        //{
        //    Debug.Log("벽 닿지 않음");
        //    _behaviorGraphAgent.SetVariableValue("IsCanRush", true);
        //}
    }
}