using System.Collections.Generic;
using UnityEngine;

public class Skill_MagicPunch : MagicSkill
{
    private void Awake()
    {
        Init();
    }

    void Start()
    {
        magicPrefab = Resources.Load<GameObject>("Magic/MagicPunch");
        if (magicPrefab == null)
        {
            Debug.LogError("마법펀치 프리팹 찾을 수 없음");
        }
    }
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    public override void Fire(Transform fireMan, List<BuffSkill> buffs, float accuracy)
    {
        // 방향구하고
        Vector2 direction = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)fireMan.position).normalized;
        // 정확도 적용하고
        direction += new Vector2(Random.Range(-accuracy, accuracy), Random.Range(-accuracy, accuracy));

        GameObject spawnedObj = Instantiate(magicPrefab);
        spawnedObj.transform.SetParent(fireMan, true);
        spawnedObj.transform.localPosition = Vector2.zero + direction;
        spawnedObj.transform.rotation = Quaternion.Euler(0,0, Mathf.Atan2(direction.y, direction.x ) * Mathf.Rad2Deg - 90);
        foreach (BuffSkill buff in buffs)
        {
            buff.BuffShot(fireMan, spawnedObj);
        }

        // 발사체에 그 방향 적용
        spawnedObj.GetComponent<Magic>().direction = direction;
    }
}
