using BMC;
using System.Collections.Generic;
using UnityEngine;

public class HealthHeartBar : MonoBehaviour
{
    public GameObject heartPrefab;
    List<HealthHeart> hearts = new List<HealthHeart>();

    void Awake()
    {
        UI_InGameEventBus.OnPlayerHeartUpdate = DrawHearts;
    }

    void Start()
    {
        DrawHearts();
    }

    public void DrawHearts()
    {
        ClearHearts();

        // 최대 체력 기준으로 몇 개의 하트를 생성할지 결정
        float maxHealthRemainder = PlayerManager.Instance.PlayerStatus.MaxHealth % 2;
        int heartsToMake = (int)((PlayerManager.Instance.PlayerStatus.MaxHealth / 2) + maxHealthRemainder);
        for(int i=0; i<heartsToMake; i++)
        {
            CreateEmptyHeart();
        }

        // HP 하트
        for(int i=0; i<heartsToMake; i++)
        {
            // 만약에 4분의 1 하트가 있다면 4로 바꿔줘야 함
            int heartStatusRemainder = (int)Mathf.Clamp(PlayerManager.Instance.PlayerStatus.Health - (i * 2), 0, 2);
            hearts[i].SetHeartImage((HeartStatus)heartStatusRemainder);
        }

        // 배리어 수치를 기준으로 몇 개의 배리어 하트를 생성할지 결정
        float offsetBarrierRemainder = PlayerManager.Instance.PlayerStatus.OffsetBarrier % 2;
        int barrierToMake = (int)((PlayerManager.Instance.PlayerStatus.OffsetBarrier / 2) + offsetBarrierRemainder);
        //Debug.LogError("barrierToMake: " + barrierToMake);
        for (int i = 0; i < barrierToMake; i++)
        {
            CreateEmptyBarrierHeart();
        }

        // 보호막 하트
        for (int i = heartsToMake; i < heartsToMake + barrierToMake; i++)
        {
            int idx = i - heartsToMake; // 배리어 하트는 기존 하트 뒤에 추가
            int barrierStatusRemainder = (int)Mathf.Clamp(PlayerManager.Instance.PlayerStatus.OffsetBarrier - (idx * 2), 0, 2);
            //Debug.LogError("배리어 OffsetBarrier: " + PlayerManager.Instance.PlayerStatus.OffsetBarrier);
            //Debug.LogError("배리어 remainder: " + barrierStatusRemainder);
            //Debug.LogError((HeartStatus)barrierStatusRemainder);
            hearts[i].SetHeartImage((HeartStatus)barrierStatusRemainder);
        }
    }

    public void CreateEmptyHeart()
    {
        // 하트 UI 생성
        GameObject newHeart = Instantiate(heartPrefab, transform);
        newHeart.transform.SetParent(transform);

        // 하트 상태를 빈 상태로 설정
        HealthHeart heartComponent = newHeart.GetComponent<HealthHeart>();
        heartComponent.SetHeartImage(HeartStatus.Empty);
        hearts.Add(heartComponent);
    }

    public void CreateEmptyBarrierHeart()
    {
        // 하트 UI 생성
        GameObject newHeart = Instantiate(heartPrefab, transform);
        newHeart.transform.SetParent(transform);

        // 하트 상태를 빈 상태로 설정
        HealthHeart heartComponent = newHeart.GetComponent<HealthHeart>();
        heartComponent.SetHeartImage(HeartStatus.Empty);
        heartComponent.SetColor(Color.gray); // 배리어 하트는 회색으로 표시
        hearts.Add(heartComponent);
    }


    public void ClearHearts()
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<HealthHeart>();
    }

    void OnDestroy()
    {
        UI_InGameEventBus.OnPlayerHeartUpdate = null;
    }
}
