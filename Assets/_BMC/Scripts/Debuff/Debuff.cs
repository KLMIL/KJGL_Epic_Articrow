using UnityEngine;

// 플레이어가 받는 디버프
public abstract class Debuff : MonoBehaviour
{
    // 디버프 로직
    public abstract void Do(float time);
}