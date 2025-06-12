using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using BMC;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SpiralShoot", story: "[Self] shoot [Projectile] spiral", category: "Action", id: "ad6c5be37ebc6080140f3704670b1aed")]
public partial class BossSpiralShootAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Projectile;
    BossFSM _fsm;

    float _duration = 5f;
    float _interval = 0.05f; // 연사 속도

    float _timer;
    float _shootTimer;
    float _angle;
    float _angleOffset = 10f;
    float _shootForce = 15f;

    protected override Status OnStart()
    {
        _timer = 0f;
        _shootTimer = 0f;
        _angle = 0f;

        if(_fsm == null)
            _fsm = Self.Value.GetComponent<BossFSM>();

        _fsm.RB.linearVelocity = Vector2.zero;
        AnimatorStateInfo animatorStateInfo = _fsm.Anim.GetCurrentAnimatorStateInfo(0);
        if (!animatorStateInfo.IsName("Shoot"))
        {
            _fsm.Anim.Play("Shoot");
        }

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        _timer += Time.deltaTime;
        _shootTimer += Time.deltaTime;

        if (_shootTimer >= _interval)
        {
            _shootTimer = 0f;
            FireProjectile(_angle);
            _angle += _angleOffset; // 나선 회전 각도 증가
        }

        //if (timer >= duration)
        //    EndAction(true);

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }

    void FireProjectile(float angle)
    {
        Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.right;
        GameObject proj = GameObject.Instantiate(Projectile.Value, Self.Value.transform.position, Quaternion.identity);
        proj.GetComponent<Rigidbody2D>().linearVelocity = dir.normalized * _shootForce;
    }
}