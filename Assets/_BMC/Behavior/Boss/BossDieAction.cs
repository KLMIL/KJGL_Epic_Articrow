using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BossDie", story: "[Self] Die", category: "Action", id: "caa60688413d516d464ea1a88d7a593e")]
public partial class BossDieAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    Animator _anim;
    protected override Status OnStart()
    {
        if (_anim == null)
            _anim = Self.Value.GetComponent<Animator>();
        _anim.Play("Die");

        //// 파괴
        //AnimatorStateInfo currentStateInfo = _anim.GetCurrentAnimatorStateInfo(0);
        //GameObject.Destroy(Self.Value, currentStateInfo.length * 3);

        return Status.Running;
    }
}