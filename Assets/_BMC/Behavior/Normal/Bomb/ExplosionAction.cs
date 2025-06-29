using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Explosion", story: "[Self] explosion", category: "Action", id: "2a044f4896c129118ad6872464e0805b")]
public partial class ExplosionAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;

    Animator _anim;

    protected override Status OnStart()
    {
        if(_anim == null)
            _anim = Self.Value.GetComponent<Animator>();
        _anim.Play("Explosion");
        Explode();

        return Status.Running;
    }

    //protected override Status OnUpdate()
    //{
    //    return Status.Success;
    //}

    //protected override void OnEnd()
    //{
    //}

    void Explode()
    {
        Collider2D collider2D = Physics2D.OverlapCircle(Self.Value.transform.position, 2f, LayerMask.GetMask("PlayerHurtBox"));
        if(collider2D != null)
        {
            collider2D.GetComponent<IDamagable>()?.TakeDamage(10);
        }
    }
}