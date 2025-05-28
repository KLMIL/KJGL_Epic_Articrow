using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Skill_MagicBullet : MagicSkill
{
    public void Awake()
    {
        Init();

        magicPrefab = Resources.Load<GameObject>("Magic/MagicBullet");
        if (magicPrefab == null)
        {
            Debug.LogError("마법탄 프리팹 찾을 수 없음");
        }
    }

    public void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    public override void TryFire(Transform fireMan, List<BuffSkill> buffs, PlayerStatus playerStatus)
    {
        base.TryFire(fireMan, buffs, playerStatus);
        return;
    }
}
