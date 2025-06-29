using System;
using Unity.Behavior;

[BlackboardEnum]
public enum BombState
{
    None,
	Idle,
    Wander,
	Chase,
	Explosion
}