using Unity.Behavior;

[BlackboardEnum]
public enum BossState
{
    Spawn,  // 소환
    Idle,   // 대기
	Rush,   // 돌진
    Hit,    // 피격
    Die     // 사망
}