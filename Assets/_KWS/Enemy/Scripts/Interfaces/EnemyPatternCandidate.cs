using UnityEngine;

[System.Serializable]
public class EnemyPatternCandidate
{
    /// <summary>
    /// FSM의 상태 이름
    /// </summary>
    public string patternStateName;
    /// <summary>
    /// 확률 가중치
    /// </summary>
    public float probability;
    /// <summary>
    /// 패턴 쿨타임(초단위)
    /// </summary>
    public float cooldown;
    /// <summary>
    /// 연속 사용 가능 여부
    /// </summary>
    public bool canRepeat = false;
    /// <summary>
    /// 패턴 Unlock 조건 (0 ~ 1, 1.0 = 항상 허용)
    /// </summary>
    public float hpUnlockRatio = 1.0f;
    /// <summary>
    /// 런타임 관리용 잔여 쿨타임
    /// </summary>
    [HideInInspector] public float lastUsedTime = -999f;
}
