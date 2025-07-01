using System;
using Unity.Behavior;

[BlackboardEnum]
public enum GolemBossState
{
    None,
	Spawn,
	Idle,
	Move,
	Rush,
	Stun,
	Shoot,
    Smash,
	Die
}