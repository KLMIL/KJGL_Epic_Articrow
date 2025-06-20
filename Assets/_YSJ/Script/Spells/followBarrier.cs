using UnityEngine;

public class followBarrier : Barrier
{
    float HoldingTime = 1f; // 베리어가 유지되는 시간
    float elapsedTime = 0f; // 경과 시간

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= HoldingTime)
        {
            TakeDamage(0); // 베리어가 유지 시간이 지나면 파괴
        }
    }
}
