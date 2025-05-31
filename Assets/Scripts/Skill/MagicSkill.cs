using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MagicSkill : CanEnterSlot
{
    public int FireCount = 1;
    public float accuracy = 0;

    public int addFireCount = 0;
    public float addAccuracy = 0;

    public string skillName;
    public float coolTime;
    public GameObject magicPrefab;
    public float consumeMana;
    protected float timer = 0f;
    protected List<BuffSkill> shotBuffs;

    public virtual void TryFire(Transform fireMan, List<BuffSkill> buffs, PlayerStatus playerStatus)
    {
        if (timer > 0)
        {
            print("아직 쿨타임!");
            return;
        }

        // 버프 한번 돌면서 적용
        foreach (BuffSkill buff in buffs)
        {
            buff.Buff(this);
        }

        int totalFireCount = this.FireCount + addFireCount;
        float totalAccuracy = this.accuracy + addAccuracy;

        for (int i = 0; i < totalFireCount; i++)
        {
            Fire(fireMan, buffs, totalAccuracy);
        }

        // 버프 내용 초기화
        addFireCount = 0;
        addAccuracy = 0;

        // 쿨돌려
        timer = coolTime;
    }

    public virtual void Fire(Transform fireMan, List<BuffSkill> buffs, float accuracy)
    {
        GameObject spawnedObj = Instantiate(magicPrefab, fireMan.position, Quaternion.identity);
        foreach (BuffSkill buff in buffs)
        {
            buff.BuffShot(fireMan, spawnedObj);
        }

        // 방향구하고
        Vector2 direction = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)fireMan.position).normalized;
        // 정확도 적용하고
        direction += new Vector2(Random.Range(-accuracy, accuracy), Random.Range(-accuracy, accuracy));
        // 발사체에 그 방향 적용
        spawnedObj.GetComponent<Magic>().direction = direction;
    }
}
