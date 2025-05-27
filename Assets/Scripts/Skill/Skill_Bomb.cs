using System.Collections.Generic;
using UnityEngine;

public class Skill_Bomb : MagicSkill
{
    public void Awake()
    {
        Init();

        magicPrefab = Resources.Load<GameObject>("Magic/Bomb");
        if (magicPrefab == null)
        {
            Debug.LogError("폭탄 프리팹 찾을 수 없음");
        }
    }

    public void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    public override void TryFire(Transform fireMan, List<BuffSkill> buffs, Mana currentMana)
    {
        base.TryFire(fireMan, buffs, currentMana);
        return;
    }
}
