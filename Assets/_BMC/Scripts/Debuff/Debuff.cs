using UnityEngine;

// 플레이어가 받는 디버프
public class Debuff : MonoBehaviour
{
    /// <summary>
    /// 디버프 적용 메서드 ex) 스턴, 화상 등
    /// </summary>
    /// <param name="duration"> 지속 시간 </param>
    /// <param name="damage"> 데미지 </param>
    /// <param name="interval"> 데미지 주기 </param>
    public virtual void Apply(float duration, float damage = 0f, float interval = 0f) { }
}