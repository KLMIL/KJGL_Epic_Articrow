using Unity.Behavior;


/// <summary>
/// 보스 상태
/// </summary>
[BlackboardEnum]
public enum BossState
{
    None,
    Spawn,  // 소환
    Idle,   // 대기
	Rush,   // 돌진
    Stun,   // 기절
    Shoot,  // 발사
    Die,    // 사망
}