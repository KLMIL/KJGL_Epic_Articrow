using BMC;
using System.Collections.Generic;
using UnityEngine;

public class ManaHeartBar : MonoBehaviour
{
    public GameObject ManaPrefab;
    List<ManaHeart> manas = new List<ManaHeart>();

    int _manasToMake = 0;

    void Awake()
    {
        UI_InGameEventBus.OnPlayerManaUpdate = DrawManas;
    }

    void Start()
    {
        DrawManas();
    }

    public void DrawManas()
    {
        float maxManaRemainder = PlayerManager.Instance.PlayerStatus.MaxMana % 2;
        int manasToMake = (int)((PlayerManager.Instance.PlayerStatus.MaxMana / 2) + maxManaRemainder);

        Debug.Log("생성해야할 마나 수: " + manasToMake);
        //if (_manasToMake == manasToMake)
        //    return;
        if(_manasToMake != manasToMake)
        {
            ClearManas();
            _manasToMake = manasToMake;

            for (int i = 0; i < manasToMake; i++)
            {
                CreateEmptyMana();
            }
        }

        for (int i = 0; i < manasToMake; i++)
        {
            int manaStatusRemainder = (int)Mathf.Clamp(PlayerManager.Instance.PlayerStatus.Mana - (i * 2), 0, 2);
            manas[i].SetHeartImage((ManaStatus)manaStatusRemainder);
        }
    }

    public void CreateEmptyMana()
    {
        // 마나 UI 생성
        GameObject newMana = Instantiate(ManaPrefab, transform);
        newMana.transform.SetParent(transform);

        // 마나 상태를 빈 상태로 설정
        ManaHeart manaComponent = newMana.GetComponent<ManaHeart>();
        manaComponent.SetHeartImage(ManaStatus.Empty);
        manas.Add(manaComponent);
    }

    public void ClearManas()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        manas = new List<ManaHeart>();
    }

    void OnDestroy()
    {
        UI_InGameEventBus.OnPlayerManaUpdate = null;
    }
}