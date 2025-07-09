using UnityEngine;

public class AnimationEndDestorySelf_YSJ : MonoBehaviour
{
    // 애니메이션 클립에서 실행
    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
