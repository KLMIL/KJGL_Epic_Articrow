using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using BMC;
using YSJ;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SpiralShoot", story: "[BossFSM] shoot [Projectile] spiral", category: "Action", id: "ad6c5be37ebc6080140f3704670b1aed")]
public partial class SpiralShootAction : Action
{
    [SerializeReference] public BlackboardVariable<BossFSM> BossFSM;
    [SerializeReference] public BlackboardVariable<GameObject> Projectile;
    Transform _shootTransform;

    float _interval = 0.05f; // 연사 속도

    float _shootTimer;
    float _shootStartAngle;     // 발사 시작 각도
    float _angleOffset = 10f;
    float _shootForce = 15f;

    protected override Status OnStart()
    {
        _shootTimer = 0f;
        _shootStartAngle = UnityEngine.Random.Range(0f, 360f);  // 시작 각도 랜덤

        if(_shootTransform == null)
            _shootTransform = BossFSM.Value.transform.Find("Visual/Core");

        BossFSM.Value.RB.linearVelocity = Vector2.zero;
        AnimatorStateInfo animatorStateInfo = BossFSM.Value.Anim.GetCurrentAnimatorStateInfo(0);
        if (!animatorStateInfo.IsName("Shoot"))
        {
            BossFSM.Value.Anim.Play("SpiralShoot");
        }

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        _shootTimer += Time.deltaTime;

        if (_shootTimer >= _interval)
        {
            _shootTimer = 0f;
            FireProjectile(_shootStartAngle);
            _shootStartAngle += _angleOffset; // 나선 회전 각도 증가
        }

        return Status.Running;
    }

    protected override void OnEnd()
    { }

    void FireProjectile(float angle)
    {
        Managers.Sound.PlaySFX(Define.SFX.GolemSpiral);
        Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.right;
        GameObject projectile = GameObject.Instantiate(Projectile.Value, _shootTransform.position, Quaternion.identity);
        projectile.transform.right = direction;
        projectile.GetComponent<Rigidbody2D>().linearVelocity = direction.normalized * _shootForce;
    }
}