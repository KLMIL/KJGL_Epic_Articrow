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
    
    //public float cooldown;
    /// <summary>
    /// 연속 사용 가능 여부
    /// </summary>
    public bool canRepeat = false;
    /// <summary>
    /// 패턴 Unlock 조건 (0 ~ 1, 1.0 = 항상 허용)
    /// </summary>
    public float hpUnlockRatio = 1.0f;
    
    //[HideInInspector] public float lastUsedTime = -999f;
}
